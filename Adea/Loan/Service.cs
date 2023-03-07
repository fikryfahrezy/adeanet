using Adea.Interface;
using Adea.Models;
using Adea.User;
using Adea.DTO;
using Adea.Exceptions;

namespace Adea.Loan;

public class LoanService
{
    private readonly LoanRepository _loanRepository;
    private readonly IFileUploader _fileUploader;

    public LoanService(LoanRepository loanRepository, IFileUploader fileUploader)
    {
        _loanRepository = loanRepository;
        _fileUploader = fileUploader;
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

    public async Task<CreateLoanResponseBodyDTO> UpdateLoanAsync(string userId, string loanId, LoanApplication loanApplication)
    {
        var userLoan = await _loanRepository.GetUserLoanAsync(loanId, userId);
        if (userLoan.Status != LoanStatus.Wait.ToString())
        {
            throw new UnprocessableEntityException("Already have processed loan");
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

    public async Task<CreateLoanResponseBodyDTO> DeleteLoanAsync(string userId, string loanId)
    {
        var userLoan = await _loanRepository.GetUserLoanAsync(loanId, userId);
        if (userLoan.Status != LoanStatus.Wait.ToString())
        {
            throw new UnprocessableEntityException("Already have processed laon");
        }

        var deletedLoanId = await _loanRepository.RemoveLoanAsync(loanId, userId);

        return new CreateLoanResponseBodyDTO
        {
            Id = deletedLoanId,
        };
    }
}