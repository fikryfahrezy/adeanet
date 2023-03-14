using Adea.Interface;
using Adea.Models;
using Adea.User;
using Adea.Exceptions;
using Adea.Controllers;

namespace Adea.Loan;

public class LoanService
{
    private readonly LoanRepository _loanRepository;
    private readonly UserRepository _userRepository;
    private readonly IFileUploader _fileUploader;

    public LoanService(LoanRepository loanRepository, UserRepository userRepository, IFileUploader fileUploader)
    {
        _loanRepository = loanRepository;
        _fileUploader = fileUploader;
        _userRepository = userRepository;
    }

    private static GetLoanResponseBodyDTO LoanModeltoLoanDTO(Loan loan) => new()
    {
        FullName = loan.FullName,
        LoanCreatedDate = loan.LoanCreatedDate,
        LoanId = loan.LoanId,
        LoanStatus = loan.LoanStatus,
        UserId = loan.UserId,
    };

    private static GetLoanDetailResponseBodyDTO LoanDetailModeltoLoanDetailDTO(LoanDetail loanDetail) => new()
    {
        IsPrivateField = loanDetail.IsPrivateField,
        ExpInYear = loanDetail.ExpInYear,
        ActiveFieldNumber = loanDetail.ActiveFieldNumber,
        SowSeedsPerCycle = loanDetail.SowSeedsPerCycle,
        NeededFertilizerPerCycleInKg = loanDetail.NeededFertilizerPerCycleInKg,
        EstimatedYieldInKg = loanDetail.EstimatedYieldInKg,
        EstimatedPriceOfHarvestPerKg = loanDetail.EstimatedPriceOfHarvestPerKg,
        HarvestCycleInMonths = loanDetail.HarvestCycleInMonths,
        LoanApplicationInIdr = loanDetail.LoanApplicationInIdr,
        BusinessIncomePerMonthInIdr = loanDetail.BusinessIncomePerMonthInIdr,
        BusinessOutcomePerMonthInIdr = loanDetail.BusinessOutcomePerMonthInIdr,
        LoanId = loanDetail.LoanId,
        UserId = loanDetail.UserId,
        FullName = loanDetail.FullName,
        BirthDate = loanDetail.BirthDate,
        FullAddress = loanDetail.FullAddress,
        Phone = loanDetail.Phone,
        OtherBusiness = loanDetail.OtherBusiness,
        IdCardUrl = loanDetail.IdCardUrl,
        Status = loanDetail.Status,
    };

    private async Task<Member> CheckUserIfOfficerAsync(string userId)
    {
        var user = await _userRepository.GetUserByUserIdAsync(userId);

        if (user == null)
        {
            throw new NotFoundException($"User with id {userId} not exist");
        }

        if (!user.IsOfficer)
        {
            throw new UnauthorizedAccessException("officer only");
        }

        return user;
    }

    public async Task<List<GetLoanResponseBodyDTO>> GetUserLoansAsync(string userId)
    {
        var userLoans = await _loanRepository.GetUserLoansAsync(userId);
        return userLoans.Select(LoanModeltoLoanDTO).ToList();
    }

    public async Task<GetLoanDetailResponseBodyDTO> GetUserLoanDetailAsync(string loanId, string userId)
    {
        var userLoan = await _loanRepository.GetUserLoanAsync(loanId, userId);
        return LoanDetailModeltoLoanDetailDTO(userLoan);
    }

    public async Task<List<GetLoanResponseBodyDTO>> GetLoansAsync()
    {
        var loans = await _loanRepository.GetLoansAsync();
        return loans.Select(LoanModeltoLoanDTO).ToList();
    }

    public async Task<GetLoanDetailResponseBodyDTO> GetLoanDetailAsync(string loanId)
    {
        var loan = await _loanRepository.GetLoanAsync(loanId);
        return LoanDetailModeltoLoanDetailDTO(loan);
    }

    public async Task<CreateLoanResponseBodyDTO> CreateLoanAsync(string userId, CreateLoanParam loanApplication)
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
        var newLoanApplicationID = await _loanRepository.InsertLoanAsync(
            userId: userId,
            idCardUrl: idCardFilePath,
            loanApplication: loanApplication
        );

        return new CreateLoanResponseBodyDTO
        {
            Id = newLoanApplicationID,
        };
    }

    public async Task<CreateLoanResponseBodyDTO> UpdateLoanAsync(string loanId, string userId, CreateLoanParam loanApplication)
    {
        var userLoan = await _loanRepository.GetUserLoanAsync(loanId, userId);
        if (userLoan.Status != LoanStatus.Wait.ToString())
        {
            throw new UnprocessableEntityException("Cannot modify processed loan");
        }

        var idCardFilePath = await _fileUploader.UploadFileAsync(loanApplication.IdCard);
        var newLoanApplicationID = await _loanRepository.UpdateLoanAsync(
            loanId: loanId,
            userId: userId,
            idCardUrl: idCardFilePath,
            loanApplication: loanApplication
        );

        return new CreateLoanResponseBodyDTO
        {
            Id = newLoanApplicationID,
        };
    }

    public async Task<CreateLoanResponseBodyDTO> DeleteLoanAsync(string loanId, string userId)
    {
        var userLoan = await _loanRepository.GetUserLoanAsync(loanId, userId);
        if (userLoan.Status != LoanStatus.Wait.ToString())
        {
            throw new UnprocessableEntityException("Cannot modify processed loan");
        }

        var deletedLoanId = await _loanRepository.RemoveLoanAsync(loanId, userId);

        return new CreateLoanResponseBodyDTO
        {
            Id = deletedLoanId,
        };
    }

    public async Task<CreateLoanResponseBodyDTO> ProceedLoanAsync(string loanId, string userId)
    {
        await CheckUserIfOfficerAsync(userId);

        var userLoan = await _loanRepository.GetLoanAsync(loanId);
        if (userLoan.Status != LoanStatus.Wait.ToString())
        {
            throw new UnprocessableEntityException("Cannot modify processed loan");
        }

        var deletedLoanId = await _loanRepository.UpdateLoanStatusAsync(loanId, userId, LoanStatus.Process);

        return new CreateLoanResponseBodyDTO
        {
            Id = deletedLoanId,
        };
    }

    public async Task<CreateLoanResponseBodyDTO> ApproveLoanAsync(string loanId, string userId, ApproveLoanParam approveLoanParam)
    {
        await CheckUserIfOfficerAsync(userId);

        var userLoan = await _loanRepository.GetLoanAsync(loanId);
        if (userLoan.Status != LoanStatus.Process.ToString())
        {
            throw new UnprocessableEntityException("Cannot modify processed loan");
        }

        var newLoanStatus = approveLoanParam.IsApprove ? LoanStatus.Approve : LoanStatus.Reject;
        var deletedLoanId = await _loanRepository.UpdateLoanStatusAsync(loanId, userId, newLoanStatus);

        return new CreateLoanResponseBodyDTO
        {
            Id = deletedLoanId,
        };
    }
}