using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.DTO;
using Data.ETF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.Services
{
    public class UserServices : IUserServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;

        public UserServices(IHttpContextAccessor httpContextAccessor, DataContext dataContext, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IJwtGenerator jwtGenerator)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }
        public async Task<UserDisplayInfoDTO> Register(UserRegDTO user)
        {
            //throw new NotImplementedException();
            var email = await _dataContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (email != null)
            {
                throw new Exception("Email already exists");
            }

            var username = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == user.Username);

            if (username != null)
            {
                throw new Exception("Username already exists");
            }

            var newUser = new UserModel
            {
                UserName = user.Username,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Role = user.Role
                

            };

         
            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                return new UserDisplayInfoDTO
                {
                    DisplayName = newUser.DisplayName,
                    Username = newUser.UserName,
                    Token = _jwtGenerator.CreateToken(newUser)
                };
            }

            else
            {
                throw new Exception("Problem creating user");
            }


        }

        public async Task<UserDisplayInfoDTO> Login(UserLoginDTO newUser)
        {
            var user = await _userManager.FindByEmailAsync(newUser.Email);
            if (user == null)
            {
                throw new ArgumentException("incorrect details");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, newUser.Password, false);

            if (result.Succeeded)
            {
                return new UserDisplayInfoDTO
                {
                    DisplayName = user.DisplayName,
                    Username = user.UserName,
                    Token = _jwtGenerator.CreateToken(user)
                };
            }
            else
            {
                throw new ArgumentException("incorrect details");
            }
        }

        public async Task<DisplayPasswordDTO> ChangePassword(PasswordDTO details)
        {
            if (details == null)
            {
                throw new ArgumentException("incorrect details");
            }
            var username = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;


            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new ArgumentException("incorrect details");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, details.OldPassword, false);

            if (result.Succeeded)
            {
              var response = await  _userManager.ChangePasswordAsync(user, details.OldPassword, details.NewPassword);

              if (response.Succeeded)
              {
                  var newDetails = new DisplayPasswordDTO
                  {
                      Email =  user.Email,
                      NewPassword = details.NewPassword
                  };

                  return newDetails;
              }

              else
              {
                  throw new Exception("Operation not successful");
              }
            }
            else
            {
                throw new Exception("Operation not successful");
            }



        }

        public async Task<string> ResetPassword(DisplayPasswordDTO details)
        {
            var user = await _userManager.FindByEmailAsync(details.Email);

            if (user == null)
            {
                throw new Exception("Invalid Email");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

          var response = await _userManager.ResetPasswordAsync(user, token, details.NewPassword);

          if (response.Succeeded)
          {
              return details.NewPassword;
          }
          else
          {
              throw new Exception("Operation not successful");
            }

        }

        public async Task<UserDisplayInfoDTO> CurrentUser()
        {
            var username = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userManager.FindByNameAsync(username);

            return new UserDisplayInfoDTO
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Token = _jwtGenerator.CreateToken(user)
            };

        }

        public async Task<PhoneStore> AddPhone(PhoneDTO phone)
        {
            var newPhone = new PhoneStore
            {
                Name = phone.Name,
                Price = phone.Price,
                Model = phone.Model
            };

        await _dataContext.Phones.AddAsync(newPhone);
       await _dataContext.SaveChangesAsync();

        return newPhone;
        }
    }
}
