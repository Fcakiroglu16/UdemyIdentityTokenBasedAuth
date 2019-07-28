using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentityTokenBasedAuth.Enum;

namespace UdemyIdentityTokenBasedAuth.ResourceViewModel
{
    public class UserViewModelResource
    {
        [Required(ErrorMessage = "Kullanıcı ismi gerekldir.")]
    
        public string UserName { get; set; }

      
        [RegularExpression(@"^(0(\d{3}) (\d{3}) (\d{2}) (\d{2}))$", ErrorMessage = "Telefon numarası uygun formatta değil")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email adresi gereklidir.")]
     
        [EmailAddress(ErrorMessage = "Email adresiniz doğru formatta değil")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifreniz gereklidir.")]
       
     
        public string Password { get; set; }

      
        public DateTime? BirthDay { get; set; }

        public string Picture { get; set; }

   
        public string City { get; set; }

       
        public Gender Gender { get; set; }


    }
}
