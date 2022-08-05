using Microsoft.AspNetCore.Mvc;

namespace Adea.Services.Loan;

[Route("api/[controller]")]
public class LoansController : ControllerBase
{
	private readonly LoanService _service;

	public LoansController(LoanService service)
	{
		_service = service;
	}
}