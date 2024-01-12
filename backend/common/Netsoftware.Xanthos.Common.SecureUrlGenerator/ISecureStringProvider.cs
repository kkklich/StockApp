using Netsoftware.Xanthos.Common.SecureUrlGenerator;

namespace Netsoftware.Xanthos.Common.UrlTokenGenerator;

public interface ISecureStringProvider
{
    void CreateProtector(string purpose);
    string Encode(string data);
    string Encode(EncodablePayload data);
    string Decode(string data);
    TOutput Decode<TOutput>(string data);
    bool ProtectorIsReady();
}