using Adea.Data;

namespace Adea.Loan;
public class LoanRepository
{
	private readonly LoanLosDbContext _dbContext;

	public LoanRepository(LoanLosDbContext dbContext)
	{
		_dbContext = dbContext;
	}
}