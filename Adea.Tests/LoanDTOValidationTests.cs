﻿using Adea.Controllers;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Adea.Tests;

public class LoanDTOValidationTests : IClassFixture<FileUploaderFixture>
{
    readonly static IFormFile? _fileMock = null;

    static LoanDTOValidationTests()
    {
        //Setup mock file using a memory stream
        // Ref: https://stackoverflow.com/questions/36858542/how-to-mock-an-iformfile-for-a-unit-integration-test-in-asp-net-core
        var content = "Hello World from a Fake File";
        var fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        //create FormFile with desired data
        _fileMock = new FormFile(stream, 0, stream.Length, "id_card", fileName);
    }

    public static IEnumerable<object[]> CreateLoanValidationCases
    => new object[][] {
			// Create loan fail, exp in year is 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 0,
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
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, exp in year is < 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = -1,
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
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, active field number is 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 0,
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
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, active field number is < 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = -1,
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
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, sow seed per cycle is 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 0,
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
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, sow seed per cycle is < 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = -1,
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
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, needed fert per cycle in kg is 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = 0,
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
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, needed fert per cycle in kg is < 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = -1,
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
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, estimated yield is 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = 1,
                    EstimatedYieldInKg = 0,
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
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, estimated yield is < 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = 1,
                    EstimatedYieldInKg = -1,
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
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, estimated price harvest is 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = 1,
                    EstimatedYieldInKg = 1,
                    EstimatedPriceOfHarvestPerKg = 0,
                    HarvestCycleInMonths = 1,
                    LoanApplicationInIdr = 1,
                    BusinessIncomePerMonthInIdr = 1,
                    BusinessOutcomePerMonthInIdr = 1,
                    FullName = "Full Name",
                    BirthDate = "2006-01-02",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, estimated price harvest is < 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = 1,
                    EstimatedYieldInKg = 1,
                    EstimatedPriceOfHarvestPerKg = -1,
                    HarvestCycleInMonths = 1,
                    LoanApplicationInIdr = 1,
                    BusinessIncomePerMonthInIdr = 1,
                    BusinessOutcomePerMonthInIdr = 1,
                    FullName = "Full Name",
                    BirthDate = "2006-01-02",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, harvest cycle in months is 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = 1,
                    EstimatedYieldInKg = 1,
                    EstimatedPriceOfHarvestPerKg = 1,
                    HarvestCycleInMonths = 0,
                    LoanApplicationInIdr = 1,
                    BusinessIncomePerMonthInIdr = 1,
                    BusinessOutcomePerMonthInIdr = 1,
                    FullName = "Full Name",
                    BirthDate = "2006-01-02",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, harvest cycle in months is < 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = 1,
                    EstimatedYieldInKg = 1,
                    EstimatedPriceOfHarvestPerKg = 1,
                    HarvestCycleInMonths = -1,
                    LoanApplicationInIdr = 1,
                    BusinessIncomePerMonthInIdr = 1,
                    BusinessOutcomePerMonthInIdr = 1,
                    FullName = "Full Name",
                    BirthDate = "2006-01-02",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, loan idr is 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = 1,
                    EstimatedYieldInKg = 1,
                    EstimatedPriceOfHarvestPerKg = 1,
                    HarvestCycleInMonths = 1,
                    LoanApplicationInIdr = 0,
                    BusinessIncomePerMonthInIdr = 1,
                    BusinessOutcomePerMonthInIdr = 1,
                    FullName = "Full Name",
                    BirthDate = "2006-01-02",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, loan idr is < 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = 1,
                    EstimatedYieldInKg = 1,
                    EstimatedPriceOfHarvestPerKg = 1,
                    HarvestCycleInMonths = 1,
                    LoanApplicationInIdr = -1,
                    BusinessIncomePerMonthInIdr = 1,
                    BusinessOutcomePerMonthInIdr = 1,
                    FullName = "Full Name",
                    BirthDate = "2006-01-02",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, income idr is 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = 1,
                    EstimatedYieldInKg = 1,
                    EstimatedPriceOfHarvestPerKg = 1,
                    HarvestCycleInMonths = 1,
                    LoanApplicationInIdr = 1,
                    BusinessIncomePerMonthInIdr = 0,
                    BusinessOutcomePerMonthInIdr = 1,
                    FullName = "Full Name",
                    BirthDate = "2006-01-02",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, income idr is < 0
			new object[] {
                new CreateLoanRequestBodyDTO {
                    IsPrivateField = true,
                    ExpInYear = 1,
                    ActiveFieldNumber = 1,
                    SowSeedsPerCycle = 1,
                    NeededFertilizerPerCycleInKg = 1,
                    EstimatedYieldInKg = 1,
                    EstimatedPriceOfHarvestPerKg = 1,
                    HarvestCycleInMonths = 1,
                    LoanApplicationInIdr = 1,
                    BusinessIncomePerMonthInIdr = -1,
                    BusinessOutcomePerMonthInIdr = 1,
                    FullName = "Full Name",
                    BirthDate = "2006-01-02",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, outcome idr is 0
			new object[] {
                new CreateLoanRequestBodyDTO {
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
                    BusinessOutcomePerMonthInIdr = 0,
                    FullName = "Full Name",
                    BirthDate = "2006-01-02",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, outcome idr is < 0
			new object[] {
                new CreateLoanRequestBodyDTO {
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
                    BusinessOutcomePerMonthInIdr = -1,
                    FullName = "Full Name",
                    BirthDate = "2006-01-02",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, empty full name
			new object[] {
                new CreateLoanRequestBodyDTO {
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
                    FullName = "",
                    BirthDate = "2006-01-02",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, empty birth date
			new object[] {
                new CreateLoanRequestBodyDTO {
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
                    BirthDate = "",
                    FullAddress = "Full Address",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, empty full address
			new object[] {
                new CreateLoanRequestBodyDTO {
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
                    FullAddress = "",
                    Phone = "0000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, empty phone
			new object[] {
                new CreateLoanRequestBodyDTO {
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
                    Phone = "",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, phone not match min length
			new object[] {
                new CreateLoanRequestBodyDTO {
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
                    Phone = "000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, phone not match min length
			new object[] {
                new CreateLoanRequestBodyDTO {
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
                    Phone = "0000000000000000000000",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, phone not only numbers
			new object[] {
                new CreateLoanRequestBodyDTO {
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
                    Phone = "0000000000xx",
                    OtherBusiness = "-",
                    IdCard = _fileMock,
                },
            },
			// Create loan fail, id card file required
			new object[] {
                new CreateLoanRequestBodyDTO {
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
                    IdCard = null,
                },
            },
    };

    [Theory]
    [MemberData(nameof(CreateLoanValidationCases))]
    public async Task Create_Loan_Request_Body_DTO_Validation_Fail_Test(CreateLoanRequestBodyDTO request)
    {
        var validator = new CreateLoanRequestBodyDTOValidator();
        await Assert.ThrowsAsync<ValidationException>(async () => await validator.ValidateAndThrowAsync(request));
    }

    [Fact]
    public async Task Create_Loan_Request_Body_DTO_Validation_Success_Test()
    {
        var requestBody = new CreateLoanRequestBodyDTO
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
            IdCard = _fileMock,
        };
        var validator = new CreateLoanRequestBodyDTOValidator();
        var result = await validator.TestValidateAsync(requestBody);
        Assert.True(result.IsValid);
    }

    public static IEnumerable<object[]> ApproveLoanValidationCases
    => new object[][] {
            new object[] {
                new ApproveLoanRequestBodyDTO {
                    IsApprove = true,
                },
            },
            new object[] {
                new ApproveLoanRequestBodyDTO {
                    IsApprove = false,
                },
            },
    };

    [Theory]
    [MemberData(nameof(ApproveLoanValidationCases))]
    public async Task Approve_Loan_Request_Body_DTO_Validation_Success_Test(ApproveLoanRequestBodyDTO requestBody)
    {
        var validator = new ApproveLoanRequestBodyDTOValidator();
        var result = await validator.TestValidateAsync(requestBody);
        Assert.True(result.IsValid);
    }
}

