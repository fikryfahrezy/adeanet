using Xunit;
using Adea.User;
using Adea.Loan;
using Adea.Models;
using Adea.Exceptions;

namespace Adea.Tests;

public class GetUserLoansTests : IClassFixture<DatabaseFixture>, IClassFixture<FileUploaderFixture>
{
    readonly DatabaseFixture _databaseFixture;
    readonly FileUploaderFixture _fileUploaderFixture;

    public GetUserLoansTests(DatabaseFixture databaseFixture, FileUploaderFixture fileUploaderFixture)
    {
        _databaseFixture = databaseFixture;
        _fileUploaderFixture = fileUploaderFixture;
    }

    [Fact]
    public async Task Get_User_Loans_Success_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

        var loanApplication = new CreateLoanParam(
            isPrivateField: true,
            expInYear: 1,
            activeFieldNumber: 1,
            sowSeedsPerCycle: 1,
            neededFertilizerPerCycleInKg: 1,
            estimatedYieldInKg: 1,
            estimatedPriceOfHarvestPerKg: 1,
            harvestCycleInMonths: 1,
            loanApplicationInIdr: 1,
            businessIncomePerMonthInIdr: 1,
            businessOutcomePerMonthInIdr: 1,
            fullName: "Full Name",
            birthDate: "2006-01-02",
            fullAddress: "Full Address",
            phone: "0000000000",
            otherBusiness: "-",
            idCard: _fileUploaderFixture.fileMock
         );

        var newLoanId = await loanRepository.InsertLoanAsync(newUserID, _fileUploaderFixture.fileName, loanApplication);

        var userLoans = await service.GetUserLoansAsync(newUserID);
        Assert.Single(userLoans);

        foreach (var loan in userLoans)
        {
            Assert.NotNull(loan);
            Assert.Equal(loanApplication.FullName, loan.FullName);
            Assert.Equal(newLoanId, loan.LoanId);
            Assert.Equal(newUserID, loan.UserId);
            Assert.Equal(LoanStatus.Wait.ToString(), loan.LoanStatus);
            Assert.NotEmpty(loan.LoanCreatedDate);
        }

        await _databaseFixture.ClearDB(context);
    }


    [Fact]
    public async Task Get_User_Loans_But_User_Not_Found_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

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
            UserId = newUserID,
            IdCardUrl = "http://random",
        };

        var userLoans = await service.GetUserLoansAsync("some-random-user-id");
        Assert.Empty(userLoans);

        await _databaseFixture.ClearDB(context);
    }
}
