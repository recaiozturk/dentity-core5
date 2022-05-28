using Core5Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Core5Identity.Infrastructers
{
    public class CustomUserValidater : IUserValidator<ApplicationUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            //email sadece gmail ya da hotmail olsun
            if (user.Email.ToLower().EndsWith("@gmail.com")|| user.Email.ToLower().EndsWith("@hotmail.com"))
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError()
                {
                    Code="EmailDomainError",
                    Description="Sadece gmail ve hotmain e izin veriliyor"
                }));
            }

        }
    }
}
