using Adea.Interface;
using Adea.Models;
using Adea.User;
using Adea.DTO;
using Adea.Exceptions;

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

    public async Task<CreateLoanResponseBodyDTO> CreateLoanAsync(string userId, LoanApplication loanApplication)
    {
        var userLoans = await _loanRepository.GetUserLoansAsync(userId);
        foreach (var userLoan in userLoans)
        {
            if (userLoan.LoanStatus == LoanStatus.Wait.ToString() || userLoan.LoanStatus == LoanStatus.Process.ToString())
            {
                throw new UnprocessableEntityException("Already have processed loan");
            }
        }

        var idCardFilePath = await _fileUploader.UploadFileAsync(loanApplication.IdCard);
        var newLoanApplicationID = await _loanRepository.InsertLoanAsync(userId, idCardFilePath, loanApplication);

        return new CreateLoanResponseBodyDTO
        {
            Id = newLoanApplicationID,
        };
    }

    public async Task<List<GetLoanResponseBodyDTO>> GetUserLoansAsync(string userId)
    {
        await CheckUserExistenceAndThrowAsync(userId);

        var userLoans = await _loanRepository.GetUserLoansAsync(userId);
        return userLoans.Select(LoanModeltoLoanDTO).ToList();
    }

    public async Task<GetLoanDetailResponseBodyDTO> GetUserLoanDetailAsync(string loanId, string userId)
    {
        await CheckUserExistenceAndThrowAsync(userId);
        var userLoan = await _loanRepository.GetUserLoanAsync(loanId, userId);
        if (userLoan is null)
        {
            throw new NotFoundException($"User id {userId} with loan id {loanId} not found");
        }

        return LoanModeltoLoanDetailDTO(userLoan);
    }

    public async Task<List<GetLoanResponseBodyDTO>> GetLoansAsync()
    {
        var loans = await _loanRepository.GetLoansAsync();
        return loans.Select(LoanModeltoLoanDTO).ToList();
    }

    public async Task<GetLoanDetailResponseBodyDTO> GetLoanDetailAsync(string loanId)
    {
        var loan = await _loanRepository.GetLoanAsync(loanId);

        if (loan is null)
        {
            throw new NotFoundException($"Loan with {loanId} not found");
        }
        return LoanModeltoLoanDetailDTO(loan);
    }

    private async Task CheckUserExistenceAndThrowAsync(string userId)
    {
        var user = await _userRepository.GetUserByUserIdAsync(userId);

        if (user is null)
        {
            throw new NotFoundException($"User with {userId} not found");
        }
    }

    private static GetLoanResponseBodyDTO LoanModeltoLoanDTO(Loan loan) => new()
    {
        FullName = loan.FullName,
        LoanCreatedDate = loan.LoanCreatedDate,
        LoanId = loan.LoanId,
        LoanStatus = loan.LoanStatus,
        UserId = loan.UserId,
    };

    private static GetLoanDetailResponseBodyDTO LoanModeltoLoanDetailDTO(LoanApplicationDAO loanApplication) => new()
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