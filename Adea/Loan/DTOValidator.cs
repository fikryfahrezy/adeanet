using FluentValidation;

namespace Adea.Loan;

public class CreatLoanRequestBodyDTOValidator : AbstractValidator<CreatLoanRequestBodyDTO>
{
	public CreatLoanRequestBodyDTOValidator()
	{
		RuleFor(v => v.FullName).NotEmpty();
		RuleFor(v => v.BirthDate).NotEmpty();
		RuleFor(v => v.FullAddress).NotEmpty();
		RuleFor(v => v.Phone).MinimumLength(10).MaximumLength(15);
		RuleFor(v => v.ExpInYear).NotEmpty().GreaterThan(0);
		RuleFor(v => v.ActiveFieldNumber).NotEmpty().GreaterThan(0);
		RuleFor(v => v.SowSeedsPerCycle).NotEmpty().GreaterThan(0);
		RuleFor(v => v.NeededFertilizerPerCycleInKg).NotEmpty().GreaterThan(0);
		RuleFor(v => v.EstimatedYieldInKg).NotEmpty().GreaterThan(0);
		RuleFor(v => v.EstimatedPriceOfHarvestPerKg).NotEmpty().GreaterThan(0);
		RuleFor(v => v.HarvestCycleInMonths).NotEmpty().GreaterThan(0);
		RuleFor(v => v.LoanApplicationInIdr).NotEmpty().GreaterThan(0);
		RuleFor(v => v.BusinessIncomePerMonthInIdr).NotEmpty().GreaterThan(0);
		RuleFor(v => v.BusinessOutcomePerMonthInIdr).NotEmpty().GreaterThan(0);
		RuleFor(v => v.IdCard).NotEmpty();
	}
}
