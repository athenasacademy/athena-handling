using Amazon;
using Amazon.SQS;
using AthenasAcademy.Handling.Secrets;

namespace AthenasAcademy.Handling.EventHandlers;

public class AWSEventHandlerBase
{
    private AppSecrets _secrets;

    protected AWSEventHandlerBase(AppSecrets secrets)
    {
        _secrets = secrets;
    }

    protected AmazonSQSClient GetClient(string queueUrl)
    {
        string accessKey = _secrets.AWS.AccessKey;
        string secretKey = _secrets.AWS.SecretKey;

        AmazonSQSConfig sqsConfig = new AmazonSQSConfig
        {
            ServiceURL = queueUrl,
            RegionEndpoint = RegionEndpoint.USWest2
        };
        return new AmazonSQSClient(accessKey, secretKey, sqsConfig);
    }
}