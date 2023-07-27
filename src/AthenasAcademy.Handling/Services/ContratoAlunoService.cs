using System.Text;
using AthenasAcademy.Handling.MessageEvents;
using AthenasAcademy.Handling.Repositories;
using AthenasAcademy.Handling.Repositories.Interfaces;
using AthenasAcademy.Handling.Secrets;
using AthenasAcademy.Handling.Services.Interfaces;

namespace AthenasAcademy.Handling.Services;

public class ContratoAlunoService : IContratoAlunoService
{
    private AppSecrets _secrets;

    public ContratoAlunoService(AppSecrets secrets)
    {
        _secrets = secrets;
    }

    public async Task<bool> GerarContratoPDF(ContratoMessageEvent contratoEvent)
    {
        string doc = "contrato_" +
            contratoEvent.Aluno.CPF.Replace("-", "").Replace(".", "") +
            DateTime.Now.ToString("ddMMyyyy") + ".doc";

        string pdf = doc.Replace(".doc", ".pdf");

        string entrada = Path.Combine(
            Directory.GetCurrentDirectory(),
            Path.Combine("FileModels", "modelo-contrato.doc"));

        string saida = Path.Combine(
            Directory.GetCurrentDirectory(),
            Path.Combine("FileModels", "Saida", "Contrato"));

        string texto = GerarTextoContrato(contratoEvent);

        EscreverContratoEmArquivoDoc(entrada, Path.Combine(saida, doc), texto);

        // GerarPDF(entrada, Path.Combine(saida, pdf))

        IAwsS3Repository s3repositiry = new AwsS3Repository(_secrets);
        string caminhoPDF = await s3repositiry.EnviarPDFAsync("contratos/PDF", Path.Combine(saida, pdf));

        LimparSaida(saida);

        await new ContratoAlunoRepository(_secrets).RegistrarContratoAluno(contratoEvent.CodigoContrato, caminhoPDF);

        return await Task.FromResult(true);
    }

    public void EscreverContratoEmArquivoDoc(string textoContrato, string entrada, string saida)
    {
        // using (Xceed.Words.NET.DocX doc = Xceed.Words.NET.DocX.Load(entrada))
        // {
        //     doc.InsertParagraph(17, textoContrato, false);
        //     doc.SaveAs(saida);
        // }
    }

    public string GerarTextoContrato(ContratoMessageEvent contrato)
    {
        StringBuilder textoContrato = new StringBuilder();
        textoContrato.AppendLine();
        textoContrato.AppendLine($"CONTRATO DE PRESTAÇÃO DE SERVIÇOS EDUCACIONAIS: {contrato.CodigoContrato.ToString().PadLeft(10, '0')}");
        textoContrato.AppendLine();
        textoContrato.AppendLine($"Pelo presente instrumento particular, de um lado, a Athenas Academy, pessoa jurídica de direito privado, CNPJ 00.000.000/0000-00, com sede na Rua das Escolas, 123, Cidade-Escola, Estado-UF, CEP 12345-678, doravante denominada CONTRATADA, e de outro lado, o(a) Sr(a). {contrato.Aluno.Nome}, CPF {contrato.Aluno.CPF}, sexo {contrato.Aluno.Sexo}, nascido(a) em {contrato.Aluno.DataNascimento}, e-mail {contrato.Aluno.Email}, doravante denominado(a) CONTRATANTE, têm entre si, justas e acordadas, as seguintes cláusulas:");
        textoContrato.AppendLine();
        textoContrato.AppendLine("1. OBJETO");
        textoContrato.AppendLine($"1.1. O presente contrato tem como objeto a prestação de serviços educacionais para o(a) CONTRATANTE no curso de {contrato.Curso.Nome}, na modalidade presencial, com carga horária total de {contrato.Curso.CargaHoraria} horas.");
        textoContrato.AppendLine();
        textoContrato.AppendLine("Outras cláusulas e informações podem ser adicionadas conforme necessário.");
        textoContrato.AppendLine();
        textoContrato.AppendLine("2. VALOR DO CONTRATO");
        textoContrato.AppendLine($"2.1. O valor total do contrato é de R$ {contrato.ValorContrato}.");
        textoContrato.AppendLine();
        textoContrato.AppendLine("3. FORMA DE PAGAMENTO");
        textoContrato.AppendLine("3.1. O pagamento do valor total do contrato será feito através de boleto bancário, gerado mensalmente e enviado ao(a) CONTRATANTE para o endereço fornecido.");
        textoContrato.AppendLine();
        textoContrato.AppendLine("4. ASSINATURA");
        textoContrato.AppendLine("4.1. O(a) CONTRATANTE declara ciência e concordância com todas as cláusulas e condições do presente contrato, estando de acordo com os termos estabelecidos.");
        textoContrato.AppendLine("4.2. O presente contrato entra em vigor a partir da data de assinatura e terá validade durante todo o período do curso escolhido.");
        textoContrato.AppendLine("4.3. O(a) CONTRATANTE declara estar ciente de seus direitos e deveres enquanto aluno(a) da Athenas Academy e se compromete a cumprir todas as normas e regulamentos da instituição.");
        textoContrato.AppendLine();
        textoContrato.AppendLine();
        textoContrato.AppendLine($"Athenas Academy, {FormatarData(DateTime.Now)}");
        textoContrato.AppendLine();
        textoContrato.AppendLine();
        textoContrato.AppendLine("________________________________________________________");
        textoContrato.AppendLine($"{contrato.Aluno.Nome} {contrato.Aluno.CPF}");
        textoContrato.AppendLine();
        textoContrato.AppendLine();
        textoContrato.AppendLine("________________________________________________________");
        textoContrato.AppendLine($"Saori Kido - CEO");

        return textoContrato.ToString();
    }

    private void LimparSaida(string diretorio)
    {
        try
        {
            if (!Directory.Exists(diretorio))
                return;

            foreach (string arquivo in Directory.GetFiles(diretorio))
                File.Delete(arquivo);

            Console.WriteLine($"Diretorio {diretorio} limpo.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao apagar arquivos: {ex.Message}");
        }
    }

    private string FormatarData(DateTime data)
    {
        string[] meses = new string[] {
        "", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
        "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
    };

        string dataFormatada = $"{data.Day} de {meses[data.Month]} de {data.Year}";

        return dataFormatada;
    }

}