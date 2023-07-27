using Amazon.SQS;
using Amazon.SQS.Model;
using AthenasAcademy.Handling.EventHandlers.Interfaces;
using AthenasAcademy.Handling.Secrets;

namespace AthenasAcademy.Handling.EventHandlers;

public class ContratoEventHandler : AWSEventHandlerBase, IContratoEventHandler
{
    private AppSecrets _secrets;

    public ContratoEventHandler(AppSecrets secrets) : base(secrets)
    {
        _secrets = secrets;
    }
    public async Task Handle()
    {
        string queueUrl = _secrets.AWS.SQS.Queues.QueueContrato;

        try
        {
            AmazonSQSClient cliente = GetClient(queueUrl);
            ReceiveMessageRequest receiver = new ReceiveMessageRequest { QueueUrl = queueUrl };
            var request = await cliente.ReceiveMessageAsync(receiver);
            if (request.Messages.Any())
                GerarContrato(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async void GerarContrato(ReceiveMessageResponse @event)
    {
        string json = @event.Messages.First().Body;
        Console.Write($"[Nova Menssagem] Contrato: {json}");

        // BoletoEventMessage boletoEvent = JsonSerializer.Deserialize<BoletoEventMessage>(json);
        // await new BoletoAlunoService().GerarBoletoPDF(boletoEvent);
        
        Console.Write($"[Contrato Aguardando Nova Menssagem]");
    }
}