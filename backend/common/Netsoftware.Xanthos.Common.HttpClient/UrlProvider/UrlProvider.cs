using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Netsoftware.Xanthos.Common.HttpClient.UrlProvider;

public class UrlProvider : IUrlProvider
{
    private readonly Dictionary<string, string> Urls;

    public UrlProvider(IConfiguration configuration)
    {
        Urls = new Dictionary<string, string>();

        var urlsSection = configuration.GetSection("ApplicationsUrls");
        var children = urlsSection.GetChildren();

        foreach (var subSection in children) AddUrl(subSection.Key, subSection.Value);
    }

    public string GetUrl(string name)
    {
        try
        {
            return Urls[name.ToUpperInvariant()];
        }
        catch (Exception)
        {
            return null;
        }
    }

    public List<KeyValuePair<string, string>> List()
    {
        return Urls.ToList();
    }

    internal void AddUrl(string name, string url)
    {
        Urls.Add(name.ToUpperInvariant(), url);
    }
}