﻿using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        return GetAll().Where(x => x.IsActive == isActive);
    }

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();
    public IEnumerable<User> CreateUser(User user)
    {
        _dataAccess.Create(user);
        return GetAll();
    }

    public User FindUser(long id)
    {
        return GetAll().Where(_x => _x.Id == id).First();
    }

    User IUserService.EditUser(User user)
    {
        _dataAccess.Update(user);
        return user;
    }

    public User DeleteUser(User user)
    {
        _dataAccess.Delete(user);
        return user;
    }
}
