using ShopApplication.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Interfaces.Services
{
    public interface IJWTService
    {
        public string GenerateAccessToken(UserLoginDTO userLoginDto, string role);
        public (string, int) GenerateRefreshToken();
    }
}
