using Xunit;
using Adea.User;
using Adea.Loan;
using Adea.Models;
using Adea.Exceptions;

namespace Adea.Tests;

public class UpdateLoanTests : IClassFixture<DatabaseFixture>, IClassFixture<FileUploaderFixture>
{
    readonly DatabaseFixture _databaseFixture;
    readonly FileUploaderFixture _fileUploaderFixture;

    public UpdateLoanTests(DatabaseFixture databaseFixture, FileUploaderFixture fileUploaderFixture)
    {
        _databaseFixture = databaseFixture;
        _fileUploaderFixture = fileUploaderFixture;
    }

    [Fact]
    public async Task Update_Loan_But_User_Not_Exist_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

        var newLoan = new LoanApplication(
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
        await Assert.ThrowsAsync<NotFoundException>(async () => await service.UpdateLoanAsync("some-random-user-id", createdLoanID, newLoan));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Update_Loan_But_Loan_Not_Exist_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

        var newLoan = new LoanApplication(
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
        await Assert.ThrowsAsync<NotFoundException>(async () => await service.UpdateLoanAsync(newUserID, "some-random-loan-id", newLoan));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Update_Loan_But_Loan_Not_Belong_To_User_Fail_Test()
    {
        using var context = _databaseFixture.CreateContext();

        var loanRepository = new LoanRepository(context);
        var userRepository = new UserRepository(context);
        var service = new LoanService(loanRepository, userRepository, _fileUploaderFixture);

        var user = new RegisterUser("username", "password", true);
        var newUserID = await userRepository.InsertUserAsync(user);

        var user2 = new RegisterUser("username2", "password", true);
        var newUser2ID = await userRepository.InsertUserAsync(user2);

        var newLoan = new LoanApplication(
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
        await Assert.ThrowsAsync<NotFoundException>(async () => await service.UpdateLoanAsync(newUser2ID, createdLoanID, newLoan));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Update_Loan_With_Process_Status_Fail_Test()
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

        var newLoan = new LoanApplication(
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

        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.UpdateLoanAsync(newUserID, loanDAO.Id, newLoan));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Update_Loan_With_Process_Status_Reject_Test()
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

        var newLoan = new LoanApplication(
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

        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.UpdateLoanAsync(newUserID, loanDAO.Id, newLoan));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Update_Loan_With_Process_Status_Approve_Test()
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

        var newLoan = new LoanApplication(
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

        await Assert.ThrowsAsync<UnprocessableEntityException>(async () => await service.UpdateLoanAsync(newUserID, loanDAO.Id, newLoan));

        await _databaseFixture.ClearDB(context);
    }

    [Fact]
    public async Task Update_Loan_With_Process_Status_Wait_Success_Test()
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

        var newLoan = new LoanApplication(
            isPrivateField: false,
            expInYear: 2,
            activeFieldNumber: 2,
            sowSeedsPerCycle: 2,
            neededFertilizerPerCycleInKg: 2,
            estimatedYieldInKg: 2,
            estimatedPriceOfHarvestPerKg: 2,
            harvestCycleInMonths: 2,
            loanApplicationInIdr: 2,
            businessIncomePerMonthInIdr: 2,
            businessOutcomePerMonthInIdr: 2,
            fullName: "New Full Name",
            birthDate: "2006-01-03",
            fullAddress: "New Full Address",
            phone: "0000000001",
            otherBusiness: "New Business",
            idCard: _fileUploaderFixture.fileMock
        );

        var updatedLoan = await service.UpdateLoanAsync(newUserID, loanDAO.Id, newLoan);
        Assert.Equal(loanDAO.Id, updatedLoan.Id);

        var newLoanDetail = await loanRepository.GetLoanAsync(updatedLoan.Id);
        Assert.Equal(newLoan.IsPrivateField, newLoanDetail.IsPrivateField);
        Assert.Equal(newLoan.ExpInYear, newLoanDetail.ExpInYear);
        Assert.Equal(newLoan.ActiveFieldNumber, newLoanDetail.ActiveFieldNumber);
        Assert.Equal(newLoan.SowSeedsPerCycle, newLoanDetail.SowSeedsPerCycle);
        Assert.Equal(newLoan.NeededFertilizerPerCycleInKg, newLoanDetail.NeededFertilizerPerCycleInKg);
        Assert.Equal(newLoan.EstimatedYieldInKg, newLoanDetail.EstimatedYieldInKg);
        Assert.Equal(newLoan.EstimatedPriceOfHarvestPerKg, newLoanDetail.EstimatedPriceOfHarvestPerKg);
        Assert.Equal(newLoan.HarvestCycleInMonths, newLoanDetail.HarvestCycleInMonths);
        Assert.Equal(newLoan.LoanApplicationInIdr, newLoanDetail.LoanApplicationInIdr);
        Assert.Equal(newLoan.BusinessIncomePerMonthInIdr, newLoanDetail.BusinessIncomePerMonthInIdr);
        Assert.Equal(newLoan.BusinessOutcomePerMonthInIdr, newLoanDetail.BusinessOutcomePerMonthInIdr);
        Assert.Equal(newLoan.FullName, newLoanDetail.FullName);
        Assert.Equal(newLoan.BirthDate, newLoanDetail.BirthDate);
        Assert.Equal(newLoan.FullAddress, newLoanDetail.FullAddress);
        Assert.Equal(newLoan.Phone, newLoanDetail.Phone);
        Assert.Equal(newLoan.OtherBusiness, newLoanDetail.OtherBusiness);
        Assert.Equal(_fileUploaderFixture.fileName, newLoanDetail.IdCardUrl);

        await _databaseFixture.ClearDB(context);
    }
}
