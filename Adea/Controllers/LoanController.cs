using Microsoft.AspNetCore.Mvc;
using Adea.Services.Loan;

namespace Adea.Controllers;

[Route("api/[controller]")]
public class LoansController : ControllerBase
{
	private readonly LoanService _service;

	public LoansController(LoanService service)
	{
		_service = service;
	}
}