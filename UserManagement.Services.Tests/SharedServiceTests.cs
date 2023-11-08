using UserManagement.Services.Implementations;
using UserManagement.Models;
using System;
using UserManagement.Web.Models.Users;

namespace UserManagement.Data.Tests;

public class SharedServiceTests
{

    [Fact]
    public void Return_Valid_EmailAddress()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.IsValidEmail("test@outlook.com");

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeTrue();
    }

    [Fact]
    public void Return_Invalid_EmailAddress()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.IsValidEmail("1");

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeFalse();
    }

    [Fact]
    public void CopyObjectProperties_Between_TwoUsers()
    {
        // Arrange
        var service = CreateService();

        User user = new User
        {
            Id = 1,
            Forename = "Peter",
            Surname = "Loew",
            Email = "ploew@example.com",
            DateOfBirth = new DateTime(1990, 6, 10),
            IsActive = true
        };

        User cloneUser = new User();

        // Act
        service.CopyObjectProperties(user, cloneUser);

        // Assert
        cloneUser.Should().BeEquivalentTo(user);
    }

    [Fact]
    public void CopyObjectProperties_Between_TwoDifferentObjects()
    {
        // Arrange
        var service = CreateService();

        User user = new User
        {
            Id = 1,
            Forename = "Peter",
            Surname = "Loew",
            Email = "ploew@example.com",
            DateOfBirth = new DateTime(1990, 6, 10),
            IsActive = true
        };

        UserListItemViewModel userVM = new UserListItemViewModel();

        // Act
        service.CopyObjectProperties(user, userVM);

        // Assert
        userVM.Forename.Should().Be(user.Forename);
        userVM.DateOfBirth.Should().Be(user.DateOfBirth);
    }


    private SharedService CreateService() => new();
}
