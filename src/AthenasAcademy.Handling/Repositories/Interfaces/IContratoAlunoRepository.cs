namespace AthenasAcademy.Handling.Repositories.Interfaces;

public interface IContratoAlunoRepository
{
    Task SalvarRegistrarContrato(int codigoMatricula, string caminhoPDF);
}