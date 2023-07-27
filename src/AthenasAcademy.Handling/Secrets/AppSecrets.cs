namespace AthenasAcademy.Handling.Secrets;


public class AppSecrets
{
    public AWS AWS { get; set; }
    public DatabaseConnection DatabaseConnection { get; set; }
}

public class DatabaseConnection
{
    public string AthenasDatabaseConnection { get; set; }
}

public class AWS
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