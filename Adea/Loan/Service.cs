using FluentValidation;
using Adea.Common;

namespace Adea.Loan;

public class LoanService
{
	private readonly LoanRepository _repository;
	private readonly FileUploader _fileUploader;

	public LoanService(LoanRepository repository, FileUploader fileUploader)
	{
		_repository = repository;
		_fileUploader = fileUploader;
	}

	public async Task<string> CreateLoanAsync(CreatLoanRequestBodyDTO loanRequest)
	{
		var validator = new CreatLoanRequestBodyDTOValidator();
		await validator.ValidateAndThrowAsync(loanRequest);

		var idCardFilePath = "";

		if (loanRequest.IdCard is not null)
		{
			idCardFilePath = await _fileUploader.UploadFileAsync(loanRequest.IdCard);
		}

		return idCardFilePath;
	}
}