using ShopApplication.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(UserReadDTO? User, string? Token)> RegisterAsync(UserCreateDTO dto);
        Task<(UserReadDTO? User, string? Token)> LoginAsync(string email, string password);
    }
}
