using Microsoft.AspNetCore.Mvc;
using Adea.DTO;
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

    [HttpGet("getall")]
    public async Task<ActionResult<IEnumerable<GetLoanResponseBodyDTO>>> GetUserLoanApplicationsAsync()
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
    public async Task<ActionResult<GetLoanDetailResponseBodyDTO>> GetUserLoanApplicationAsync(string loanId)
    {
        var userId = GetUserId();
        return await _loanService.GetUserLoanDetailAsync(loanId, userId);
    }

    [HttpPost("create")]
    public async Task<ActionResult<CreateLoanResponseBodyDTO>> PostLoanApplicationAsync([FromForm] CreateLoanRequestBodyDTO loanRequest)
    {
        var userId = GetUserId();

        var validator = new CreateLoanRequestBodyDTOValidator();
        await validator.ValidateAndThrowAsync(loanRequest);

        var loanApplication = CreateLoanDTOToModel(loanRequest);

        return await _loanService.CreateLoanAsync(userId, loanApplication);
    }

    [HttpPut("update/{loanId}")]
    public async Task<ActionResult<CreateLoanResponseBodyDTO>> UpdateLoanApplicationAsync(string loanId, [FromForm] CreateLoanRequestBodyDTO loanRequest)
    {
        var userId = GetUserId();

        var validator = new CreateLoanRequestBodyDTOValidator();
        await validator.ValidateAndThrowAsync(loanRequest);

        var loanApplication = CreateLoanDTOToModel(loanRequest);

        return await _loanService.UpdateLoanAsync(userId, loanId, loanApplication);
    }


    [HttpPut("update/{loanId}")]
    public async Task<ActionResult<CreateLoanResponseBodyDTO>> DeleteLoanApplicationAsync(string loanId)
    {
        var userId = GetUserId();
        return await _loanService.DeleteLoanAsync(userId, loanId);
    }

    [HttpGet("getall/admin")]
    public async Task<ActionResult<IEnumerable<GetLoanResponseBodyDTO>>> GetLoanAsync()
    {
        return await _loanService.GetLoansAsync();
    }

    [HttpGet("get/admin/{loanId}")]
    public async Task<ActionResult<GetLoanDetailResponseBodyDTO>> GetLoanAsync(string loanId)
    {
        return await _loanService.GetLoanDetailAsync(loanId);
    }

    private static LoanApplication CreateLoanDTOToModel(CreateLoanRequestBodyDTO createLoanRequest) => new(
            fullName: createLoanRequest.FullName,
            birthDate: createLoanRequest.BirthDate,
            fullAddress: createLoanRequest.FullAddress,
            phone: createLoanRequest.Phone,
            otherBusiness: createLoanRequest.OtherBusiness,
            isPrivateField: createLoanRequest.IsPrivateField,
            expInYear: createLoanRequest.ExpInYear,
            activeFieldNumber: createLoanRequest.ActiveFieldNumber,
            sowSeedsPerCycle: createLoanRequest.SowSeedsPerCycle,
            neededFertilizerPerCycleInKg: createLoanRequest.NeededFertilizerPerCycleInKg,
            estimatedYieldInKg: createLoanRequest.EstimatedYieldInKg,
            estimatedPriceOfHarvestPerKg: createLoanRequest.EstimatedPriceOfHarvestPerKg,
            harvestCycleInMonths: createLoanRequest.HarvestCycleInMonths,
            loanApplicationInIdr: createLoanRequest.LoanApplicationInIdr,
            businessIncomePerMonthInIdr: createLoanRequest.BusinessIncomePerMonthInIdr,
            businessOutcomePerMonthInIdr: createLoanRequest.BusinessOutcomePerMonthInIdr,
            idCard: createLoanRequest.IdCard
    );
}