using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentityTokenBasedAuth.Models;

namespace UdemyIdentityTokenBasedAuth.Security.Token
{
    interface ITokenHandler
    {
        AccessToken CreateAccessToken(AppUser user);

        void RevokeRefreshToken(AppUser user);
    }
}
