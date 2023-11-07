using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void ViewUser_FirstUser_DetailsMustBetheFirstUser()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        // Act
        var result = controller.View(0);
        var viewResult = result as ViewResult;


        // Assert
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        viewResult.Model
        .Should().BeOfType<User>()
        .Which.Forename.Should().Be(users[0].Forename);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

    }

    [Fact]
    public void EditUser_FirstUser_UpdateProperties()
    {
        // Arrange
        int userId = 0;
        string newForename = "Jimmy";
        var newDOB = new DateTime(1990, 1, 1);

        var controller = CreateController();
        var users = SetupUsers();

        // Act
        var dbUser = _userService.Object.GetAll().First();
        dbUser.Forename = newForename;
        dbUser.DateOfBirth = newDOB;

        controller.Edit(userId, dbUser);

        var userAfterEdit = _userService.Object.GetAll().First();

        // Assert
        userAfterEdit.Forename.Should().Be(newForename);
        userAfterEdit.DateOfBirth.Should().Be(newDOB);

    }

    [Fact]
    public void DeleteUser_FirstUser_WhenUserDoesNotExist()
    {
        // Arrange
        int userId = 0;

        var controller = CreateController();
        var users = SetupUsers();

        // Act
        controller.Delete(userId);

       // var dbUsers = _userService.Setup(s => s.GetAll()).Returns(users);
        var userAfterDeletion = _userService.Object.FindUser(userId);

        // Assert
        userAfterDeletion.Should().BeNull();

    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }


    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<ISharedService> _sharedService = new();

    private UsersController CreateController() => new(_userService.Object, _sharedService.Object);
}
