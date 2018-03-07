using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PalindromeApi.BusinessLogic;
using PalindromeApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PalindromeApi.Controllers
{
    [Route("api")]
    public class PalindromeController : Controller
    {
        private readonly IPalindromeChecker _palindromeChecker;
        private readonly PalindromeContext _palindromeContext;

        public PalindromeController(IPalindromeChecker palindromeChecker, PalindromeContext palindromeContext)
        {
            _palindromeChecker = palindromeChecker;
            _palindromeContext = palindromeContext;
        }

        /// <summary>
        /// It checks if the string provided is palindrome or not
        /// if it is then it will get preserved in data store
        /// </summary>
        /// <returns></returns>
        [HttpPost("ispalindrome")]
        [ProducesResponseType(typeof(bool), 200)]
        [SwaggerOperation(operationId: "isPalindrome")]
        public async Task<IActionResult> IsPalindrome([FromBody, Required] Palindrome palindrome)
        {
            if(_palindromeContext.Palindromes.Any(x => string.Equals(x.Value, palindrome.Value, 
            StringComparison.OrdinalIgnoreCase)))
            {
                return Ok(true);
            }
            
            var palindromeCheckResult = await _palindromeChecker.IsPalindrome(palindrome.Value);

            if(palindromeCheckResult)
            {
                _palindromeContext.Palindromes.Add(new Palindrome { Value = palindrome.Value });
                await _palindromeContext.SaveChangesAsync();
            }

            return Ok(palindromeCheckResult);
        }

        /// <summary>
        /// Retrieves all the palindromes from repository
        /// </summary>
        /// <returns></returns>
        [HttpGet("palindromes")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore  = true)]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [SwaggerOperation(operationId: "getPalindromes")]
        public IActionResult GetPalindromes()
        {
            return Ok(_palindromeContext.Palindromes.Select(x => x));
        }
    }
}