using AthenasAcademy.Handling.Secrets;
using Npgsql;

namespace AthenasAcademy.Handling.Repositories;

public class BaseRepository
{
    private static DatabaseConnection _configuration;
    private static NpgsqlConnection _connection;
    public BaseRepository(DatabaseConnection configuration)
    {
        _configuration = configuration;
    }

    public NpgsqlConnection GetConnection()
    {
        try
        {
            NpgsqlConnectionStringBuilder builder = new(_configuration.AthenasDatabaseConnection);

            builder.MaxPoolSize = 100;
            builder.MinPoolSize = 1;
            builder.ConnectionIdleLifetime = 500;
            builder.Pooling = true;

            _connection = new NpgsqlConnection(builder.ConnectionString);

             _connection.Open();
            return _connection;
        }
        catch (Exception ex) { throw ex; }
    }
}