using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace Open.SharedKernel.Cryptography;

public class Cryptography : ICryptography
    {
        private static readonly int keySize = 2048;
        private RSACryptoServiceProvider csp = new RSACryptoServiceProvider(keySize);
        private RSAParameters privateKey;
        private RSAParameters publicKey;

        public Cryptography()
        {
            privateKey = csp.ExportParameters(true);
            publicKey = csp.ExportParameters(false);
        }

        public string GetPublicKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, publicKey);

            return sw.ToString();
        }

        public string Encrypt(string plainText)
        {
            csp = new RSACryptoServiceProvider(keySize);
            csp.ImportParameters(publicKey);

            var cypher = csp.Encrypt(Encoding.UTF8.GetBytes(plainText), false);
            return Convert.ToBase64String(cypher);
        }

        public string Decrypt(string cypher)
        {
            csp.ImportParameters(privateKey);

            var bytes = Convert.FromBase64String(cypher);
            var plainText = csp.Decrypt(bytes, false);

            return Encoding.UTF8.GetString(plainText);
        }

        public string EncryptLarge(string dataToEncrypt)
        {
            // Our bytearray to hold all of our data after the encryption
            byte[] encryptedBytes = new byte[0];
            var csp = new RSACryptoServiceProvider(keySize);

            //Create a new instance of RSACryptoServiceProvider.
            var encoder = new UTF8Encoding();
            var encryptThis = encoder.GetBytes(dataToEncrypt);

            // Importing the public key
            csp.ImportParameters(publicKey);

            var blockSize = (csp.KeySize / 8) - 32;

            // buffer to write byte sequence of the given block_size
            var buffer = new byte[blockSize];
            var encryptedBuffer = new byte[blockSize];

            // Initializing our encryptedBytes array to a suitable size, depending on the size of data to be encrypted
            encryptedBytes = new byte[encryptThis.Length + blockSize - (encryptThis.Length % blockSize) + 32];

            for (int i = 0; i < encryptThis.Length; i += blockSize)
            {
                // If there is extra info to be parsed, but not enough to fill out a complete bytearray, fit array for last bit of data
                if (2 * i > encryptThis.Length && ((encryptThis.Length - i) % blockSize != 0))
                {
                    buffer = new byte[encryptThis.Length - i];
                    blockSize = encryptThis.Length - i;
                }

                // If the amount of bytes we need to decrypt isn't enough to fill out a block, only decrypt part of it
                if (encryptThis.Length < blockSize)
                {
                    buffer = new byte[encryptThis.Length];
                    blockSize = encryptThis.Length;
                }

                // encrypt the specified size of data, then add to final array.
                Buffer.BlockCopy(encryptThis, i, buffer, 0, blockSize);
                encryptedBuffer = csp.Encrypt(buffer, false);
                encryptedBuffer.CopyTo(encryptedBytes, i);
            }

            // Clear the RSA key container, deleting generated keys.
            csp.PersistKeyInCsp = false;

            // Convert the byteArray using Base64 and returns as an encrypted string
            return Convert.ToBase64String(encryptedBytes);
        }

        public string DecryptLarge(string cypher)
        {
            // The bytearray to hold all of our data after decryption
            byte[] decryptedBytes;

            //Create a new instance of RSACryptoServiceProvider.
            var RSA = new RSACryptoServiceProvider(keySize);
            var bytesToDecrypt = Convert.FromBase64String(cypher);

            // Import the private key info
            RSA.ImportParameters(privateKey);

            // No need to subtract padding size when decrypting (OR do I?)
            int blockSize = RSA.KeySize / 8;

            // buffer to write byte sequence of the given block_size
            var buffer = new byte[blockSize];

            // buffer containing decrypted information
            var decryptedBuffer = new byte[blockSize];

            // Initializes our array to make sure it can hold at least the amount needed to decrypt.
            decryptedBytes = new byte[cypher.Length];

            for (int i = 0; i < bytesToDecrypt.Length; i += blockSize)
            {
                if (2 * i > bytesToDecrypt.Length && ((bytesToDecrypt.Length - i) % blockSize != 0))
                {
                    buffer = new byte[bytesToDecrypt.Length - i];
                    blockSize = bytesToDecrypt.Length - i;
                }

                // If the amount of bytes we need to decrypt isn't enough to fill out a block, only decrypt part of it
                if (bytesToDecrypt.Length < blockSize)
                {
                    buffer = new byte[bytesToDecrypt.Length];
                    blockSize = bytesToDecrypt.Length;
                }

                Buffer.BlockCopy(bytesToDecrypt, i, buffer, 0, blockSize);
                decryptedBuffer = RSA.Decrypt(buffer, false);
                decryptedBuffer.CopyTo(decryptedBytes, i);
            }
            // We encode each byte with UTF8 and then write to a string while trimming off the extra empty data created by the overhead.
            var encoder = new UTF8Encoding();
            return encoder.GetString(decryptedBytes).TrimEnd(new[] { '\0' });

        }
    }
