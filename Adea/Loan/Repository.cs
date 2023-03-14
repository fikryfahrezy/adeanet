using Adea.Data;
using Adea.Exceptions;
using Adea.Models;
using Microsoft.EntityFrameworkCore;

namespace Adea.Loan;

public class LoanRepository
{
    private readonly LoanLosDbContext _dbContext;

    public LoanRepository(LoanLosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private static LoanApplicationDAO LoanModeltoDAO(string userId, string idCardUrl, CreateLoanParam loanApplication) => new()
    {
        IsPrivateField = loanApplication.IsPrivateField,
        ExpInYear = loanApplication.ExpInYear,
        ActiveFieldNumber = loanApplication.ActiveFieldNumber,
        SowSeedsPerCycle = loanApplication.SowSeedsPerCycle,
        NeededFertilizerPerCycleInKg = loanApplication.NeededFertilizerPerCycleInKg,
        EstimatedYieldInKg = loanApplication.EstimatedYieldInKg,
        EstimatedPriceOfHarvestPerKg = loanApplication.EstimatedPriceOfHarvestPerKg,
        HarvestCycleInMonths = loanApplication.HarvestCycleInMonths,
        LoanApplicationInIdr = loanApplication.LoanApplicationInIdr,
        BusinessIncomePerMonthInIdr = loanApplication.BusinessIncomePerMonthInIdr,
        BusinessOutcomePerMonthInIdr = loanApplication.BusinessOutcomePerMonthInIdr,
        UserId = userId,
        FullName = loanApplication.FullName,
        BirthDate = loanApplication.BirthDate,
        FullAddress = loanApplication.FullAddress,
        Phone = loanApplication.Phone,
        IdCardUrl = idCardUrl,
        OtherBusiness = loanApplication.OtherBusiness
    };

    private static Loan LoanDAOtoLoanModel(LoanApplicationDAO loan) => new(
        fullName: loan.FullName,
        loanCreatedDate: loan.CreatedDate.ToString("2006-01-02"),
        loanId: loan.Id,
        loanStatus: loan.Status,
        userId: loan.UserId
    );

    private static LoanDetail LoanDetailDAOtoLoanDetailModel(LoanApplicationDAO loanApplication) => new(
        isPrivateField: loanApplication.IsPrivateField,
        expInYear: loanApplication.ExpInYear,
        activeFieldNumber: loanApplication.ActiveFieldNumber,
        sowSeedsPerCycle: loanApplication.SowSeedsPerCycle,
        neededFertilizerPerCycleInKg: loanApplication.NeededFertilizerPerCycleInKg,
        estimatedYieldInKg: loanApplication.EstimatedPriceOfHarvestPerKg,
        estimatedPriceOfHarvestPerKg: loanApplication.EstimatedPriceOfHarvestPerKg,
        harvestCycleInMonths: loanApplication.HarvestCycleInMonths,
        loanApplicationInIdr: loanApplication.LoanApplicationInIdr,
        businessIncomePerMonthInIdr: loanApplication.BusinessIncomePerMonthInIdr,
        businessOutcomePerMonthInIdr: loanApplication.BusinessOutcomePerMonthInIdr,
        loanId: loanApplication.Id,
        userId: loanApplication.UserId,
        fullName: loanApplication.FullName,
        birthDate: loanApplication.BirthDate,
        fullAddress: loanApplication.FullAddress,
        phone: loanApplication.Phone,
        otherBusiness: loanApplication.OtherBusiness,
        idCardUrl: loanApplication.IdCardUrl,
        status: loanApplication.Status
    );

    public async Task<IEnumerable<Loan>> GetUserLoansAsync(string userId)
    {
        var loanApplications = await _dbContext.LoanApplications.Where(l => l.UserId == userId).ToListAsync();
        return loanApplications.Select(LoanDAOtoLoanModel).ToList();
    }

    public async Task<string> InsertLoanAsync(string userId, string idCardUrl, CreateLoanParam loanApplication)
    {
        var loan = LoanModeltoDAO(userId, idCardUrl, loanApplication);
        _dbContext.LoanApplications.Add(loan);
        await _dbContext.SaveChangesAsync();

        return loan.Id;
    }
    public async Task<string> UpdateLoanAsync(string loanId, string userId, string idCardUrl, CreateLoanParam loanApplication)
    {
        var existingLoan = await _dbContext.LoanApplications.Where(l => l.Id == loanId && l.UserId == userId).FirstOrDefaultAsync();
        if (existingLoan is null)
        {
            throw new NotFoundException($"Loan with {loanId} not found");
        }

        existingLoan.IsPrivateField = loanApplication.IsPrivateField;
        existingLoan.ExpInYear = loanApplication.ExpInYear;
        existingLoan.ActiveFieldNumber = loanApplication.ActiveFieldNumber;
        existingLoan.SowSeedsPerCycle = loanApplication.SowSeedsPerCycle;
        existingLoan.NeededFertilizerPerCycleInKg = loanApplication.NeededFertilizerPerCycleInKg;
        existingLoan.EstimatedYieldInKg = loanApplication.EstimatedPriceOfHarvestPerKg;
        existingLoan.EstimatedPriceOfHarvestPerKg = loanApplication.EstimatedPriceOfHarvestPerKg;
        existingLoan.HarvestCycleInMonths = loanApplication.HarvestCycleInMonths;
        existingLoan.LoanApplicationInIdr = loanApplication.LoanApplicationInIdr;
        existingLoan.BusinessIncomePerMonthInIdr = loanApplication.BusinessIncomePerMonthInIdr;
        existingLoan.BusinessOutcomePerMonthInIdr = loanApplication.BusinessOutcomePerMonthInIdr;
        existingLoan.FullName = loanApplication.FullName;
        existingLoan.BirthDate = loanApplication.BirthDate;
        existingLoan.FullAddress = loanApplication.FullAddress;
        existingLoan.Phone = loanApplication.Phone;
        existingLoan.IdCardUrl = idCardUrl;
        existingLoan.OtherBusiness = loanApplication.OtherBusiness;

        await _dbContext.SaveChangesAsync();

        return loanId;
    }

    public async Task<LoanDetail> GetUserLoanAsync(string loanId, string userId)
    {
        var loan = await _dbContext.LoanApplications.Where(l => l.Id == loanId && l.UserId == userId).FirstOrDefaultAsync();
        if (loan is null)
        {
            throw new NotFoundException($"User id {userId} with loan id {loanId} not found");
        }
        return LoanDetailDAOtoLoanDetailModel(loan);
    }

    public async Task<IEnumerable<Loan>> GetLoansAsync()
    {
        var loanApplications = await _dbContext.LoanApplications.ToListAsync();
        return loanApplications.Select(LoanDAOtoLoanModel).ToList();
    }

    public async Task<LoanDetail> GetLoanAsync(string loanId)
    {
        var loan = await _dbContext.LoanApplications.Where(l => l.Id == loanId).FirstOrDefaultAsync();
        if (loan is null)
        {
            throw new NotFoundException($"Loan with {loanId} not found");
        }
        return LoanDetailDAOtoLoanDetailModel(loan);
    }

    public async Task<string> RemoveLoanAsync(string loanId, string userId)
    {
        var loan = await _dbContext.LoanApplications.Where(l => l.Id == loanId && l.UserId == userId).FirstOrDefaultAsync();
        if (loan is null)
        {
            throw new NotFoundException($"User id {userId} with loan id {loanId} not found");
        }

        _dbContext.LoanApplications.Remove(loan);
        await _dbContext.SaveChangesAsync();

        return loan.Id;
    }


    public async Task<string> UpdateLoanStatusAsync(string loanId, string officerId, LoanStatus loanStatus)
    {
        var existingLoan = await _dbContext.LoanApplications.Where(l => l.Id == loanId).FirstOrDefaultAsync();
        if (existingLoan is null)
        {
            throw new NotFoundException($"Loan with {loanId} not found");
        }

        existingLoan.OfficerId = officerId;
        existingLoan.Status = loanStatus.ToString();

        await _dbContext.SaveChangesAsync();

        return loanId;
    }
}