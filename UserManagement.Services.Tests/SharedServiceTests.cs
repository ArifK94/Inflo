using UserManagement.Services.Implementations;

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


    private SharedService CreateService() => new();
}
