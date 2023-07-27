using Microsoft.Extensions.Configuration;

namespace AthenasAcademy.Handling.Services.Interfaces;

public interface IQueueConsumerService
{
    Task IniciarServico();
}