using Adea.Loan;
using Adea.Models;
using Adea.User;
using Xunit;

namespace Adea.Tests;
public class GetLoansTests : IClassFixture<DatabaseFixture>, IClassFixture<FileUploaderFixture>
{
    readonly DatabaseFixture _fixture;
    readonly FileUploaderFixture _fileUploaderFixture;

    public GetLoansTests(DatabaseFixture fixture, FileUploaderFixture fileUploaderFixture)
    {
        _fixture = fixture;
        _fileUploaderFixture = fileUploaderFixture;
    }

    [Fact]
    public async void Get_Loans_Tests()
    {
        using var context = _fixture.CreateContext();

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

        await loanRepository.InsertLoanAsync(newUserID, "https://random", loanApplication);

        var loans = await service.GetLoansAsync();
        Assert.NotEmpty(loans);

        await _fixture.ClearDB(context);
    }
}
