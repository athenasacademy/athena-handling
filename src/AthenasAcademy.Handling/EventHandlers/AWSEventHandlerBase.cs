using Amazon;
using Amazon.SQS;
using AthenasAcademy.Handling.Secrets;

namespace AthenasAcademy.Handling.EventHandlers;

public class AWSEventHandlerBase
{
    private AWSSecrets _secrets;

    protected AWSEventHandlerBase(AWSSecrets secrets)
    {
        _secrets = secrets;
    }

    protected AmazonSQSClient GetClient(string queueUrl)
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