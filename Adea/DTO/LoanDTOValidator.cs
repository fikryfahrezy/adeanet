using FluentValidation;
using System.Globalization;

namespace Adea.DTO;

public class CreatLoanRequestBodyDTOValidator : AbstractValidator<CreateLoanRequestBodyDTO>
{
    public CreatLoanRequestBodyDTOValidator()
    {
        RuleFor(v => v.FullName).NotEmpty();
        RuleFor(v => v.BirthDate)
            .Must(v => DateTime.TryParseExact(
                    v,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime tmp
                ) == true
            )
            .NotEmpty();
        RuleFor(v => v.FullAddress).NotEmpty();
        RuleFor(v => v.Phone).Matches("^[0-9]*$").MinimumLength(10).MaximumLength(15);
        RuleFor(v => v.ExpInYear).NotEmpty().GreaterThan((short)0);
        RuleFor(v => v.ActiveFieldNumber).NotEmpty().GreaterThan((short)0);
        RuleFor(v => v.SowSeedsPerCycle).NotEmpty().GreaterThan((short)0);
        RuleFor(v => v.NeededFertilizerPerCycleInKg).NotEmpty().GreaterThan((short)0);
        RuleFor(v => v.EstimatedYieldInKg).NotEmpty().GreaterThan((short)0);
        RuleFor(v => v.EstimatedPriceOfHarvestPerKg).NotEmpty().GreaterThan((short)0);
        RuleFor(v => v.HarvestCycleInMonths).NotEmpty().GreaterThan((short)0);
        RuleFor(v => v.LoanApplicationInIdr).NotEmpty().GreaterThan(0);
        RuleFor(v => v.BusinessIncomePerMonthInIdr).NotEmpty().GreaterThan(0);
        RuleFor(v => v.BusinessOutcomePerMonthInIdr).NotEmpty().GreaterThan(0);
        RuleFor(v => v.IdCard).NotEmpty();
    }
}
