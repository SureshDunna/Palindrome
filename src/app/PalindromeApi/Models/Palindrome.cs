using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PalindromeApi.Models
{
    public class PalindromeContext : DbContext
    {
        public PalindromeContext() {}
        public PalindromeContext(DbContextOptions<PalindromeContext> options) : base(options){}
        public DbSet<Palindrome> Palindromes { get; set; }
    }
    public class Palindrome
    {
        /// <summary>
        /// value contains string to be verified if it is a palindrome or not
        /// </summary>
        /// <returns></returns>
        [Required]
        [Key]    
        public string Value { get; set; }
    }
}