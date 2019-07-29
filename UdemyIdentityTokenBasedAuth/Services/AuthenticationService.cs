using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UdemyIdentityTokenBasedAuth.Domain.Responses;
using UdemyIdentityTokenBasedAuth.Domain.Services;
using UdemyIdentityTokenBasedAuth.Models;
using UdemyIdentityTokenBasedAuth.ResourceViewModel;
using UdemyIdentityTokenBasedAuth.Security.Token;

namespace UdemyIdentityTokenBasedAuth.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly ITokenHandler tokenHandler;
        private readonly CustomTokenOptions tokenOptions;
        private readonly IUserService userService;



        public AuthenticationService(IUserService userService,ITokenHandler tokenHandler,IOptions<CustomTokenOptions> tokenOptions, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager) : base(userManager, signInManager, roleManager)
        {
            this.tokenHandler = tokenHandler;
            this.userService = userService;
            this.tokenOptions = tokenOptions.Value;



        }

        public async Task<BaseResponse<AccessToken>> CreateAccessTokenByRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel)
        {

            var userClaim = await userService.GetUserByRefreshToken(refreshTokenViewModel.RefreshToken);

            if(userClaim.Item1!=null)
            {
                AccessToken accessToken = tokenHandler.CreateAccessToken(userClaim.Item1);

                Claim refreshTokenClaim = new Claim("refreshToken", accessToken.RefreshToken);
                Claim refreshTokenEndDateClaim = new Claim("refreshTokenEndDate", DateTime.Now.AddMinutes(tokenOptions.RefreshTokenExpiration).ToString());

                await userManager.ReplaceClaimAsync(userClaim.Item1, userClaim.Item2[0], refreshTokenClaim);
                await userManager.ReplaceClaimAsync(userClaim.Item1, userClaim.Item2[1], refreshTokenEndDateClaim);

                return new BaseResponse<AccessToken>(accessToken);

            }
            else
            {
                return new BaseResponse<AccessToken>("Böyle bir refreshtoken sahip kullanıcı yok");
            }





        }

        public async Task<BaseResponse<AccessToken>> RevokeRefreshToken(RefreshTokenViewModelResource refreshTokenViewModel)
        {
            bool result = await userService.RevokeRefrefreshToken(refreshTokenViewModel.RefreshToken);

            if(result)
            {

                return new BaseResponse<AccessToken>(new AccessToken());
            }
            else
            {
                return new BaseResponse<AccessToken>("refreshtoken veritabanında bulunamadı");
            }


        }

        public async Task<BaseResponse<AccessToken>> SignIn(SignInViewModelResource signInViewModel)
        {

            AppUser user = await userManager.FindByEmailAsync(signInViewModel.Email);

            if(user!=null)
            {
                bool isUser = await userManager.CheckPasswordAsync(user, signInViewModel.Password);


                if(isUser)
                {
                    AccessToken accessToken = tokenHandler.CreateAccessToken(user);


                    Claim refreshTokenClaim = new Claim("refreshToken", accessToken.RefreshToken);
                    Claim refreshTokenEndDateClaim = new Claim("refreshTokenEndDate", DateTime.Now.AddMinutes(tokenOptions.RefreshTokenExpiration).ToString());


                    List<Claim> refreshClaimList = userManager.GetClaimsAsync(user).Result.Where(c => c.Type.Contains("refreshToken")).ToList();


                    if(refreshClaimList.Any())
                    {
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[0], refreshTokenClaim);
                        await userManager.ReplaceClaimAsync(user, refreshClaimList[1], refreshTokenEndDateClaim);

                    }
                    else
                    {
                        await userManager.AddClaimsAsync(user, new[] { refreshTokenClaim, refreshTokenEndDateClaim });
                    }

                    return new BaseResponse<AccessToken>(accessToken);

                }


                return new BaseResponse<AccessToken>("şifre yanlış");

            }



            return new BaseResponse<AccessToken>("email yanlış");





        }

        public async Task<BaseResponse<UserViewModelResource>> SignUp(UserViewModelResource userViewModel)
        {

            AppUser user = new AppUser { UserName = userViewModel.UserName, Email = userViewModel.Email };


            IdentityResult result = await this.userManager.CreateAsync(user);


            if(result.Succeeded)
            {
                return new BaseResponse<UserViewModelResource>(user.Adapt<UserViewModelResource>());
            }
            else
            {
                return new BaseResponse<UserViewModelResource>(result.Errors.First().Description);
            }


        }
    }
}
