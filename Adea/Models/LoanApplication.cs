namespace Adea.Models;

public class LoanStatus
{
    private LoanStatus(string value) { Value = value; }

    public string Value { get; private set; }

    public static LoanStatus Unknown { get { return new LoanStatus(""); } }
    public static LoanStatus Wait { get { return new LoanStatus("wait"); } }
    public static LoanStatus Process { get { return new LoanStatus("process"); } }
    public static LoanStatus Reject { get { return new LoanStatus("reject"); } }
    public static LoanStatus Approve { get { return new LoanStatus("approve"); } }

    public override string ToString()
    {
        return Value;
    }
}

public class LoanApplicationDAO
{
    public LoanApplicationDAO()
    {
        Id = Guid.NewGuid().ToString();
        CreatedDate = DateTime.Now;
        UpdatedDate = DateTime.Now;
    }

    public string Id { get; set; } = "";
    public string UserId { get; set; } = "";
    public string? OfficerId { get; set; }
    public string FullName { get; set; } = "";
    public string BirthDate { get; set; } = "";
    public string FullAddress { get; set; } = "";
    public string Phone { get; set; } = "";
    public string IdCardUrl { get; set; } = "";
    public string OtherBusiness { get; set; } = "";
    public string Status { get; set; } = LoanStatus.Unknown.ToString();
    public bool IsPrivateField { get; set; } = false;
    public short ExpInYear { get; set; } = 0;
    public short ActiveFieldNumber { get; set; } = 0;
    public short SowSeedsPerCycle { get; set; } = 0;
    public short NeededFertilizerPerCycleInKg { get; set; } = 0;
    public short EstimatedYieldInKg { get; set; } = 0;
    public short EstimatedPriceOfHarvestPerKg { get; set; } = 0;
    public short HarvestCycleInMonths { get; set; } = 0;
    public long LoanApplicationInIdr { get; set; } = 0;
    public long BusinessIncomePerMonthInIdr { get; set; } = 0;
    public long BusinessOutcomePerMonthInIdr { get; set; } = 0;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public UserDAO? Officer { get; set; } = null;
    public UserDAO? User { get; set; } = null;
}
