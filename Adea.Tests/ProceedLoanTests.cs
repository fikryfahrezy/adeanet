using Xunit;
using Adea.User;
using Adea.Loan;
using Adea.Models;
using Adea.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Adea.Tests;

public class ProceedLoanTests : IClassFixture<DatabaseFixture>, IClassFixture<FileUploaderFixture>
{
    readonly DatabaseFixture _databaseFixture;
    readonly FileUploaderFixture _fileUploaderFixture;

    public ProceedLoanTests(DatabaseFixture databaseFixture, FileUploaderFixture fileUploaderFixture)
    {
        _databaseFixture = databaseFixture;
        _fileUploaderFixture = fileUploaderFixture;
    }

    [Fact]
    public async Task Proceed_Loan_Success_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var admin = new RegisterUser("admin", "password", true);
        var newAdminID = await userRepository.InsertUserAsync(admin);

        var member = new RegisterUser("username", "password", false);
        var newMemberID = await userRepository.InsertUserAsync(member);

        var loanDAO = new LoanApplicationDAO
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
            UserId = newMemberID,
            IdCardUrl = "http://random",
            Status = LoanStatus.Wait.ToString(),
        };

        context.LoanApplications.Add(loanDAO);
        await context.SaveChangesAsync();

        var proceededLoan = await service.ProceedLoanAsync(loanDAO.Id, newAdminID);
        Assert.Equal(loanDAO.Id, proceededLoan.Id);

        var loanApplication = await context.LoanApplications.Where(l => l.Id == proceededLoan.Id).FirstOrDefaultAsync();
        Assert.NotNull(loanApplication);
        Assert.Equal(loanApplication.Status, LoanStatus.Process.ToString());
        Assert.Equal(loanApplication.OfficerId, newAdminID);

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Proceed_Loan_But_Loan_Not_Exist_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var admin = new RegisterUser("admin", "password", true);
        var newAdminID = await userRepository.InsertUserAsync(admin);

        await Assert.ThrowsAsync<NotFoundException>(async () => await service.ProceedLoanAsync("some-random-loan-id", newAdminID));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Proceed_Loan_With_Process_Status_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var admin = new RegisterUser("admin", "password", true);
        var newAdminID = await userRepository.InsertUserAsync(admin);

        var member = new RegisterUser("username", "password", false);
        var newMemberID = await userRepository.InsertUserAsync(member);

        var loanDAO = new LoanApplicationDAO
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
            UserId = newMemberID,
            IdCardUrl = "http://random",
            Status = LoanStatus.Process.ToString(),
        };

        context.LoanApplications.Add(loanDAO);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.ProceedLoanAsync(loanDAO.Id, newAdminID));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Proceed_Loan_With_Reject_Status_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var admin = new RegisterUser("admin", "password", true);
        var newAdminID = await userRepository.InsertUserAsync(admin);

        var member = new RegisterUser("username", "password", false);
        var newMemberID = await userRepository.InsertUserAsync(member);

        var loanDAO = new LoanApplicationDAO
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
            UserId = newMemberID,
            IdCardUrl = "http://random",
            Status = LoanStatus.Reject.ToString(),
        };

        context.LoanApplications.Add(loanDAO);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.ProceedLoanAsync(loanDAO.Id, newAdminID));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Proceed_Loan_With_Approve_Status_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var admin = new RegisterUser("admin", "password", true);
        var newAdminID = await userRepository.InsertUserAsync(admin);

        var member = new RegisterUser("username", "password", false);
        var newMemberID = await userRepository.InsertUserAsync(member);

        var loanDAO = new LoanApplicationDAO
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
            UserId = newMemberID,
            IdCardUrl = "http://random",
            Status = LoanStatus.Approve.ToString(),
        };

        context.LoanApplications.Add(loanDAO);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.ProceedLoanAsync(loanDAO.Id, newAdminID));

        await _databaseFixture.ClearDB(context);
    }
}
