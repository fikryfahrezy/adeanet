namespace Adea.Services.Models;

public partial class LoanApplicationDAO
{
	public string Id { get; set; } = null!;
	public string UserId { get; set; } = null!;
	public string? OfficerId { get; set; }
	public string? FullName { get; set; }
	public string? BirthDate { get; set; }
	public string? FullAddress { get; set; }
	public string? Phone { get; set; }
	public string? IdCardUrl { get; set; }
	public string? OtherBusiness { get; set; }
	public string? Status { get; set; }
	public bool? IsPrivateField { get; set; }
	public short? ExpInYear { get; set; }
	public short? ActiveFieldNumber { get; set; }
	public short? SowSeedsPerCycle { get; set; }
	public short? NeededFertilizierPerCycleInKg { get; set; }
	public short? EstimatedYieldInKg { get; set; }
	public short? EstimatedPriceOfHarvestPerKg { get; set; }
	public short? HarvestCycleInMonths { get; set; }
	public long? LoanApplicationInIdr { get; set; }
	public long? BusinessIncomePerMonthInIdr { get; set; }
	public long? BusinessOutcomePerMonthInIdr { get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }

	public virtual UserDAO? Officer { get; set; }
	public virtual UserDAO User { get; set; } = null!;
}
