using System.Data;
using AthenasAcademy.Handling.Repositories.Interfaces;
using AthenasAcademy.Handling.Secrets;
using Dapper;

namespace AthenasAcademy.Handling.Repositories;

public class ContratoAlunoRepository : BaseRepository, IContratoAlunoRepository
{
    private AppSecrets _secrets;

    public ContratoAlunoRepository(AppSecrets secrets) : base(secrets.DatabaseConnection)
    {
        _secrets = secrets;
    }

    public async Task RegistrarContratoAluno(int codigoContrato, string caminhoPDF)
    {

        var connectionString = _secrets.DatabaseConnection.AthenasDatabaseConnection;

        await Task.FromResult(true);
        try
        {
            using (IDbConnection connection = GetConnection())
            {
                string query = "UPDATE contrato_matricula_aluno SET dir_contrato_matricula = @ContratoMatricula WHERE contrato = @Contrato;";
                await connection.ExecuteAsync(query, new { Contrato = codigoContrato, ContratoMatricula = caminhoPDF });
            }
        }
        catch (Exception ex) { Console.WriteLine($"Erro ao registrar processo de geração do contrato. {ex.Message}"); }
    }
}
