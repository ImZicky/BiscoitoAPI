using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Model.Entity;
using Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiscoitoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BiscoitoDbSeedController : Controller
    {

        private readonly UserManager<BiscoitoAPIUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _conf;

        public BiscoitoDbSeedController(UserManager<BiscoitoAPIUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration conf)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _conf = conf;
        }


        [HttpGet]
        public async Task SeedEssentials()
        {
            var roles = Enum.GetValues<Roles>();
         
            foreach(var role in roles)
            {
                //Seed Roles
                await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }

            //Seed Default User
            var defaultUser = new BiscoitoAPIUser { UserName = _conf["BiscoitoAdminSeedUser:Email"], Email = _conf["BiscoitoAdminSeedUser:Email"], EmailConfirmed = true, PhoneNumberConfirmed = true };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, _conf["BiscoitoAdminSeedUser:Password"]);
                await userManager.AddToRoleAsync(defaultUser, Roles.Administrator.ToString());
                await userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
            }
        }

        //[Authorize]
        //[Route("api/[controller]")]
        //[ApiController]


    }
}
