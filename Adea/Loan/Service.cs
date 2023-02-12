using Adea.Interface;
using Adea.Models;
using Adea.User;
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

    public LoanApplicationDAO LoanApplicationDTOtoDAO(string userId, string idCardUrl, CreateLoanRequestBodyDTO loanRequest) => new LoanApplicationDAO
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
        OtherBusiness = loanRequest.OtherBusiness,
    };
}