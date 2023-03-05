using Xunit;
using Adea.User;
using Adea.Loan;
using Adea.Models;
using Adea.Exceptions;

namespace Adea.Tests;

public class GetUserLoanDetailTests : IClassFixture<DatabaseFixture>, IClassFixture<FileUploaderFixture>
{
    readonly DatabaseFixture _databaseFixture;
    readonly FileUploaderFixture _fileUploaderFixture;

    public GetUserLoanDetailTests(DatabaseFixture databaseFixture, FileUploaderFixture fileUploaderFixture)
    {
        _databaseFixture = databaseFixture;
        _fileUploaderFixture = fileUploaderFixture;
    }

    [Fact]
    public async Task Get_User_Loan_Success_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

        var loanApplication = new LoanApplication(
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

        var newLoanID = await loanRepository.InsertLoanAsync(newUserID, _fileUploaderFixture.fileName, loanApplication);
        var userLoanDetail = await service.GetUserLoanDetailAsync(newLoanID, newUserID);

        Assert.Equal(newLoanID, userLoanDetail.LoanId);
        Assert.Equal(loanApplication.IsPrivateField, userLoanDetail.IsPrivateField);
        Assert.Equal(loanApplication.ExpInYear, userLoanDetail.ExpInYear);
        Assert.Equal(loanApplication.ActiveFieldNumber, userLoanDetail.ActiveFieldNumber);
        Assert.Equal(loanApplication.SowSeedsPerCycle, userLoanDetail.SowSeedsPerCycle);
        Assert.Equal(loanApplication.NeededFertilizerPerCycleInKg, userLoanDetail.NeededFertilizerPerCycleInKg);
        Assert.Equal(loanApplication.EstimatedYieldInKg, userLoanDetail.EstimatedYieldInKg);
        Assert.Equal(loanApplication.EstimatedPriceOfHarvestPerKg, userLoanDetail.EstimatedPriceOfHarvestPerKg);
        Assert.Equal(loanApplication.HarvestCycleInMonths, userLoanDetail.HarvestCycleInMonths);
        Assert.Equal(loanApplication.LoanApplicationInIdr, userLoanDetail.LoanApplicationInIdr);
        Assert.Equal(loanApplication.BusinessIncomePerMonthInIdr, userLoanDetail.BusinessIncomePerMonthInIdr);
        Assert.Equal(loanApplication.BusinessOutcomePerMonthInIdr, userLoanDetail.BusinessOutcomePerMonthInIdr);
        Assert.Equal(loanApplication.FullName, userLoanDetail.FullName);
        Assert.Equal(loanApplication.BirthDate, userLoanDetail.BirthDate);
        Assert.Equal(loanApplication.FullAddress, userLoanDetail.FullAddress);
        Assert.Equal(loanApplication.Phone, userLoanDetail.Phone);
        Assert.Equal(loanApplication.OtherBusiness, userLoanDetail.OtherBusiness);
        Assert.Equal(_fileUploaderFixture.fileName, userLoanDetail.IdCardUrl);

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Get_User_Loan_Not_Belong_To_User_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

        var user2 = new RegisterUser("username2", "password", true);
        var newUser2ID = await userRepository.InsertUserAsync(user2);

        var loanApplication = new LoanApplication(
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

        var newLoanID = await loanRepository.InsertLoanAsync(newUserID, "https://random", loanApplication);

        await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetUserLoanDetailAsync(newLoanID, newUser2ID));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Get_User_Loan_Not_Found_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

        await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetUserLoanDetailAsync("some-random-loan-id", newUserID));

        await _databaseFixture.ClearDB(context);
    }
}
