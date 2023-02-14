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
        var userId = GetUserId();
        var validator = new CreatLoanRequestBodyDTOValidator();
        await validator.ValidateAndThrowAsync(loanRequest);
        return await _loanService.CreateLoanAsync(userId, loanRequest);
    }

    [HttpGet("getall")]
    public async Task<ActionResult<IEnumerable<GetUserLoanResponseBodyDTO>>> GetUserLoanApplicationsAsync()
    {
        var userId = GetUserId();
        return await _loanService.GetUserLoansAsync(userId);
    }

    private string GetUserId()
    {
        var userId = Request.Headers.Authorization.FirstOrDefault();
        if (userId == null)
        {
            throw new UnprocessableEntityException("Authorization required");
        }

        return userId;
    }

    [HttpGet("get/{loanId}")]
    public async Task<ActionResult<GetUserLoanDetailResponseBodyDTO>> GetUserLoanApplicationAsync(string loanId)
    {
        var userId = GetUserId();
        return await _loanService.GetUserLoanDetailAsync(loanId, userId);
    }
}