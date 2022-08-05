namespace Adea.Services.Loan;

public class LoanService
{
	private readonly LoanRepository _repository;

	public LoanService(LoanRepository repository)
	{
		_repository = repository;
	}
}