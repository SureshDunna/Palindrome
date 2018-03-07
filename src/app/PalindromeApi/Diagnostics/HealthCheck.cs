using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PalindromeApi.Models;

namespace PalindromeApi.Diagnostics
{
    public class HealthCheck : HealthCheckBase
    {
        private readonly ILogger _logger;
        private readonly PalindromeContext _context;

        public HealthCheck(ILogger<HealthCheck> logger, PalindromeContext context) : base(logger)
        {
            _logger = logger;
            _context = context;
        }

        protected override void ConfigureHealthChecks()
        {
            Register("DB Health Check", 
            () =>
            {
                try
                {
                    return Task.FromResult(_context.Palindromes.Any());
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Error occured while doing the healthcheck {ex}");
                    return Task.FromResult(false);
                }
            });
        }
    }
}