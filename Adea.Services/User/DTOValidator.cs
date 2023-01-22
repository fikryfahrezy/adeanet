using FluentValidation;

namespace Adea.Services.User;

public class RegisterRequestBodyDTOValidator : AbstractValidator<RegisterRequestBodyDTO>
{
	public RegisterRequestBodyDTOValidator()
	{
		RuleFor(v => v.Username).NotEmpty();
		RuleFor(v => v.Password).NotEmpty();
	}
}


public class LoginRequestBodyDTODTOValidator : AbstractValidator<LoginRequestBodyDTO>
{
	public LoginRequestBodyDTODTOValidator()
	{
		RuleFor(v => v.Username).NotEmpty();
		RuleFor(v => v.Password).NotEmpty();
	}
}