using System;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.ViewModels
{
    public class LoginViewModel
	{
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

