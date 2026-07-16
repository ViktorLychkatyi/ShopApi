using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Interfaces.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<int?> AddTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetTokenAsync(string token);
        Task UpdateTokenAsync(RefreshToken token);
    }
}
