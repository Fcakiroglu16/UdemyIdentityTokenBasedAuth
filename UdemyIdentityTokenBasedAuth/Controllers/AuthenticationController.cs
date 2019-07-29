using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UdemyIdentityTokenBasedAuth.Domain.Responses;
using UdemyIdentityTokenBasedAuth.Domain.Services;
using UdemyIdentityTokenBasedAuth.ResourceViewModel;

namespace UdemyIdentityTokenBasedAuth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

            public AuthenticationController(IAuthenticationService service)
        {
            this.authenticationService = service;
        }

        //localhost:33444/api/ Authentication/IsAuthenticatin
        [HttpGet]
        public ActionResult IsAuthenticatin()
        {

            return Ok(User.Identity.IsAuthenticated);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModelResource userViewModelResource)
        {

          BaseResponse<UserViewModelResource> response=    await this.authenticationService.SignUp(userViewModelResource);


            if(response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }


        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModelResource signInViewModel)
        {

            var response = await authenticationService.SignIn(signInViewModel);

           if(response.Success)
            {
                return Ok(response.Extra);
            }
           else
            {
                return BadRequest(response.Message);
            }


        }


        [HttpPost]
        public async Task<IActionResult> CreateAccessTokenByRefreshToken(RefreshTokenViewModelResource refreshTokenView)
        {


            var response = await authenticationService.CreateAccessTokenByRefreshToken(refreshTokenView);


            if(response.Success)
            {
                return Ok(response.Extra);
            }
            else
            {
                return BadRequest(response.Message);
            }

        }

        [HttpDelete]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenViewModelResource refreshTokenView)
        {
            var response = await authenticationService.RevokeRefreshToken(refreshTokenView);
            if(response.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest(response.Message);
            }

        }


    }
}