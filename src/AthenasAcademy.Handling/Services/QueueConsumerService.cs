using AthenasAcademy.Handling.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AthenasAcademy.Handling.Services;

class QueueConsumerService : IQueueConsumerService
{
    private IConfiguration _configuration;

    public QueueConsumerService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task IniciarServico()
    {
        await Task.FromResult(true);
    }
}