using Xunit;
using Adea.User;
using Adea.Loan;
using Adea.Models;
using Adea.Exceptions;

namespace Adea.Tests;

public class GetLoanDetailTests : IClassFixture<DatabaseFixture>, IClassFixture<FileUploaderFixture>
{
    readonly DatabaseFixture _databaseFixture;
    readonly FileUploaderFixture _fileUploaderFixture;

    public GetLoanDetailTests(DatabaseFixture databaseFixture, FileUploaderFixture fileUploaderFixture)
    {
        _databaseFixture = databaseFixture;
        _fileUploaderFixture = fileUploaderFixture;
    }

    [Fact]
    public async Task Get_Loan_Success_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new UserDAO
        {
            Username = "username",
            Password = "password",
            IsOfficer = true,
        };

        await userRepository.InsertUserAsync(user);

        var newLoan = new LoanApplicationDAO
        {
            IsPrivateField = true,
            ExpInYear = 1,
            ActiveFieldNumber = 1,
            SowSeedsPerCycle = 1,
            NeededFertilizerPerCycleInKg = 1,
            EstimatedYieldInKg = 1,
            EstimatedPriceOfHarvestPerKg = 1,
            HarvestCycleInMonths = 1,
            LoanApplicationInIdr = 1,
            BusinessIncomePerMonthInIdr = 1,
            BusinessOutcomePerMonthInIdr = 1,
            FullName = "Full Name",
            BirthDate = "2006-01-02",
            FullAddress = "Full Address",
            Phone = "0000000000",
            OtherBusiness = "-",
            UserId = user.Id,
            IdCardUrl = "http://random",
        };

        await loanRepository.InsertLoanAsync(newLoan);

        var loanDetail = await service.GetLoanDetailAsync(newLoan.Id);
        Assert.Equal(newLoan.Id, loanDetail.LoanId);

        await _databaseFixture.ClearDB(context);
    }


    [Fact]
    public async Task Get_Loan_Fail_Loan_NotFound_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetLoanDetailAsync("some-random-loan-id"));

        await _databaseFixture.ClearDB(context);
    }
}
