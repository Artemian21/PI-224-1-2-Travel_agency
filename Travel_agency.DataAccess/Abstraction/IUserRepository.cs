﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.DataAccess.Abstraction
{
    public interface IUserRepository
    {
        Task<UserEntity> AddUserAsync(UserEntity user);
        Task DeleteUserAsync(Guid userId);
        Task<IEnumerable<UserEntity>> GetAllUsers();
        Task<UserEntity> GetUserByIdAsync(Guid userId);
        Task<UserEntity> UpdateUserAsync(UserEntity updatedUser);
    }


}
