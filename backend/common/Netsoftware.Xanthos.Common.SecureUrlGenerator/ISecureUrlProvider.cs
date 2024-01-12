using Netsoftware.Xanthos.Common.SecureUrlGenerator;

namespace Netsoftware.Xanthos.Common.UrlTokenGenerator;

public interface ISecureUrlProvider
{
    string GetSecureUrl(string url, string stringToEncode, string purpose = "purpose");
    string GetSecureUrl(string url, EncodablePayload objectToEncode, string purpose = "purpose");
}