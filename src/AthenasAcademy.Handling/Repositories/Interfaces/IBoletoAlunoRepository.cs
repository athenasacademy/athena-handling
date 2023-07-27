namespace AthenasAcademy.Handling.Repositories.Interfaces;

public interface IBoletoAlunoRepository
{
    Task RegistrarBoletoCandidato(int codigoInscricao, string caminhoPDF);
}