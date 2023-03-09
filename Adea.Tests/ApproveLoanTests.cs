using Xunit;
using Adea.User;
using Adea.Loan;
using Adea.Models;
using Adea.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Adea.Tests;

public class ApproveLoanTests : IClassFixture<DatabaseFixture>, IClassFixture<FileUploaderFixture>
{
    readonly DatabaseFixture _databaseFixture;
    readonly FileUploaderFixture _fileUploaderFixture;

    public ApproveLoanTests(DatabaseFixture databaseFixture, FileUploaderFixture fileUploaderFixture)
    {
        _databaseFixture = databaseFixture;
        _fileUploaderFixture = fileUploaderFixture;
    }

    [Fact]
    public async Task Approve_Loan_Success_Test()
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

        var newLoanStatus = new ApproveLoanParam(true);
        var proceededLoan = await service.ApproveLoanAsync(loanDAO.Id, newAdminID, newLoanStatus);
        Assert.Equal(loanDAO.Id, proceededLoan.Id);

        var loanApplication = await context.LoanApplications.Where(l => l.Id == proceededLoan.Id).FirstOrDefaultAsync();
        Assert.NotNull(loanApplication);
        Assert.Equal(loanApplication.Status, LoanStatus.Approve.ToString());
        Assert.Equal(loanApplication.OfficerId, newAdminID);

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Approve_Loan_But_Loan_Not_Exist_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var admin = new RegisterUser("admin", "password", true);
        var newAdminID = await userRepository.InsertUserAsync(admin);

        var newLoanStatus = new ApproveLoanParam(true);
        await Assert.ThrowsAsync<NotFoundException>(async () => await service.ApproveLoanAsync("some-random-loan-id", newAdminID, newLoanStatus));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Approve_Loan_With_Wait_Status_Fail_Test()
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

        var newLoanStatus = new ApproveLoanParam(true);
        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.ApproveLoanAsync(loanDAO.Id, newAdminID, newLoanStatus));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Approve_Loan_With_Reject_Status_Fail_Test()
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

        var newLoanStatus = new ApproveLoanParam(true);
        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.ApproveLoanAsync(loanDAO.Id, newAdminID, newLoanStatus));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Approve_Loan_With_Approve_Status_Fail_Test()
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

        var newLoanStatus = new ApproveLoanParam(true);
        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.ApproveLoanAsync(loanDAO.Id, newAdminID, newLoanStatus));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Reject_Loan_Success_Test()
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

        var newLoanStatus = new ApproveLoanParam(false);
        var proceededLoan = await service.ApproveLoanAsync(loanDAO.Id, newAdminID, newLoanStatus);
        Assert.Equal(loanDAO.Id, proceededLoan.Id);

        var loanApplication = await context.LoanApplications.Where(l => l.Id == proceededLoan.Id).FirstOrDefaultAsync();
        Assert.NotNull(loanApplication);
        Assert.Equal(loanApplication.Status, LoanStatus.Reject.ToString());
        Assert.Equal(loanApplication.OfficerId, newAdminID);

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Reject_Loan_But_Loan_Not_Exist_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var admin = new RegisterUser("admin", "password", true);
        var newAdminID = await userRepository.InsertUserAsync(admin);

        var newLoanStatus = new ApproveLoanParam(false);
        await Assert.ThrowsAsync<NotFoundException>(async () => await service.ApproveLoanAsync("some-random-loan-id", newAdminID, newLoanStatus));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Reject_Loan_With_Wait_Status_Fail_Test()
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

        var newLoanStatus = new ApproveLoanParam(false);
        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.ApproveLoanAsync(loanDAO.Id, newAdminID, newLoanStatus));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Reject_Loan_With_Reject_Status_Fail_Test()
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

        var newLoanStatus = new ApproveLoanParam(false);
        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.ApproveLoanAsync(loanDAO.Id, newAdminID, newLoanStatus));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Reject_Loan_With_Approve_Status_Fail_Test()
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

        var newLoanStatus = new ApproveLoanParam(false);
        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.ApproveLoanAsync(loanDAO.Id, newAdminID, newLoanStatus));

        await _databaseFixture.ClearDB(context);
    }
}
