﻿using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    IEnumerable<User> FilterByActive(bool isActive);
    IEnumerable<User> GetAll();
    IEnumerable<User> CreateUser(User user);
    User FindUser(long id);
    User EditUser(User user);
    User DeleteUser(User user);

}
