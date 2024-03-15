using System.Security.Cryptography;
using System.Text;

public static class Aes
{
    public static string Encrypt(string text)
    {
        using System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create();
        using SHA256 sha256 = SHA256.Create();

        aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
        aes.IV = new byte[16];

        using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using MemoryStream ms = new MemoryStream();
        using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (StreamWriter sw = new StreamWriter(cs))
        {
            sw.Write(text);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string encryptedText, string text)
    {
        using System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create();
        using SHA256 sha256 = SHA256.Create();

        aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
        aes.IV = new byte[16];

        using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedText));
        using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using StreamReader sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}