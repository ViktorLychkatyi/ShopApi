using ShopApplication.DTOs.UserDTOs;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(UserReadDTO? User, string? Token, string? RefreshToken)> RegisterAsync(UserCreateDTO dto);
        Task<(UserReadDTO? User, string? Token, string? RefreshToken)> LoginAsync(string email, string password);
        Task<(UserReadDTO? User, string? Token, string? RefreshToken)> RefreshTokenAsync(string refreshToken);
    }
}
