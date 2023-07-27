using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using AthenasAcademy.Handling.Repositories.Interfaces;
using AthenasAcademy.Handling.Secrets;

namespace AthenasAcademy.Handling.Repositories;

public class AwsS3Repository : IAwsS3Repository
{
    private AppSecrets _secrets;

    public AwsS3Repository(AppSecrets secrets)
    {
        _secrets = secrets;
    }

    public async Task<string> EnviarPDFAsync(string arquivo, string caminho)
    {
        try
        {
            using (AmazonS3Client client = GetClient())
            {
                using (TransferUtility utility = new TransferUtility(client))
                {
                    TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

                    request.BucketName = _secrets.AWS.S3.BucketName;
                    request.FilePath = arquivo;
                    request.Key = $"{caminho}/{Path.GetFileName(arquivo)}";

                    await utility.UploadAsync(request);

                    return request.Key;
                }
            }
        }
        catch (AmazonS3Exception ex)
        {
            throw new Exception($"Erro ao enviar o arquivo PDF para o Amazon S3: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro geral ao enviar o arquivo PDF para o Amazon S3: {ex.Message}");
        }
    }

    public async Task<string> EnviarPNGAsync(string arquivo, string caminho)
    {
        try
        {
            using (AmazonS3Client client = GetClient())
            {
                using (TransferUtility utility = new TransferUtility(client))
                {
                    TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

                    request.BucketName = _secrets.AWS.S3.BucketName;
                    request.FilePath = arquivo;
                    request.Key = $"{caminho}/{Path.GetFileName(arquivo)}";

                    await utility.UploadAsync(request);

                    return request.Key;
                }
            }
        }
        catch (AmazonS3Exception ex)
        {
            throw new Exception($"Erro ao enviar o arquivo PDF para o Amazon S3: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro geral ao enviar o arquivo PDF para o Amazon S3: {ex.Message}");
        }
    }

    protected AmazonS3Client GetClient()
    {
        string accessKey = _secrets.AWS.AccessKey;
        string secretKey = _secrets.AWS.SecretKey;

        AmazonS3Config s3Config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USWest2
        };
        return new AmazonS3Client(accessKey, secretKey, s3Config);
    }
}