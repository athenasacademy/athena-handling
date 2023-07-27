namespace AthenasAcademy.Handling.Repositories.Interfaces;

public interface IContratoAlunoRepository
{
    Task RegistrarContratoAluno(int codigoMatricula, string caminhoPDF);
}