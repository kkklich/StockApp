using Netsoftware.Xanthos.Common.SecureUrlGenerator;

namespace Netsoftware.Xanthos.Common.UrlTokenGenerator;

public interface ISecureUrlDecoder
{
    /// <summary>
    ///     Return object with specific type.
    /// </summary>
    /// <param name="payloadToDecode"></param>
    /// <param name="purpose"></param>
    /// <typeparam name="TResult">Class should inherit from EncodablePayload</typeparam>
    /// <returns></returns>
    TResult Decode<TResult>(string payloadToDecode, string purpose = "purpose") where TResult : EncodablePayload;

    /// <summary>
    ///     Return object as JSON
    /// </summary>
    /// <param name="urlToDecode"></param>
    /// <returns></returns>
    string Decode(string payloadToDecode, string purpose = "purpose");
}