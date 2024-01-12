using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Netsoftware.Xanthos.Common.AuthorizationHeaderProviderMiddleware;

public static class AuthorizationHeaderProvider
{
    private static KeyValuePair<string, StringValues> _header;

    public static void SetHeader(KeyValuePair<string, StringValues> authorizationHeader) => _header = authorizationHeader;

    public static KeyValuePair<string, StringValues> GetAuthorizationHeader() => _header;
}