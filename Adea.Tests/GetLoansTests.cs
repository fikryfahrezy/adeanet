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

        var loans = await service.GetLoansAsync();
        Assert.NotEmpty(loans);

        await _fixture.ClearDB(context);
    }
}
