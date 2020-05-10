﻿using System.Threading.Tasks;
using Data.DTO;
using Data.ETF;

namespace Data.Services
{
    public interface IUserServices
    {
        public Task<UserDisplayInfoDTO> Register(UserRegDTO user);
        public Task<UserDisplayInfoDTO> Login(UserLoginDTO newUser);
        public Task<DisplayPasswordDTO> ChangePassword(PasswordDTO details);
        public Task<string> ResetPassword(DisplayPasswordDTO details);
        public Task<UserDisplayInfoDTO> CurrentUser();
        public Task<PhoneStore> AddPhone(PhoneDTO phone);

    }
}