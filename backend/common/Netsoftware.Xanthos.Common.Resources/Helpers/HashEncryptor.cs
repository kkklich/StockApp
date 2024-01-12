using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Netsoftware.Xanthos.Common.Resources.Helpers;

public static class HashEncryptor
{
    public static string EncryptId(int id)
    {
        return Encrypt(id.ToString(), null);
    }

    public static int DecryptId(string encryptedId)
    {
        var decryptedIdAsString = Decrypt(encryptedId, null);

        if (!int.TryParse(decryptedIdAsString, out var result))
            throw new InvalidOperationException($"Cannot decrypt integer id: {encryptedId}");

        return result;
    }

    public static string Encrypt(string plainText, string password)
    {
        if (plainText == null) return null;

        password ??= string.Empty;

        // Get the bytes of the string
        var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);

        var bytesEncrypted = Encrypt(bytesToBeEncrypted, password);

        return Convert.ToBase64String(bytesEncrypted);
    }

    public static string Decrypt(string encryptedText, string password)
    {
        if (encryptedText == null) return null;

        password ??= string.Empty;

        // Get the bytes of the string
        var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);

        var bytesDecrypted = Decrypt(bytesToBeDecrypted, password);

        return Encoding.UTF8.GetString(bytesDecrypted);
    }

    private static byte[] Encrypt(byte[] bytesToBeEncrypted, string password)
    {
        byte[] encryptedBytes = null;

        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.


        using var ms = new MemoryStream();
        using var AES = Aes.Create("AesManaged");
        var key = new Rfc2898DeriveBytes(password, 16, 1000);

        AES.KeySize = 256;
        AES.BlockSize = 128;
        AES.Key = key.GetBytes(AES.KeySize / 8);
        AES.IV = key.GetBytes(AES.BlockSize / 8);

        AES.Mode = CipherMode.CBC;

        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
            cs.Close();
        }

        encryptedBytes = ms.ToArray();

        return encryptedBytes;
    }

    private static byte[] Decrypt(byte[] bytesToBeDecrypted, string password)
    {
        byte[] decryptedBytes = null;

        // Set your salt here, change it to meet your flavor:
        // The salt bytes must be at least 8 bytes.


        using var ms = new MemoryStream();
        using var AES = Aes.Create("AesManaged");
        var key = new Rfc2898DeriveBytes(password, 16, 1000);

        AES.KeySize = 256;
        AES.BlockSize = 128;
        AES.Key = key.GetBytes(AES.KeySize / 8);
        AES.IV = key.GetBytes(AES.BlockSize / 8);
        AES.Mode = CipherMode.CBC;

        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
        {
            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
            cs.Close();
        }

        decryptedBytes = ms.ToArray();

        return decryptedBytes;
    }
}