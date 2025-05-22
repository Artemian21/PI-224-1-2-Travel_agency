using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Enums;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models.Users;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<bool> AssignUserRoleAsync(Guid userId, UserRole role)
        {
            var userEntity = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (userEntity == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            userEntity.Role = role;
            await _unitOfWork.Users.UpdateUserAsync(userEntity);
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var userEntity = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (userEntity == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            await _unitOfWork.Users.DeleteUserAsync(userId);
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUserAsync()
        {
            var userEntities = await _unitOfWork.Users.GetAllUsers();
            return _mapper.Map<IEnumerable<UserDto>>(userEntities);
        }

        public async Task<UserWithBookingsDto> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BusinessValidationException("Email cannot be empty.");
            }

            var userEntity = await _unitOfWork.Users.GetUserByEmailAsync(email);
            if (userEntity == null)
            {
                throw new NotFoundException($"User with email '{email}' not found.");
            }

            return _mapper.Map<UserWithBookingsDto>(userEntity);
        }

        public async Task<UserWithBookingsDto> GetUserByIdAsync(Guid userId)
        {
            var userEntity = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (userEntity == null)
            {
                throw new NotFoundException($"User with ID {userId} not found.");
            }

            return _mapper.Map<UserWithBookingsDto>(userEntity);
        }

        public async Task<UserDto> UpdateUserProfileAsync(UserDto updateDto)
        {
            ValidateUserDto(updateDto);

            var userEntity = await _unitOfWork.Users.GetUserByIdAsync(updateDto.Id);
            var updatedUserEntity = await _unitOfWork.Users.UpdateUserAsync(userEntity);
            if (updatedUserEntity == null)
            {
                throw new NotFoundException($"User with ID {updateDto.Id} not found.");
            }

            return _mapper.Map<UserDto>(updatedUserEntity);
        }

        private void ValidateUserDto(UserDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "User object cannot be null.");

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new BusinessValidationException("Username is required.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new BusinessValidationException("Email is required.");

            if (!Enum.IsDefined(typeof(UserRole), dto.Role))
                throw new BusinessValidationException("Invalid user role.");
        }
    }
}
