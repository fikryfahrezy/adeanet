using Adea.Exceptions;

namespace Adea.Loan;

public class LoanApplication
{
    public string FullName;
    public string BirthDate;
    public string FullAddress;
    public string Phone;
    public string OtherBusiness;
    public bool IsPrivateField;
    public short ExpInYear;
    public short ActiveFieldNumber;
    public short SowSeedsPerCycle;
    public short NeededFertilizerPerCycleInKg;
    public short EstimatedYieldInKg;
    public short EstimatedPriceOfHarvestPerKg;
    public short HarvestCycleInMonths;
    public int LoanApplicationInIdr;
    public int BusinessIncomePerMonthInIdr;
    public int BusinessOutcomePerMonthInIdr;
    public IFormFile IdCard;

    public LoanApplication(
        string fullName,
        string birthDate,
        string fullAddress,
        string phone,
        string otherBusiness,
        bool isPrivateField,
        short expInYear,
        short activeFieldNumber,
        short sowSeedsPerCycle,
        short neededFertilizerPerCycleInKg,
        short estimatedYieldInKg,
        short estimatedPriceOfHarvestPerKg,
        short harvestCycleInMonths,
        int loanApplicationInIdr,
        int businessIncomePerMonthInIdr,
        int businessOutcomePerMonthInIdr,
        IFormFile? idCard
    )
    {

        if (idCard is null)
        {
            throw new UnprocessableEntityException(nameof(idCard));
        }

        FullName = fullName;
        BirthDate = birthDate;
        FullAddress = fullAddress;
        Phone = phone;
        OtherBusiness = otherBusiness;
        IsPrivateField = isPrivateField;
        ExpInYear = expInYear;
        ActiveFieldNumber = activeFieldNumber;
        SowSeedsPerCycle = sowSeedsPerCycle;
        NeededFertilizerPerCycleInKg = neededFertilizerPerCycleInKg;
        EstimatedYieldInKg = estimatedYieldInKg;
        EstimatedPriceOfHarvestPerKg = estimatedPriceOfHarvestPerKg;
        HarvestCycleInMonths = harvestCycleInMonths;
        LoanApplicationInIdr = loanApplicationInIdr;
        BusinessIncomePerMonthInIdr = businessIncomePerMonthInIdr;
        BusinessOutcomePerMonthInIdr = businessOutcomePerMonthInIdr;
        IdCard = idCard;
    }
}

public class Loan
{
    public string LoanId;
    public string UserId;
    public string FullName;
    public string LoanStatus;
    public string LoanCreatedDate;

    public Loan(string loanId, string userId, string fullName, string loanStatus, string loanCreatedDate)
    {
        LoanId = loanId;
        UserId = userId;
        FullName = fullName;
        LoanStatus = loanStatus;
        LoanCreatedDate = loanCreatedDate;
    }
}

public class LoanDetail
{
    public bool IsPrivateField;
    public long ExpInYear;
    public long ActiveFieldNumber;
    public long SowSeedsPerCycle;
    public long NeededFertilizerPerCycleInKg;
    public long EstimatedYieldInKg;
    public long EstimatedPriceOfHarvestPerKg;
    public long HarvestCycleInMonths;
    public long LoanApplicationInIdr;
    public long BusinessIncomePerMonthInIdr;
    public long BusinessOutcomePerMonthInIdr;
    public string LoanId;
    public string UserId;
    public string FullName;
    public string BirthDate;
    public string FullAddress;
    public string Phone;
    public string OtherBusiness;
    public string IdCardUrl;
    public string Status;

    public LoanDetail(
        bool isPrivateField,
        long expInYear,
        long activeFieldNumber,
        long sowSeedsPerCycle,
        long neededFertilizerPerCycleInKg,
        long estimatedYieldInKg,
        long estimatedPriceOfHarvestPerKg,
        long harvestCycleInMonths,
        long loanApplicationInIdr,
        long businessIncomePerMonthInIdr,
        long businessOutcomePerMonthInIdr,
        string loanId,
        string userId,
        string fullName,
        string birthDate,
        string fullAddress,
        string phone,
        string otherBusiness,
        string idCardUrl,
        string status
    )
    {
        IsPrivateField = isPrivateField;
        ExpInYear = expInYear;
        ActiveFieldNumber = activeFieldNumber;
        SowSeedsPerCycle = sowSeedsPerCycle;
        NeededFertilizerPerCycleInKg = neededFertilizerPerCycleInKg;
        EstimatedYieldInKg = estimatedYieldInKg;
        EstimatedPriceOfHarvestPerKg = estimatedPriceOfHarvestPerKg;
        HarvestCycleInMonths = harvestCycleInMonths;
        LoanApplicationInIdr = loanApplicationInIdr;
        BusinessIncomePerMonthInIdr = businessIncomePerMonthInIdr;
        BusinessOutcomePerMonthInIdr = businessOutcomePerMonthInIdr;
        LoanId = loanId;
        UserId = userId;
        FullName = fullName;
        BirthDate = birthDate;
        FullAddress = fullAddress;
        Phone = phone;
        OtherBusiness = otherBusiness;
        IdCardUrl = idCardUrl;
        Status = status;
    }
}
