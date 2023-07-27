using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using AthenasAcademy.Handling.EventHandlers.Interfaces;
using AthenasAcademy.Handling.MessageEvents;
using AthenasAcademy.Handling.Secrets;
using AthenasAcademy.Handling.Services;

namespace AthenasAcademy.Handling.EventHandlers;

public class BoletoEventHandler : AWSEventHandlerBase, IBoletoEventHandler
{
    private AppSecrets _secrets;

    public BoletoEventHandler(AppSecrets secrets) : base(secrets)
    {
        _secrets = secrets;
    }
    public async Task Handle()
    {
        string queueUrl = _secrets.AWS.SQS.Queues.QueueBoleto;

        try
        {
            AmazonSQSClient cliente = GetClient(queueUrl);
            ReceiveMessageRequest receiver = new ReceiveMessageRequest { QueueUrl = queueUrl };
            var request = await cliente.ReceiveMessageAsync(receiver);
            if (request.Messages.Any())
                GerarBoleto(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    private async void GerarBoleto(ReceiveMessageResponse @event)
    {
        string json = @event.Messages.First().Body;
        Console.Write($"[Nova Menssagem] Boleto: {json}");

        BoletoEventMessage boletoEvent = JsonSerializer.Deserialize<BoletoEventMessage>(json);
        await new BoletoAlunoService(_secrets).GerarBoletoPDF(boletoEvent);

        Console.Write($"[Boleto Aguardando Nova Menssagem]");
        return;
    }
}