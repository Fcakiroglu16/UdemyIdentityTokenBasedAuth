using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UdemyIdentityTokenBasedAuth.Domain.Responses;
using UdemyIdentityTokenBasedAuth.Models;
using UdemyIdentityTokenBasedAuth.ResourceViewModel;

namespace UdemyIdentityTokenBasedAuth.Domain.Services
{
  public   interface IUserService
    {

        Task<BaseResponse<UserViewModelResource>> UpdateUser(UserViewModelResource userViewModel, string userName);


        Task<AppUser> GetUserByUserName(string userName);

        Task<BaseResponse<AppUser>> UploadUserPicture(string picturePath, string userName);

        Task<Tuple<AppUser, IList<Claim>>> GetUserByRefreshToken(string refreshToken);

        Task<bool> RevokeRefrefreshToken(string refreshToken);




    }
}
