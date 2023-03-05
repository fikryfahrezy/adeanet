using Xunit;
using Adea.User;
using Adea.Loan;
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
        var loanDetail = await service.GetLoanDetailAsync(newLoanID);

        Assert.Equal(newLoanID, loanDetail.LoanId);
        Assert.Equal(loanApplication.IsPrivateField, loanDetail.IsPrivateField);
        Assert.Equal(loanApplication.ExpInYear, loanDetail.ExpInYear);
        Assert.Equal(loanApplication.ActiveFieldNumber, loanDetail.ActiveFieldNumber);
        Assert.Equal(loanApplication.SowSeedsPerCycle, loanDetail.SowSeedsPerCycle);
        Assert.Equal(loanApplication.NeededFertilizerPerCycleInKg, loanDetail.NeededFertilizerPerCycleInKg);
        Assert.Equal(loanApplication.EstimatedYieldInKg, loanDetail.EstimatedYieldInKg);
        Assert.Equal(loanApplication.EstimatedPriceOfHarvestPerKg, loanDetail.EstimatedPriceOfHarvestPerKg);
        Assert.Equal(loanApplication.HarvestCycleInMonths, loanDetail.HarvestCycleInMonths);
        Assert.Equal(loanApplication.LoanApplicationInIdr, loanDetail.LoanApplicationInIdr);
        Assert.Equal(loanApplication.BusinessIncomePerMonthInIdr, loanDetail.BusinessIncomePerMonthInIdr);
        Assert.Equal(loanApplication.BusinessOutcomePerMonthInIdr, loanDetail.BusinessOutcomePerMonthInIdr);
        Assert.Equal(loanApplication.FullName, loanDetail.FullName);
        Assert.Equal(loanApplication.BirthDate, loanDetail.BirthDate);
        Assert.Equal(loanApplication.FullAddress, loanDetail.FullAddress);
        Assert.Equal(loanApplication.Phone, loanDetail.Phone);
        Assert.Equal(loanApplication.OtherBusiness, loanDetail.OtherBusiness);
        Assert.Equal(_fileUploaderFixture.fileName, loanDetail.IdCardUrl);

        await _databaseFixture.ClearDB(context);
    }


    [Fact]
    public async Task Get_Loan_Not_Found_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetLoanDetailAsync("some-random-loan-id"));

        await _databaseFixture.ClearDB(context);
    }
}
