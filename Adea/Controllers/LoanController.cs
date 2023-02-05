using Microsoft.AspNetCore.Mvc;
using Adea.Loan;
using Adea.Exceptions;
using FluentValidation;

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
	public async Task<ActionResult<CreateLoanResponseBodyDTO>> PostLoanApplicationAsync([FromForm] CreateLoanRequestBodyDTO loanRequest)
    {
		var userId = Request.Headers.Authorization.FirstOrDefault();
		if (userId == null)
		{
			throw new UnprocessableEntityException("Authorization required");
		}

        var validator = new CreatLoanRequestBodyDTOValidator();
        await validator.ValidateAndThrowAsync(loanRequest);
        return await _loanService.CreateLoanAsync(userId, loanRequest);
	}
}