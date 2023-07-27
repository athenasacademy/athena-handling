namespace AthenasAcademy.Handling.Abstractions;

public abstract class BoletoAbstract
{
    private string agencia => "0666-1";

    private string conta => "11230-1";

    public string Juros
    {
        get => "0,99%";
    }

    public string Multa
    {
        get => "2,00%";
    } 

    public string Banco
    {
        get => "999-0";
    }

    public string TextoLocalPagamento
    {
        get => "Pagar em qualquer banco até o vencimento";
    }

    public string Beneficiario
    {
        get => "ATHENAS ACADEMY SCHOOL LTDA.";
    }

    public string NossoNumero
    {
        get => "123456789-0";
    }

    public string Aceite
    {
        get => "SIM";
    }

    public string Especie
    {
        get => "Mensalidade escolar";
    }

    public string DataDocumento
    {
        get => DateTime.Now.ToString("dd/MM/yyyy");
    }

    public string NumeroDocumento
    {
        get => "12345678";
    }

    public string Carteira
    {
        get => "00";
    }

    public string EspecieMoeda
    {
        get => "REAL";
    }

    public string AgenciaConta
    {
        get => string.Format("{0}/{1}", this.agencia, this.conta);
    }

    public string LinhaDigitavel
    {
        get => string.Format(
            "{0}.{1} {2}.{3} {4}.{5} {6} ",
            Banco.Split("-")[0].PadLeft(4, '0'), // OK - 00190
            string.Format("{0}{1}", NossoNumero.Split('-')[0].Substring(0, 4), "9"), // OK - 46135
            string.Format("{0}{1}", NossoNumero.Split('-')[0].Substring(4, 4), "9"), // OK - 73607

            string.Format("9{0}9", agencia.Split('-')[0]), // OK - 534044
            
            string.Format("000{0}", conta.Split('-')[0].Substring(0, 2)), // OK - 00056
            string.Format(conta.Split('-')[0].Substring(conta.Split('-')[0].Length - 3) + "099"), // OK - 273311

            Banco.Split("-")[1] // OK - 9
            );
    }
}