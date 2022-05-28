using Core5Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core5Identity.Infrastructers
{
    public class CustomPasswordValidater : IPasswordValidator<ApplicationUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            //Kullanıcıya gösterilecek hatalar
            List<IdentityError> errors = new List<IdentityError>();

            //password un içinde usernaame var mı
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError()
                {
                    Code = "PasswordContainsUserName",
                    Description = "Password cannot contain User Name"
                });
            }

            //password ardışık değer içeriyor mu 
            if (password.Contains("123"))
            {
                errors.Add(new IdentityError()
                {
                    Code="PasswordContainsNumericSequence",
                    Description="Passwords cannot contain numeric sequence"
                });
            }

            //hata yoksa sorun yok varsa hata listesini gönderiyoruz
            return Task.FromResult(errors.Count==0 ? IdentityResult.Success:IdentityResult.Failed(errors.ToArray()));
        }
    }
}
