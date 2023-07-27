
using System.Globalization;
using Aspose.Cells;
using Aspose.Cells.Rendering;
using AthenasAcademy.Handling.MessageEvents;
using AthenasAcademy.Handling.Models;
using AthenasAcademy.Handling.Repositories;
using AthenasAcademy.Handling.Repositories.Interfaces;
using AthenasAcademy.Handling.Secrets;
using AthenasAcademy.Handling.Services.Interfaces;
using ClosedXML.Excel;


namespace AthenasAcademy.Handling.Services;

public class BoletoAlunoService : IBoletoAlunoService
{
    private AppSecrets _secrets;

    public BoletoAlunoService(AppSecrets secrets)
    {
        _secrets = secrets;
    }

    public async Task<bool> GerarBoletoPDF(BoletoEventMessage boletoEvent)
    {
        string xlsx = "boleto_" +
            boletoEvent.Pagador.CPF.Replace("-", "").Replace(".", "") +
            DateTime.ParseExact(boletoEvent.DataVencimento, "dd/MM/yyyy",
            CultureInfo.InvariantCulture).ToString("ddMMyyyy") + ".xlsx";

        string pdf = xlsx.Replace(".xlsx", ".pdf");
        string png = xlsx.Replace(".xlsx", ".png");

        string entrada = Path.Combine(
            Directory.GetCurrentDirectory(),
            Path.Combine("FileModels", "modelo-boleto.xlsx"));

        string saida = Path.Combine(
            Directory.GetCurrentDirectory(),
            Path.Combine("FileModels", "Saida", "Boleto"));

        BoletoModel boleto = new()
        {
            DataVencimento = boletoEvent.DataVencimento,
            ValorDocumento = (decimal)boletoEvent.ValorDocumento,
            Pagador = new PagadorModel
            {
                Nome = boletoEvent.Pagador.Nome,
                CPF = boletoEvent.Pagador.CPF,
                Logradouro = boletoEvent.Pagador.Logradouro,
                Numero = boletoEvent.Pagador.Numero,
                Complemento = boletoEvent.Pagador.Complemento,
                Bairro = boletoEvent.Pagador.Bairro,
                CEP = boletoEvent.Pagador.CEP,
                UF = boletoEvent.Pagador.UF
            }
        };

        Dictionary<string, string> posicoesTexto = ObterPosicoesTextoRegra(boleto);

        EscreverTextosEmCelulas(entrada, Path.Combine(saida, xlsx), "default", posicoesTexto);

        GeerarPDF(Path.Combine(saida, xlsx));
     
        IAwsS3Repository s3repositiry = new AwsS3Repository(_secrets);
        string caminhoPDF = await s3repositiry.EnviarPDFAsync("boletos/PDF", Path.Combine(saida, pdf));
        string caminhoPNG = await s3repositiry.EnviarPNGAsync("boletos/PNG", Path.Combine(saida, png));

        LimparSaida(saida);

        await new BoletoAlunoRepository(_secrets).RegistrarBoletoCandidato(boletoEvent.CodigoInscricao, caminhoPDF);

        return await Task.FromResult(true);
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

    private static Dictionary<string, string> ObterPosicoesTextoRegra(BoletoModel boleto)
    {
        return new Dictionary<string, string>()
        {
            {"B6", boleto.TextoLocalPagamento},
            {"B8", boleto.Beneficiario},
            {"B10", boleto.DataDocumento},
            {"B18", boleto.Pagador.Nome},
            {"B19", boleto.Pagador.Logradouro},
            {"B20", boleto.Pagador.CEP},

            {"B24", boleto.CodigoDeBarras},

            {"C10", boleto.NumeroDocumento},
            {"C12", boleto.Carteira},
            {"C14", boleto.TextoCampoLivre[0]},
            {"C15", boleto.TextoCampoLivre[1]},
            {"C16", boleto.TextoCampoLivre[2]},

            {"D3", "|" + boleto.Banco + "|"},

            {"E3", boleto.CodigoDeBarras},
            {"E10", boleto.Especie},
            {"E12", boleto.EspecieMoeda},

            {"F10", boleto.Aceite},

            {"G10", boleto.DataDocumento},

            {"H6", boleto.DataVencimento},
            {"H8", boleto.AgenciaConta},
            {"H10", boleto.NossoNumero},
            {"H12", string.Format("R$ {0}", boleto.ValorDocumento.ToString("F2"))}
        };
    }

    private static void EscreverTextosEmCelulas(string entrada, string saida, string guia, Dictionary<string, string> dictPosicoesTexto)
    {
        try
        {
            using (var workbook = new XLWorkbook(entrada))
            {
                var worksheet = workbook.Worksheet(guia) ?? workbook.Worksheets.Add(guia);

                foreach (var posicaoTexto in dictPosicoesTexto)
                {
                    worksheet.Cell(posicaoTexto.Key).Value = posicaoTexto.Value;
                }

                workbook.SaveAs(saida);

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorreu um erro: " + ex.Message);
        }
    }

    private static void GeerarPDF(string saida)
    {
        Workbook workbookAspose = new Workbook(saida);
        PdfSaveOptions pdfSaveOptions = new PdfSaveOptions
        {
            OnePagePerSheet = true,
            AllColumnsInOnePagePerSheet = true,
            Compliance = PdfCompliance.PdfA1b
        };

        workbookAspose.Save(saida.Replace(".xlsx", ".pdf"), pdfSaveOptions);

        ImageOrPrintOptions imgOptions = new ImageOrPrintOptions
        {
            OnePagePerSheet = true
        };

        imgOptions.ImageType = Aspose.Cells.Drawing.ImageType.Png;

        var sheetRender = new SheetRender(workbookAspose.Worksheets[0], imgOptions);
        sheetRender.ToImage(0, saida.Replace(".xlsx", ".png"));
    }
}