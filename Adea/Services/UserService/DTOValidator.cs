using FluentValidation;

namespace Adea.Services.UserService;

public class RegisterDTOValidator : AbstractValidator<RegisterRequestBodyDTO>
{
	public RegisterDTOValidator()
	{
		RuleFor(v => v.Username).NotEmpty();
		RuleFor(v => v.Password).NotEmpty();
		RuleFor(v => v.IsOfficer).NotNull();
	}
}
