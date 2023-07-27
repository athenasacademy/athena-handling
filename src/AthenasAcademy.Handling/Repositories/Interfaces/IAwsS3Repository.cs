namespace AthenasAcademy.Handling.Repositories.Interfaces;

public interface IAwsS3Repository
{
    Task<string> EnviarPDFAsync(string arquivo, string caminho);

    Task<string> EnviarPNGAsync(string arquivo, string caminho);
}