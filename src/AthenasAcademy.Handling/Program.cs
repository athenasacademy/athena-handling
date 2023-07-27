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

        IQueueConsumerService consomer = new QueueConsumerService(configuration);
        await consomer.IniciarServico();
    }
}