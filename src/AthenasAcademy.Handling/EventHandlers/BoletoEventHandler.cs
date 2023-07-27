using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using AthenasAcademy.Handling.EventHandlers.Interfaces;
using AthenasAcademy.Handling.Secrets;

namespace AthenasAcademy.Handling.EventHandlers;

public class BoletoEventHandler : AWSEventHandlerBase, IBoletoEventHandler
{
    private AWSSecrets _secrets;

    public BoletoEventHandler(AWSSecrets secrets) : base(secrets)
    {
        _secrets = secrets;
    }
    public async Task Handle()
    {
        string queueUrl = _secrets.SQS.Queues.QueueBoleto;

        try
        {
            AmazonSQSClient cliente = GetClient(queueUrl);
            ReceiveMessageRequest receiver = new ReceiveMessageRequest { QueueUrl = queueUrl };
            await cliente.ReceiveMessageAsync(receiver);
        }
        catch (Exception ex )
        { 
            Console.WriteLine(ex.Message);
        }
    }
}