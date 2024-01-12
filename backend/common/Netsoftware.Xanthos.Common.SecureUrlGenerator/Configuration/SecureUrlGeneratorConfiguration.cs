namespace Netsoftware.Xanthos.Common.UrlTokenGenerator.Configuration;

public class SecureUrlGeneratorConfiguration
{
    public string PersistKeysToFileSystem { get; set; }
    public string ProtectorApplicationName { get; set; }
    public string KeyCertificateFileName { get; set; }
    public string KeyCertificatePassword { get; set; }
}