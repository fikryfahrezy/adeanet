using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Adea.Filters;

public class ValidationFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		if (!context.ModelState.IsValid)
		{
			var errors = context.ModelState
			.Where(e => e.Value != null && e.Value.Errors.Count > 0)
			.ToDictionary(kvp => kvp.Key.ToLower(), (kvp) => kvp.Value == null ? new string[0] : kvp.Value.Errors.Select(x => x.ErrorMessage).ToArray());

			var details = new ValidationProblemDetails(errors)
			{
				Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
			};

			context.Result = new BadRequestObjectResult(details);
			return;
		}

		await next();
	}
}