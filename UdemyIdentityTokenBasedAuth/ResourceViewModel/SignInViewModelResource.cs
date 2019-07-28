using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyIdentityTokenBasedAuth.ResourceViewModel
{
    public class SignInViewModelResource
    {
      
        [Required(ErrorMessage = "Email alanı gereklidir")]
        [EmailAddress]
        public string Email { get; set; }

       
        [Required(ErrorMessage = "Şifre alanı gereklidir")]
    
        [MinLength(4, ErrorMessage = "şifreniz en az 4 karakterli olmalıdır.")]
        public string Password { get; set; }

        
    }
}
