using Microsoft.AspNetCore.Authentication.Twitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentityTokenBasedAuth.Domain.Responses;
using UdemyIdentityTokenBasedAuth.ResourceViewModel;

namespace UdemyIdentityTokenBasedAuth.Domain.Services
{
    interface IAuthenticationService
    {

        Task<BaseResponse<UserViewModelResource>> SignUp(UserViewModelResource userViewModel);

        Task<BaseResponse<AccessToken>> SignIn(SignInViewModelResource signInViewModel);

        Task<BaseResponse<AccessToken>> CreateAccessTokenByRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel);

        Task<BaseResponse<AccessToken>> RevokeRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel);




    }
}
