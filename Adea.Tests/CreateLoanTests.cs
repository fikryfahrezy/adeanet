using Xunit;
using Adea.User;
using Adea.Loan;
using Adea.Models;

namespace Adea.Tests;

public class CreateLoanTests : IClassFixture<DatabaseFixture>, IClassFixture<FileUploaderFixture>
{
    readonly DatabaseFixture _databaseFixture;
    readonly FileUploaderFixture _fileUploaderFixture;

    public CreateLoanTests(DatabaseFixture databaseFixture, FileUploaderFixture fileUploaderFixture)
    {
        _databaseFixture = databaseFixture;
        _fileUploaderFixture = fileUploaderFixture;
    }

    [Fact]
    public async Task Create_NewLoan_Test()
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

        var newLoan = new CreateLoanRequestBodyDTO
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
            IdCard = _fileUploaderFixture.fileMock,
        };

        var createdLoan = await service.CreateLoanAsync(user.Id, newLoan);
        Assert.NotEmpty(createdLoan.Id);

        await _databaseFixture.ClearDB(context);

    }
}
