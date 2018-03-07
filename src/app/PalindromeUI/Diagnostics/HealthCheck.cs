using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PalindromeUI.BusinessLogic;

namespace PalindromeUI.Diagnostics
{
    public class HealthCheck : HealthCheckBase
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public HealthCheck(ILogger<HealthCheck> logger, 
        IHttpClientFactory httpClientFactory) : base(logger)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected override void ConfigureHealthChecks()
        {
            Register("Palindrome Api Health Check", 
            async () =>
            {
                try
                {
                    var palindromeApiClient = _httpClientFactory.CreateClient("http://localhost:5002/");

                    var httpResponseMessage = await palindromeApiClient.GetAsync("healthcheck/ping");

                    if(!httpResponseMessage.IsSuccessStatusCode)
                    {
                        return false;
                    }

                    var healthCheckResult = JsonConvert.DeserializeObject<HealthCheckResult>(await httpResponseMessage.Content.ReadAsStringAsync());

                    _logger.LogError($"healthcheck result is {healthCheckResult.Healthy}");
                    return healthCheckResult.Healthy;
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Error occured while doing the healthcheck {ex}");
                    return false;
                }
            });
        }
    }
}