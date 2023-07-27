namespace AthenasAcademy.Handling.Secrets;

public class AWSSecrets
{
    public string AccessKey { get; set; }

    public string SecretKey { get; set; }

    public string Region { get; set; }

    public AWSSQSSecrets SQS { get; set; }

    public AWSS3Secrets S3 { get; set; }
}

public class AWSSQSSecrets
{
    public Queues Queues { get; set; }
}

public class Queues
{
    public string QueueBoleto { get; set; }

    public string QueueContrato { get; set; }
}

public class AWSS3Secrets
{
    public string BucketName { get; set; }
}