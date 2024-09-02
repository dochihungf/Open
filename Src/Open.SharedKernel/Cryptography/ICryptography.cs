namespace Open.SharedKernel.Cryptography;

public interface ICryptography
{
    string Encrypt(string plainText);

    string Decrypt(string cypher);

    string EncryptLarge(string dataToEncrypt);

    string DecryptLarge(string cypher);
}
