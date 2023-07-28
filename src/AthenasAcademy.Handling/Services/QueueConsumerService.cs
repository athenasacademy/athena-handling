using AthenasAcademy.Handling.EventHandlers;
using AthenasAcademy.Handling.EventHandlers.Interfaces;
using AthenasAcademy.Handling.Secrets;
using AthenasAcademy.Handling.Services.Interfaces;

namespace AthenasAcademy.Handling.Services;

class QueueConsumerService : IQueueConsumerService
{
    private AppSecrets _secrets;

    public QueueConsumerService(AppSecrets secrets)
    {
        _secrets = secrets;
    }

    public async Task IniciarServico()
    {
        try
        {
            Task taskContrato = TaskConsumerContrato();
            Task taskBoleto = TaskConsumerBoleto();
            Task.WhenAll(taskContrato, taskBoleto).Wait();
            Console.WriteLine("Fim do programa.");
            await Task.FromResult(true);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro ou mais. {ex.Message}");
            await IniciarServico();
        }
    }

    public async Task TaskConsumerBoleto()
    {
        while (true)
        {
            IBoletoEventHandler boletoEventHandler = new BoletoEventHandler(_secrets);
            await boletoEventHandler.Handle();
            await Task.Delay(100);
        }
    }

    public async Task TaskConsumerContrato()
    {
        while (true)
        {
            IContratoEventHandler contratoEventHandler = new ContratoEventHandler(_secrets);
            await contratoEventHandler.Handle();
            await Task.Delay(1000);
        }
    }
}