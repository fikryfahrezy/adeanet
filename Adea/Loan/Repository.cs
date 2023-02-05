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

    public async Task<IEnumerable<LoanApplicationDAO>> GetUserLoansAsync(string userId)
    {
        return await _dbContext.LoanApplications.Where(l => l.UserId == userId).ToListAsync();
    }

    public async Task InsertLoanAsync(LoanApplicationDAO loanApplication)
    {
        _dbContext.LoanApplications.Add(loanApplication);
        await _dbContext.SaveChangesAsync();
    }
}