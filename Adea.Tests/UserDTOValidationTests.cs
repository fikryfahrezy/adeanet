using Adea.User;
using FluentValidation;
using Xunit;

namespace Adea.Tests;
public class UserDTOValidationTests
{
    public static IEnumerable<object[]> VerifyUserValidationCases
    => new object[][] {
			// Login fail, no input provided
			new object[] {
                new LoginRequestBodyDTO {
                },
            },
			// Login fail, no username provided
			new object[] {
                new LoginRequestBodyDTO {
                    Password = "password",
                },
            },
			// Login fail, no password provided
			new object[] {
                new LoginRequestBodyDTO {
                    Username = "username",
                },
            },
    };

    [Theory]
    [MemberData(nameof(VerifyUserValidationCases))]
    public async Task LoginRequestBodyDTO_Validation_Fail(LoginRequestBodyDTO request)
    {
        var validator = new LoginRequestBodyDTODTOValidator();
        await Assert.ThrowsAsync<ValidationException>(async () => await validator.ValidateAndThrowAsync(request));
    }


    public static IEnumerable<object[]> SaveUserValidationCases
        => new object[][] {
			// Register fail, no input provided
			new object[] {
                new RegisterRequestBodyDTO {
                },
            },
			// Register fail, no username provided
			new object[] {
                new RegisterRequestBodyDTO {
                    Password = "password",
                },
            },
			// Register fail, no password provided
			new object[] {
                new RegisterRequestBodyDTO {
                    Username = "username",
                },
            },
        };

    [Theory]
    [MemberData(nameof(SaveUserValidationCases))]
    public async Task RegisterRequestBodyDTO_Validation_Fail(RegisterRequestBodyDTO request)
    {
        var validator = new RegisterRequestBodyDTOValidator();
        await Assert.ThrowsAsync<ValidationException>(async () => await validator.ValidateAndThrowAsync(request));
    }
}

