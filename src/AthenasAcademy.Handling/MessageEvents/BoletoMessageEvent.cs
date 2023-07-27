namespace AthenasAcademy.Handling.MessageEvents;

public class BoletoEventMessage
{
    public int CodigoInscricao { get; set; }

    public string DataVencimento { get; set; }

    public decimal ValorDocumento { get; set; }

    public Pagador Pagador { get; set; }
}

public class Pagador
{
    public string Nome { get; set; }

    public string CPF { get; set; }

    public string Logradouro { get; set; }

    public string Numero { get; set; }

    public string Complemento { get; set; }

    public string Bairro { get; set; }

    public string CEP { get; set; }

    public string UF { get; set; }
}
