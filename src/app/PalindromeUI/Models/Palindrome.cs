using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PalindromeUI.Models
{
    public class Palindrome
    {
        [Required]
        public string Value { get; set; }
    }
}