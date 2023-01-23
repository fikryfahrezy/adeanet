using Microsoft.AspNetCore.Mvc;
using Adea.Loan;

namespace Adea.Controllers;

[Route("api/[controller]")]
public class LoansController : ControllerBase
{
	private readonly LoanService _loanService;

	public LoansController(LoanService loanService)
	{
		_loanService = loanService;
	}

	[HttpPost("create")]
	public async Task<ActionResult<string>> PostLoanApplication([FromForm] CreatLoanRequestBodyDTO loanRequest)
	{
		return await _loanService.CreateLoan(loanRequest);
	}
}