namespace Identity.Infrastructure;

public interface IGeminiKeyEncryptor
{
    string Encrypt(string plainTextApiKey);
    string Decrypt(string encryptedApiKey);
}
