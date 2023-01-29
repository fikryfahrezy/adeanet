using Microsoft.AspNetCore.Mvc;
using Adea.Loan;

namespace Adea.Controllers;

[Route("[controller]")]
public class LoanController : ControllerBase
{
	private readonly LoanService _loanService;

	public LoanController(LoanService loanService)
	{
		_loanService = loanService;
	}

	[HttpPost("create")]
	public async Task<ActionResult<string>> PostLoanApplicationAsync([FromForm] CreatLoanRequestBodyDTO loanRequest)
	{
		return await _loanService.CreateLoanAsync(loanRequest);
	}
}