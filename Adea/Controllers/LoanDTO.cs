using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Adea.Controllers;

public class CreateLoanRequestBodyDTO
{
    [BindProperty(Name = "full_name")]
    public string FullName { get; set; } = "";

    [BindProperty(Name = "birth_date")]
    public string BirthDate { get; set; } = "";

    [BindProperty(Name = "full_address")]
    public string FullAddress { get; set; } = "";

    [BindProperty(Name = "phone")]
    public string Phone { get; set; } = "";

    [BindProperty(Name = "other_business")]
    public string OtherBusiness { get; set; } = "";

    [BindProperty(Name = "is_private_field")]
    public bool IsPrivateField { get; set; }

    [BindProperty(Name = "exp_in_year")]
    public short ExpInYear { get; set; }

    [BindProperty(Name = "active_field_number")]
    public short ActiveFieldNumber { get; set; }

    [BindProperty(Name = "sow_seeds_per_cycle")]
    public short SowSeedsPerCycle { get; set; }

    [BindProperty(Name = "needed_fertilizer_per_cycle_in_kg")]
    public short NeededFertilizerPerCycleInKg { get; set; }

    [BindProperty(Name = "estimated_yield_in_kg")]
    public short EstimatedYieldInKg { get; set; }

    [BindProperty(Name = "estimated_price_of_harvest_per_kg")]
    public short EstimatedPriceOfHarvestPerKg { get; set; }

    [BindProperty(Name = "harvest_cycle_in_months")]
    public short HarvestCycleInMonths { get; set; }

    [BindProperty(Name = "loan_application_in_idr")]
    public int LoanApplicationInIdr { get; set; }

    [BindProperty(Name = "business_income_per_month_in_idr")]
    public int BusinessIncomePerMonthInIdr { get; set; }

    [BindProperty(Name = "business_outcome_per_month_in_idr")]
    public int BusinessOutcomePerMonthInIdr { get; set; }

    [BindProperty(Name = "id_card")]
    public IFormFile? IdCard { get; set; } = null;
}

public class CreateLoanResponseBodyDTO
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";
}

public class GetLoanResponseBodyDTO
{
    [JsonPropertyName("loan_id")]
    public string LoanId { get; set; } = "";

    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = "";

    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = "";

    [JsonPropertyName("loan_status")]
    public string LoanStatus { get; set; } = "";

    [JsonPropertyName("loan_created_date")]
    public string LoanCreatedDate { get; set; } = "";
}

public class GetLoanDetailResponseBodyDTO
{
    [JsonPropertyName("is_private_field")]
    public bool IsPrivateField { get; set; }

    [JsonPropertyName("exp_in_year")]
    public long ExpInYear { get; set; }

    [JsonPropertyName("active_field_number")]
    public long ActiveFieldNumber { get; set; }

    [JsonPropertyName("sow_seeds_per_cycle")]
    public long SowSeedsPerCycle { get; set; }

    [JsonPropertyName("needed_fertilizer_per_cycle_in_kg")]
    public long NeededFertilizerPerCycleInKg { get; set; }

    [JsonPropertyName("estimated_yield_in_kg")]
    public long EstimatedYieldInKg { get; set; }

    [JsonPropertyName("estimated_price_of_harvest_per_kg")]
    public long EstimatedPriceOfHarvestPerKg { get; set; }

    [JsonPropertyName("harvest_cycle_in_months")]
    public long HarvestCycleInMonths { get; set; }

    [JsonPropertyName("loan_application_in_idr")]
    public long LoanApplicationInIdr { get; set; }

    [JsonPropertyName("business_income_per_month_in_idr")]
    public long BusinessIncomePerMonthInIdr { get; set; }

    [JsonPropertyName("business_outcome_per_month_in_idr")]
    public long BusinessOutcomePerMonthInIdr { get; set; }

    [JsonPropertyName("loan_id")]
    public string LoanId { get; set; } = "";

    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = "";

    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = "";

    [JsonPropertyName("birth_date")]
    public string BirthDate { get; set; } = "";

    [JsonPropertyName("full_address")]
    public string FullAddress { get; set; } = "";

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = "";

    [JsonPropertyName("other_business")]
    public string OtherBusiness { get; set; } = "";

    [JsonPropertyName("id_card_url")]
    public string IdCardUrl { get; set; } = "";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "";
}
public class ApproveLoanRequestBodyDTO
{
    [BindProperty(Name = "is_approve")]
    public bool IsApprove { get; set; }
}