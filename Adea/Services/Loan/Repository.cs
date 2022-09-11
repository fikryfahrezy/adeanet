using Adea.Data;

namespace Adea.Services.Loan;
public class LoanRepository
{
	private readonly LoanLosDbContext _dbContext;

	public LoanRepository(LoanLosDbContext dbContext)
	{
		_dbContext = dbContext;
	}
}