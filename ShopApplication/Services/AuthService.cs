using AutoMapper;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Helpers;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Services
{
    public class AuthService(IMapper _mapper, IAuthRepository _repository, IHashHelper _hashHelper, IJWTService _jwtService, IRefreshTokenRepository _refreshTokenRepository) : IAuthService
    {
        public async Task<(UserReadDTO? User, string? Token, string? RefreshToken)> RegisterAsync(UserCreateDTO dto)
        {
            var isExist = await _repository.IsExistEmailAsync(dto.Email);
            if (!isExist)
            {
                var hash = _hashHelper.Hash(dto.Password);
                var user = _mapper.Map<User>(dto);
                var registerUser = await _repository.RegisterUserAsync(user, hash);

                if (registerUser != null)
                {
                    var token = _jwtService.GenerateAccessToken(_mapper.Map<UserLoginDTO>(user), user.Role.ToString());
                    var refreshToken = _jwtService.GenerateRefreshToken();

                    _refreshTokenRepository.AddTokenAsync(new RefreshToken
                    {
                        Token = refreshToken.Item1,
                        UserId = user.Id,
                        IsRevoked = false,
                        CreatedAt = DateTime.UtcNow,
                        ExpiresAt = DateTime.UtcNow.AddDays(refreshToken.Item2)
                    });

                    return (_mapper.Map<UserReadDTO>(registerUser), token, refreshToken.Item1);
                }
            }
            return (null, null, null);
        }

        public async Task<(UserReadDTO? User, string? Token, string? RefreshToken)> LoginAsync(string email, string password)
        {
            var user = await _repository.GetUserByEmailAsync(email);
            if (user != null)
            {
                var isPasswordValid = _hashHelper.IsValidPassword(password, user.PasswordHash);
                if (isPasswordValid)
                {
                    var token = _jwtService.GenerateAccessToken(_mapper.Map<UserLoginDTO>(user), user.Role.ToString());
                    var refreshToken = _jwtService.GenerateRefreshToken();

                    _refreshTokenRepository.AddTokenAsync(new RefreshToken
                    {
                        Token = refreshToken.Item1,
                        UserId = user.Id,
                        IsRevoked = false,
                        CreatedAt = DateTime.UtcNow,
                        ExpiresAt = DateTime.UtcNow.AddDays(refreshToken.Item2)
                    });

                    return (_mapper.Map<UserReadDTO>(user), token, refreshToken.Item1);
                }
            }
            return (null, null, null);
        }

       public async Task<(UserReadDTO? User, string? Token, string? RefreshToken)> RefreshTokenAsync(string refreshToken)
       {
            var token = await _refreshTokenRepository.GetTokenAsync(refreshToken);
            if (token != null && !token.IsRevoked && token.ExpiresAt > DateTime.UtcNow)
            {
                var user = await _repository.GetUserByIdAsync(token.UserId);
                if (user != null)
                {
                    var newToken = _jwtService.GenerateAccessToken(_mapper.Map<UserLoginDTO>(user), user.Role.ToString());
                    var newRefreshToken = _jwtService.GenerateRefreshToken();

                    token.IsRevoked = true;
                    await _refreshTokenRepository.UpdateTokenAsync(token);

                    await _refreshTokenRepository.AddTokenAsync(new RefreshToken
                    {
                        Token = newRefreshToken.Item1,
                        UserId = user.Id,
                        IsRevoked = false,
                        CreatedAt = DateTime.UtcNow,
                        ExpiresAt = DateTime.UtcNow.AddDays(newRefreshToken.Item2)
                    });
                    return (_mapper.Map<UserReadDTO>(user), newToken, newRefreshToken.Item1);
                }
            }
            return (null, null, null);
        }
    }
}
