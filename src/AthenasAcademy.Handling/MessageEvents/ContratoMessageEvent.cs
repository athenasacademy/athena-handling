namespace AthenasAcademy.Handling.MessageEvents;

public class ContratoMessageEvent
{
    public int CodigoContrato { get; set; }

    public decimal ValorContrato { get; set; }

    public Aluno Aluno { get; set; }

    public Curso Curso { get; set; }
}

public class Aluno
{
    public string Nome { get; set; }

    public string CPF { get; set; }

    public string Sexo { get; set; }

    public string DataNascimento { get; set; }

    public string Email { get; set; }
}

public class Curso
{
    public string Nome { get; set; }

    public int CargaHoraria { get; set; }
}