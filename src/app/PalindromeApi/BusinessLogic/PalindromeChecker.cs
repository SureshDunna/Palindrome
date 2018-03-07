using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PalindromeApi.BusinessLogic
{
    public interface IPalindromeChecker
    {
        Task<bool> IsPalindrome(string inputString);
    }
    public class PalindromeChecker : IPalindromeChecker
    {
        public Task<bool> IsPalindrome(string inputString)
        {
            //Empty string by default palindrome
            if(string.IsNullOrWhiteSpace(inputString))
            {
                return Task.FromResult(true);
            }

            var forwardIndex = 0;
            var reverseIndex = inputString.Length - 1;

            while(forwardIndex <= reverseIndex)
            {
                //ignoring all the white space and  punctuation characters
                while(char.IsWhiteSpace(inputString[forwardIndex]) || char.IsPunctuation(inputString[forwardIndex]))
                {
                    //by ignoring the punctuation characters if forward index crosses reverse index 
                    //then its palindrome
                    if(++forwardIndex > reverseIndex)
                    {
                        return Task.FromResult(true);
                    }
                    
                    continue;
                }
                
                //ignoring all the white space and punctuation characters
                while(char.IsWhiteSpace(inputString[reverseIndex]) || char.IsPunctuation(inputString[reverseIndex]))
                {
                    //by ignoring the punctuation characters if reverse index crosses forward index in reverse order
                    //then its palindrome
                    if(forwardIndex > --reverseIndex)
                    {
                        return Task.FromResult(true);
                    }

                    continue;                
                }

                //if both the indexes are pointing to the same location then its palindrome
                //this can be incases of single charater or any string with base length after ignoring
                //punctuation characters
                if(forwardIndex == reverseIndex)
                {
                    return Task.FromResult(true);
                }

                var asciiDifference = inputString[forwardIndex] - inputString[reverseIndex];

                //asciiDifference = 0 means its same character with same case
                //asciiDifference = 32 0r -32 means its same character with different case
                if(asciiDifference != 0 && asciiDifference != 32 && asciiDifference != -32)
                {
                    return Task.FromResult(false);
                }

                forwardIndex++;
                reverseIndex--;
            }

            //if we reached here then that means its palindrome
            return Task.FromResult(true);
        }
    }
}