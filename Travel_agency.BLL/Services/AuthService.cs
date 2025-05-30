using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Enums;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models.Users;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJWTProvider _jwtProvider;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJWTProvider jwtProvider, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
        }

        public async Task<UserDto> Register(RegisterUserDto registerDto)
        {
            if (registerDto == null)
                throw new ArgumentNullException(nameof(registerDto), "Register data cannot be null");

            var existingByEmail = await _userRepository.GetUserByEmailAsync(registerDto.Email);
            if (existingByEmail != null)
                throw new ConflictException("Email is already in use");

            if (!IsPasswordStrong(registerDto.Password))
                throw new ValidationException("Password must be at least 8 characters long and include uppercase, lowercase, digit, and special character");

            var passwordHash = _passwordHasher.GenerateHash(registerDto.Password);

            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = UserRole.Registered
            };

            var addedUser = await _userRepository.AddUserAsync(userEntity);
            return _mapper.Map<UserDto>(addedUser);
        }

        public async Task<string> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new BusinessValidationException("Email and password must not be empty");

            var userEntity = await _userRepository.GetUserByEmailAsync(email);
            if (userEntity == null)
                throw new NotFoundException("User not found");

            if (!_passwordHasher.VerifyHash(password, userEntity.PasswordHash))
                throw new UnauthorizedAccessException("Invalid password");

            var userDto = _mapper.Map<UserDto>(userEntity);
            return _jwtProvider.GenerateToken(userDto);
        }

        private bool IsPasswordStrong(string password)
        {
            var pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
