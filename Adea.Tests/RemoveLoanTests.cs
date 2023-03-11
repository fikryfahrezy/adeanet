using Xunit;
using Adea.User;
using Adea.Loan;
using Adea.Models;
using Adea.Exceptions;

namespace Adea.Tests;

public class RemoveLoanTests : IClassFixture<DatabaseFixture>, IClassFixture<FileUploaderFixture>
{
    readonly DatabaseFixture _databaseFixture;
    readonly FileUploaderFixture _fileUploaderFixture;

    public RemoveLoanTests(DatabaseFixture databaseFixture, FileUploaderFixture fileUploaderFixture)
    {
        _databaseFixture = databaseFixture;
        _fileUploaderFixture = fileUploaderFixture;
    }

    [Fact]
    public async Task Delete_Loan_With_Process_Status_Wait_Success_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

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
            UserId = newUserID,
            IdCardUrl = "http://random",
            Status = LoanStatus.Wait.ToString(),
        };

        context.LoanApplications.Add(loanDAO);
        await context.SaveChangesAsync();

        var deletedLoan = await service.DeleteLoanAsync(loanDAO.Id, newUserID);
        Assert.Equal(loanDAO.Id, deletedLoan.Id);

        await Assert.ThrowsAsync<NotFoundException>(async () => await loanRepository.GetLoanAsync(loanDAO.Id));
        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Delete_Loan_But_User_Not_Exist_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

        var newLoan = new CreateLoanParam(
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

        var createdLoanID = await loanRepository.InsertLoanAsync(newUserID, "https://random", newLoan);
        await Assert.ThrowsAsync<NotFoundException>(async () => await service.DeleteLoanAsync(createdLoanID, "some-random-user-id"));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Delete_Loan_But_Loan_Not_Exist_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

        var newLoan = new CreateLoanParam(
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

        var createdLoanID = await loanRepository.InsertLoanAsync(newUserID, "https://random", newLoan);
        await Assert.ThrowsAsync<NotFoundException>(async () => await service.DeleteLoanAsync("some-random-loan-id", newUserID));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Delete_Loan_But_Loan_Not_Belong_To_User_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

        var user2 = new RegisterUser("username2", "password", true);
        var newUser2ID = await userRepository.InsertUserAsync(user2);

        var newLoan = new CreateLoanParam(
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

        var createdLoanID = await loanRepository.InsertLoanAsync(newUserID, "https://random", newLoan);
        await Assert.ThrowsAsync<NotFoundException>(async () => await service.DeleteLoanAsync(createdLoanID, newUser2ID));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Delete_Loan_With_Process_Status_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

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
            UserId = newUserID,
            IdCardUrl = "http://random",
            Status = LoanStatus.Process.ToString(),
        };

        context.LoanApplications.Add(loanDAO);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.DeleteLoanAsync(loanDAO.Id, newUserID));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Delete_Loan_With_Process_Status_Reject_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

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
            UserId = newUserID,
            IdCardUrl = "http://random",
            Status = LoanStatus.Reject.ToString(),
        };

        context.LoanApplications.Add(loanDAO);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.DeleteLoanAsync(loanDAO.Id, newUserID));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Delete_Loan_With_Process_Status_Approve_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

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
            UserId = newUserID,
            IdCardUrl = "http://random",
            Status = LoanStatus.Approve.ToString(),
        };

        context.LoanApplications.Add(loanDAO);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.DeleteLoanAsync(loanDAO.Id, newUserID));

        await _databaseFixture.ClearDB(context);
    }
}
