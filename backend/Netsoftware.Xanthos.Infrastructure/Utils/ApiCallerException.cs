using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Netsoftware.Xanthos.Infrastructure.Utils;

[Serializable]
public class ApiCallerException : Exception
{
    public ApiCallerException(string message, HttpResponseMessage response, string url) : base(message)
    {
        ApiResponse = response;
        RequestUrl = url;
    }

    protected ApiCallerException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public HttpResponseMessage ApiResponse { get; set; }
    public string RequestUrl { get; set; }
}