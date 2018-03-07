using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace Palindrome.AcceptanceTests
{
    public class PalindromeControllerTests : TestBase
    {
        [Fact]
        public async Task can_get_all_palindromes()
        {
            using(var client = _server.CreateClient())
            {
                var response = await client.GetAsync("http://localhost/api/palindromes");

                Assert.Equal(response.StatusCode, HttpStatusCode.OK);

                var palindromes = JsonConvert.DeserializeObject<IEnumerable<PalindromeApi.Models.Palindrome>>(await response.Content.ReadAsStringAsync());

                Assert.NotNull(palindromes);

                var palindromesList = palindromes.ToList();

                Assert.True(palindromesList.Count >= 2);
                Assert.True(palindromesList.Any(x => x.Value == "abba"));
            }
        }

        [Theory]
        [InlineData("Was it a cat I saw?", true)]
        [InlineData("Don't nod.", true)]
        [InlineData("Radar", true)]
        [InlineData("No lemon, no melon", true)]
        [InlineData("abcde", false)]
        public async Task can_check_palindrome(string inputString, bool expectedResult)
        {
            using(var client = _server.CreateClient())
            {
                var response = await client.PostAsync("http://localhost/api/ispalindrome",
                new StringContent(JsonConvert.SerializeObject(new PalindromeApi.Models.Palindrome { Value = inputString }), Encoding.UTF8, "application/json"));

                Assert.Equal(response.StatusCode, HttpStatusCode.OK);

                var palindromeCheckResult = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

                Assert.Equal(expectedResult, palindromeCheckResult);

                if(expectedResult)
                {
                    var getPalindromeResult = await client.GetAsync("http://localhost/api/palindromes");

                    Assert.Equal(response.StatusCode, HttpStatusCode.OK);

                    var palindromes = JsonConvert.DeserializeObject<IEnumerable<PalindromeApi.Models.Palindrome>>(await getPalindromeResult.Content.ReadAsStringAsync());

                    Assert.NotNull(palindromes);

                    var palindromesList = palindromes.ToList();

                    Assert.True(palindromesList.Any(x => x.Value == inputString));
                }
            }
        }
    }
}