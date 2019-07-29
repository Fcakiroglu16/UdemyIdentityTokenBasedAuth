using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyIdentityTokenBasedAuth.Models;

namespace UdemyIdentityTokenBasedAuth.Services
{
    public class BaseService:ControllerBase
    {
        protected  UserManager<AppUser> userManager { get; }
        protected SignInManager<AppUser> signInManager { get; }
        protected RoleManager<AppRole> roleManager { get; }

        public BaseService(UserManager<AppUser> userManager ,SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

        }


    }
}
