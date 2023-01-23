using FluentValidation;

namespace Adea.Loan;

public class LoanService
{
	private readonly LoanRepository _repository;

	public LoanService(LoanRepository repository)
	{
		_repository = repository;
	}

	public async Task<string> CreateLoan(CreatLoanRequestBodyDTO loanRequest)
	{
		var validator = new CreatLoanRequestBodyDTOValidator();
		await validator.ValidateAndThrowAsync(loanRequest);

		return loanRequest.FullName;
	}
}