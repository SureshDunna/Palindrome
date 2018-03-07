using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using PalindromeApi.BusinessLogic;
using Xunit;

namespace Palindrome.UnitTests.BusinessLogic
{
    public class PalindromeCheckerTests
    {
        private readonly IPalindromeChecker _palindromeChecker;

        public PalindromeCheckerTests()
        {
            _palindromeChecker = new PalindromeChecker();
        }

        [Theory]
        [InlineData("Was it a cat I saw?")]
        [InlineData("Don't nod.")]
        [InlineData("Radar")]
        [InlineData("No lemon, no melon")]
        public async Task must_return_true_for_all_palindrome_strings(string inputString)
        {
            Assert.True(await _palindromeChecker.IsPalindrome(inputString));
        }

        [Fact]
        public async Task must_return_false_if_not_palindrome()
        {
            Assert.False(await _palindromeChecker.IsPalindrome("abcd"));
        }
    }
}