using Microsoft.AspNetCore.Mvc;

namespace Adea.Loan;

public record CreatLoanRequestBodyDTO
{
	[BindProperty(Name = "full_name")]
	public string FullName { get; set; } = "";

	[BindProperty(Name = "birth_date")]
	public DateTime? BirthDate { get; set; }

	[BindProperty(Name = "full_address")]
	public string FullAddress { get; set; } = "";

	[BindProperty(Name = "phone")]
	public string Phone { get; set; } = "";

	[BindProperty(Name = "other_business")]
	public string OtherBusiness { get; set; } = "";

	[BindProperty(Name = "is_private_field")]
	public bool IsPrivateField { get; set; } = false;

	[BindProperty(Name = "exp_in_year")]
	public int ExpInYear { get; set; } = 0;

	[BindProperty(Name = "active_field_number")]
	public int ActiveFieldNumber { get; set; } = 0;

	[BindProperty(Name = "sow_seeds_per_cycle")]
	public int SowSeedsPerCycle { get; set; } = 0;

	[BindProperty(Name = "needed_fertilizer_per_cycle_in_kg")]
	public int NeededFertilizerPerCycleInKg { get; set; } = 0;

	[BindProperty(Name = "estimated_yield_in_kg")]
	public int EstimatedYieldInKg { get; set; } = 0;

	[BindProperty(Name = "estimated_price_of_harvest_per_kg")]
	public int EstimatedPriceOfHarvestPerKg { get; set; } = 0;

	[BindProperty(Name = "harvest_cycle_in_months")]
	public int HarvestCycleInMonths { get; set; } = 0;

	[BindProperty(Name = "loan_application_in_idr")]
	public int LoanApplicationInIdr { get; set; } = 0;

	[BindProperty(Name = "business_income_per_month_in_idr")]
	public int BusinessIncomePerMonthInIdr { get; set; } = 0;

	[BindProperty(Name = "business_outcome_per_month_in_idr")]
	public int BusinessOutcomePerMonthInIdr { get; set; } = 0;

	[BindProperty(Name = "id_card")]
	public IFormFile? IdCard { get; set; }
}