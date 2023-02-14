using Adea.Interface;
using Adea.Models;
using Adea.User;
using Adea.Exceptions;
using System.Numerics;

namespace Adea.Loan;

public class LoanService
{
    private readonly LoanRepository _loanRepository;
    private readonly UserRepository _userRepository;
    private readonly IFileUploader _fileUploader;

    public LoanService(LoanRepository loanRepository, UserRepository userRepository, IFileUploader fileUploader)
    {
        _loanRepository = loanRepository;
        _userRepository = userRepository;
        _fileUploader = fileUploader;
    }

    public async Task<CreateLoanResponseBodyDTO> CreateLoanAsync(string userId, CreateLoanRequestBodyDTO loanRequest)
    {
        var userLoans = await _loanRepository.GetUserLoansAsync(userId);
        foreach (var userLoan in userLoans)
        {
            if (userLoan.Status.ToString() == LoanStatus.Wait.ToString() || userLoan.Status.ToString() == LoanStatus.Process.ToString())
            {
                throw new UnprocessableEntityException("Already have processed loan");
            }
        }

        var idCardFilePath = "";

        if (loanRequest.IdCard is not null)
        {
            idCardFilePath = await _fileUploader.UploadFileAsync(loanRequest.IdCard);
        }

        var newLoanApplication = LoanApplicationDTOtoDAO(userId, idCardFilePath, loanRequest);
        await _loanRepository.InsertLoanAsync(newLoanApplication);

        return new CreateLoanResponseBodyDTO
        {
            Id = newLoanApplication.Id,
        };
    }

    private static LoanApplicationDAO LoanApplicationDTOtoDAO(string userId, string idCardUrl, CreateLoanRequestBodyDTO loanRequest) => new()
    {
        IsPrivateField = loanRequest.IsPrivateField,
        ExpInYear = loanRequest.ExpInYear,
        ActiveFieldNumber = loanRequest.ActiveFieldNumber,
        SowSeedsPerCycle = loanRequest.SowSeedsPerCycle,
        NeededFertilizerPerCycleInKg = loanRequest.NeededFertilizerPerCycleInKg,
        EstimatedYieldInKg = loanRequest.EstimatedYieldInKg,
        EstimatedPriceOfHarvestPerKg = loanRequest.EstimatedPriceOfHarvestPerKg,
        HarvestCycleInMonths = loanRequest.HarvestCycleInMonths,
        LoanApplicationInIdr = loanRequest.LoanApplicationInIdr,
        BusinessIncomePerMonthInIdr = loanRequest.BusinessIncomePerMonthInIdr,
        BusinessOutcomePerMonthInIdr = loanRequest.BusinessOutcomePerMonthInIdr,
        UserId = userId,
        FullName = loanRequest.FullName,
        BirthDate = loanRequest.BirthDate,
        FullAddress = loanRequest.FullAddress,
        Phone = loanRequest.Phone,
        IdCardUrl = idCardUrl,
        OtherBusiness = loanRequest.OtherBusiness
    };

    public async Task<List<GetUserLoanResponseBodyDTO>> GetUserLoansAsync(string userId)
    {
        await CheckUserExistenceAndThrowAsync(userId);

        var userLoans = await _loanRepository.GetUserLoansAsync(userId);
        return userLoans
            .Select(userLoan => new GetUserLoanResponseBodyDTO
            {
                FullName = userLoan.FullName,
                LoanCreatedDate = userLoan.CreatedDate.ToString("2006-01-02"),
                LoanId = userLoan.Id,
                LoanStatus = userLoan.Status,
                UserId = userLoan.UserId,
            })
            .ToList();
    }

    private async Task CheckUserExistenceAndThrowAsync(string userId)
    {
        var user = await _userRepository.GetUserByUserIdAsync(userId);

        if (user == null)
        {
            throw new NotFoundException($"User with {userId} not found");
        }
    }

    public async Task<GetUserLoanDetailResponseBodyDTO> GetUserLoanDetailAsync(string loanId, string userId)
    {
        await CheckUserExistenceAndThrowAsync(userId);
        var userLoan = await GetUserLoanAsync(loanId, userId);

        return LoanApplicationDAOtoDTO(userLoan);
    }

    private async Task<LoanApplicationDAO> GetUserLoanAsync(string loanId, string userId)
    {
        var userLoan = await _loanRepository.GetUserLoanAsync(loanId, userId);
        if (userLoan == null)
        {
            throw new NotFoundException($"User id {userId} with loan id {loanId} not found");
        }

        return userLoan;
    }

    private static GetUserLoanDetailResponseBodyDTO LoanApplicationDAOtoDTO(LoanApplicationDAO loanApplication) => new()
    {
        IsPrivateField = loanApplication.IsPrivateField,
        ExpInYear = loanApplication.ExpInYear,
        ActiveFieldNumber = loanApplication.ActiveFieldNumber,
        SowSeedsPerCycle = loanApplication.SowSeedsPerCycle,
        NeededFertilizerPerCycleInKg = loanApplication.NeededFertilizerPerCycleInKg,
        EstimatedYieldInKg = loanApplication.EstimatedPriceOfHarvestPerKg,
        EstimatedPriceOfHarvestPerKg = loanApplication.EstimatedPriceOfHarvestPerKg,
        HarvestCycleInMonths = loanApplication.HarvestCycleInMonths,
        LoanApplicationInIdr = loanApplication.LoanApplicationInIdr,
        BusinessIncomePerMonthInIdr = loanApplication.BusinessIncomePerMonthInIdr,
        BusinessOutcomePerMonthInIdr = loanApplication.BusinessOutcomePerMonthInIdr,
        LoanId = loanApplication.Id,
        UserId = loanApplication.UserId,
        FullName = loanApplication.FullName,
        BirthDate = loanApplication.BirthDate,
        FullAddress = loanApplication.FullAddress,
        Phone = loanApplication.Phone,
        OtherBusiness = loanApplication.OtherBusiness,
        IdCardUrl = loanApplication.IdCardUrl,
        Status = loanApplication.Status,
    };
}