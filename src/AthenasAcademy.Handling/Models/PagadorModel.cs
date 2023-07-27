namespace AthenasAcademy.Handling.Models;

public class PagadorModel
{
    private string _nome;
    private string _logradouro;
    private string _cep;

    public string Nome
    {
        get { return string.Format("Pagador {0} {1}", _nome.ToUpper(), string.Format("CPF {0}", this.CPF)); }
        set { _nome = value; }
    }

    public string Logradouro
    {
        get { return string.Format("{0} {1} {2} {3} {4}", _logradouro, this.Numero, this.Complemento, this.Bairro, this.UF); }
        set { _logradouro = value; }
    }
    
    public string CEP
    {
        get { return string.Format("{0} {1} {2}", this.Bairro, this.UF, this._cep); }
        set { _cep = value; }
    }
    
    public string CPF { get; set; }

    public string Numero { get; set; }

    public string Complemento { get; set; }

    public string Bairro { get; set; }

    public string UF { get; set; }
}