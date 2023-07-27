using AthenasAcademy.Handling.Abstractions;

namespace AthenasAcademy.Handling.Models;

public class BoletoModel : BoletoAbstract
{
    public string DataVencimento { get; set; }

    public decimal  ValorDocumento { get; set; }

    public PagadorModel Pagador { get; set; }

    public string CodigoDeBarras
    {
        get => string.Format(
            "{0}{1}",
            this.LinhaDigitavel, 
            string.Format(
                "{0}",
                this.ValorDocumento.ToString().Replace(",", string.Empty)).PadLeft(11, '0')
            );
    }

    public string[] TextoCampoLivre
    {
        get
        {
            return new[]
            {
                "Instruções (Texto de responsabilidade do Beneficiário)",
                string.Format("Após {0} cobrar juros de {1} ao mes.", this.DataVencimento, this.Juros),
                string.Format("Após {0} cobrar multa de {1}.", this.DataVencimento, this.Multa),
            };
        }
    }
}