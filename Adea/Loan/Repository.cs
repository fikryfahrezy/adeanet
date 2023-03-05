using Adea.Data;
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

    public async Task<IEnumerable<Loan>> GetUserLoansAsync(string userId)
    {
        var loanApplications = await _dbContext.LoanApplications.Where(l => l.UserId == userId).ToListAsync();
        return loanApplications.Select(LoanDAOtoLoanModel).ToList();
    }

    public async Task<string> InsertLoanAsync(string userId, string idCardUrl, LoanApplication loanApplication)
    {
        var loan = LoanModeltoDAO(userId, idCardUrl, loanApplication);
        _dbContext.LoanApplications.Add(loan);
        await _dbContext.SaveChangesAsync();

        return loan.Id;
    }

    public async Task<LoanApplicationDAO?> GetUserLoanAsync(string loanId, string userId)
    {
        return await _dbContext.LoanApplications.Where(l => l.Id == loanId && l.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Loan>> GetLoansAsync()
    {
        var loanApplications = await _dbContext.LoanApplications.ToListAsync();
        return loanApplications.Select(LoanDAOtoLoanModel).ToList();
    }

    public async Task<LoanApplicationDAO?> GetLoanAsync(string loanId)
    {
        return await _dbContext.LoanApplications.Where(l => l.Id == loanId).FirstOrDefaultAsync();
    }

    private static LoanApplicationDAO LoanModeltoDAO(string userId, string idCardUrl, LoanApplication loanApplication) => new()
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
}