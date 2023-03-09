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
    public async void Get_Loans_Success_Test()
    {
        using var context = _fixture.CreateContext();

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

        var loans = await service.GetLoansAsync();
        Assert.NotEmpty(loans);

        foreach (var loan in loans)
        {
            Assert.NotNull(loan);
            Assert.Equal(loanApplication.FullName, loan.FullName);
            Assert.Equal(newLoanId, loan.LoanId);
            Assert.Equal(newUserID, loan.UserId);
            Assert.Equal(LoanStatus.Wait.ToString(), loan.LoanStatus);
            Assert.NotEmpty(loan.LoanCreatedDate);
        }

        await _fixture.ClearDB(context);
    }
}
