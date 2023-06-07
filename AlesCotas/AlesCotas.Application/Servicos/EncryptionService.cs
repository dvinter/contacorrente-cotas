using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlesCotas.Application.Servicos
{
    internal class EncryptionService
    {
        public string Token { get; set; }
        public string EncryptionKey { get; set; }

        public EncryptionService()
        {
        }

        public EncryptionService(string token, string encryptionKey)
        {
            Token = token;
            EncryptionKey = encryptionKey;
        }


        public string EncryptToken(string token, string encryptionKey)
        {
            var key = Encoding.UTF8.GetBytes(encryptionKey);

            using(var aesAlg = Aes.Create())
            {
                using(var encryptor =  aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using(var msEncrypt = new MemoryStream())
                    {
                        using(var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using(var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(token);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public string DecryptToken(string encryptedToken, string  encryptionKey)
        {
            var fullCipher = Convert.FromBase64String(encryptedToken);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0 , iv.Length);
            var key = Encoding.UTF8.GetBytes(encryptionKey);

            using(var aesAlg = Aes.Create())
            {
                using(var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using(var msDecrypt = new MemoryStream(cipher))
                    {
                        using(var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}
