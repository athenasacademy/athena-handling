namespace AthenasAcademy.Handling.Repositories.Interfaces;

public interface IBoletoAlunoRepository
{
    Task SalvarRegistrarBoleto(int codigoInscricao, string caminhoPDF);
}