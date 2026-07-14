using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Interfaces.Repository
{
    public interface IAuthRepository
    {
        Task<User>? RegisterUserAsync(User user, string hash);
        Task<bool> IsExistEmailAsync(string email);
        Task<User>? GetUserByEmailAsync(string email);

    }
}
