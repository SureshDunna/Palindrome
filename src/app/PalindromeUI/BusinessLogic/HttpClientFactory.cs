using System;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;

namespace PalindromeUI.BusinessLogic
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient(string baseUrl);
    }
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly IMemoryCache _cache; 

        public HttpClientFactory(IMemoryCache cache)
        {
            _cache = cache;
        }

        public HttpClient CreateClient(string baseUrl)
        {
            return _cache.GetOrCreate<HttpClient>(baseUrl, (entry) =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2);

                return new HttpClient { BaseAddress = new Uri(baseUrl) };
            });
        }
    }
}