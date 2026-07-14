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
    public class AuthService(IMapper _mapper, IAuthRepository _repository, IHashHelper _hashHelper, IJWTService _jwtService) : IAuthService
    {
        public async Task<(UserReadDTO? User, string? Token)> RegisterAsync(UserCreateDTO dto)
        {

            var isExist = await _repository.IsExistEmailAsync(dto.Email);
            if (!isExist)
            {
                var hash = _hashHelper.Hash(dto.Password);
                var user = _mapper.Map<User>(dto);

                var token = _jwtService.GenerateAccessToken(_mapper.Map<UserLoginDTO>(user), user.Role.ToString());
                var registerUser = await _repository.RegisterUserAsync(user, hash);
                if (registerUser != null)
                    return (_mapper.Map<UserReadDTO>(registerUser), token);
            }
            return (null, null);
        }

        public async Task<(UserReadDTO? User, string? Token)> LoginAsync(string email, string password)
        {
            var user = await _repository.GetUserByEmailAsync(email);
            if (user != null)
            {
                var isPasswordValid = _hashHelper.IsValidPassword(password, user.PasswordHash);
                if (isPasswordValid)
                {
                    var token = _jwtService.GenerateAccessToken(_mapper.Map<UserLoginDTO>(user), user.Role.ToString());
                    return (_mapper.Map<UserReadDTO>(user), token);
                }
            }
            return (null, null);
        }
    }
}
