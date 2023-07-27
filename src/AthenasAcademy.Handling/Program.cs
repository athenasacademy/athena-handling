using AthenasAcademy.Handling.Secrets;
using AthenasAcademy.Handling.Services;
using AthenasAcademy.Handling.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AthenasAcademy.Handling;

public class Program
{
    public static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddEnvironmentVariables()
          .AddCommandLine(args)
          .Build();

        AWS secretsAWS = new();
        DatabaseConnection secretsDatabaseConnection = new();

        configuration.GetSection("AWS").Bind(secretsAWS);
        configuration.GetSection("DatabaseConnection").Bind(secretsDatabaseConnection);

        AppSecrets secrets = new()
        {
            AWS = secretsAWS,
            DatabaseConnection = secretsDatabaseConnection
        };

        IQueueConsumerService consomer = new QueueConsumerService(secrets);
        await consomer.IniciarServico();
    }
}