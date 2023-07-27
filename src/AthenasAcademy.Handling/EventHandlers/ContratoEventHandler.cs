using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using AthenasAcademy.Handling.EventHandlers.Interfaces;
using AthenasAcademy.Handling.Secrets;
using Microsoft.Extensions.Configuration;

namespace AthenasAcademy.Handling.EventHandlers;

public class ContratoEventHandler : IContratoEventHandler
{
    private AWSSecrets _secrets;

    public ContratoEventHandler(AWSSecrets secrets)
    {
        _secrets = secrets;
    }
    public async Task Handle()
    {
        string queueUrl = _secrets.SQS.Queues.QueueContrato;

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

    private AmazonSQSClient GetClient(string queueUrl)
    {
        string accessKey = _secrets.AccessKey;
        string secretKey = _secrets.SecretKey;

        AmazonSQSConfig sqsConfig = new AmazonSQSConfig
        {
            ServiceURL = queueUrl,
            RegionEndpoint = RegionEndpoint.USWest2
        };
        return new AmazonSQSClient(accessKey, secretKey, sqsConfig);
    }
}