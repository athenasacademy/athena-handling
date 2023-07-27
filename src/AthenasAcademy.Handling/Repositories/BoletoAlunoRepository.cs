using System.Data;
using AthenasAcademy.Handling.Repositories.Interfaces;
using AthenasAcademy.Handling.Secrets;
using Dapper;

namespace AthenasAcademy.Handling.Repositories;

public class BoletoAlunoRepository : BaseRepository, IBoletoAlunoRepository
{
    private AppSecrets _secrets;

    public BoletoAlunoRepository(AppSecrets secrets) : base(secrets.DatabaseConnection)
    {
        _secrets = secrets;
    }

    public async Task RegistrarBoletoCandidato(int codigoInscricao, string caminhoPDF)
    {

        var connectionString = _secrets.DatabaseConnection.AthenasDatabaseConnection;

        await Task.FromResult(true);
        try
        {
            using (IDbConnection connection = GetConnection())
            {
                string query = "UPDATE inscricao_candidato SET boleto = @NovoBoleto WHERE codigo_inscricao = @CodigoInscricao;";
                await connection.ExecuteAsync(query, new { CodigoInscricao = codigoInscricao, NovoBoleto = caminhoPDF });
            }
        }
        catch (Exception ex) { throw ex; }
    }
}
