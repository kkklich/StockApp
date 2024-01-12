using System.Collections.Generic;

namespace Netsoftware.Xanthos.Common.HttpClient.UrlProvider;

public interface IUrlProvider
{
    string GetUrl(string name);
    public List<KeyValuePair<string, string>> List();
}