using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PalindromeUI.BusinessLogic;
using PalindromeUI.Config;
using PalindromeUI.Models;

namespace ContosoUniversity.Controllers
{
    public class PalindromeController : Controller
    {
        private readonly PalindromeApiConfig _config;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ILogger _logger;

        public PalindromeController(IOptions<PalindromeApiConfig> config, 
        IHttpClientFactory httpClientFactory, ILogger<PalindromeController> logger)
        {
            _config = config.Value;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // GET: Palindromes
        public async Task<IActionResult> Index()
        {
            var palindromeApiClient = _httpClientFactory.CreateClient(_config.BaseUrl);

            var httpResponseMessage = await palindromeApiClient.GetAsync("palindromes");

            if(httpResponseMessage.IsSuccessStatusCode)
            {
                var palindromes = JsonConvert.DeserializeObject<IEnumerable<PalindromeUI.Models.Palindrome>>(await httpResponseMessage.Content.ReadAsStringAsync());
                return View(palindromes);
            }

            TempData[MyAlerts.FAIL] = $"Error occured during palindrome check and status code is {httpResponseMessage.StatusCode}";
            
            return RedirectToAction("Index");
        }

        public IActionResult Check()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Check([Bind("Value")] PalindromeUI.Models.Palindrome palindrome)
        {
            if (ModelState.IsValid)
            {
                var palindromeApiClient = _httpClientFactory.CreateClient(_config.BaseUrl);
            
                var httpResponseMessage = await palindromeApiClient.PostAsync("ispalindrome",             
                new StringContent(JsonConvert.SerializeObject(palindrome), Encoding.UTF8, "application/json"));
                
                if(httpResponseMessage.IsSuccessStatusCode)
                {
                    var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                    _logger.LogInformation(responseString);
                    var isPalindrome = JsonConvert.DeserializeObject<bool>(responseString);

                    if(isPalindrome)
                    {
                        TempData[MyAlerts.SUCCESS] = $"Input string {palindrome.Value} is palindrome";
                    }
                    else
                    {
                        TempData[MyAlerts.FAIL] = $"Input string {palindrome.Value} is not palindrome";
                    }
                }
                else
                {
                    TempData[MyAlerts.FAIL] = $"Error occured during palindrome check and status code is {httpResponseMessage.StatusCode}";
                }

                return RedirectToAction("Check");
            }
            
            return View(palindrome);
        }
    }
}
