using MaxiShop.Application.Comman;
using MaxiShop.Application.InputModel;
using MaxiShop.Application.Services.Interface;
using MaxiShop.Application.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        private ApplicationUser ApplicationUser;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            ApplicationUser = new();
        }

        

        public async Task<IEnumerable<IdentityError>> Register(Register register)
        {
            ApplicationUser.UserName = register.FirstName;
            ApplicationUser.LastName = register.LastName;
            ApplicationUser.Email = register.Email;
            ApplicationUser.UserName = register.Email;

            var result = await _userManager.CreateAsync(ApplicationUser,register.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(ApplicationUser, "CUSTOMER");
            }


            return result.Errors;
        }


        public async Task<object> Login(Login login)
        {
            ApplicationUser = await _userManager.FindByEmailAsync(login.Email);

            if (ApplicationUser == null)
            {
                return "Invalid Email Address";
            }

            var result = await _signInManager.PasswordSignInAsync(ApplicationUser, login.Password, isPersistent: true, lockoutOnFailure: true);

            var isValidCredentials = await _userManager.CheckPasswordAsync(ApplicationUser, login.Password);

            if (result.Succeeded)
            {
                var token = await GenerateToken();

                LoginResponse loginResponse = new LoginResponse
                {
                    UserId = ApplicationUser.Id,
                    Token = token
                };

                return loginResponse;
            }
            else
            {
                if (result.IsLockedOut)
                {
                    return "Your Account is Locked,Contact System Admin";
                }

                if (result.IsNotAllowed)
                {
                    return "Invalid Password";
                }

                if (isValidCredentials == false)
                {
                    return "Invallid Password";
                }
                else
                {
                    return "Login Failed";
                }
            }

        }

        public async Task<string> GenerateToken()
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSetting:Key"]));

            var signingCredentials = new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(ApplicationUser);

            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,ApplicationUser.Email)
            }.Union(roleClaims).ToList();

            var token = new JwtSecurityToken
                (
                issuer: _config["JwtSetting:Issuer"],
                audience: _config["JwtSetting:Audience"],
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["JwtSetting:DurationInMinute"]))

                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
