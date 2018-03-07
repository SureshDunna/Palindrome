using System.Linq;
using PalindromeApi.Models;

namespace PalindromeApi.DataAccess
{
    public class DbInitializer
    {
        public static void Initialize(PalindromeContext context)
        {
            context.Database.EnsureCreated();

            if(context.Palindromes.Any())
            {
                return;
            }

            context.Palindromes.Add(new Models.Palindrome { Value = "abba"});

            context.SaveChanges();
        }
    }
}