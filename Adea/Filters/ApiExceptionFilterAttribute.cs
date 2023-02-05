using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adea.Exceptions;

namespace Adea.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
	private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

	public ApiExceptionFilterAttribute()
	{
		// Register known exception types and handlers.
		_exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
			{
				{ typeof(NotFoundException), HandleNotFoundException },
				{ typeof(UnauthorizedException), HandleUnauthorizedException },
				{ typeof(UnprocessableEntityException), HandleUnprocessableEntityException },
				{ typeof(ValidationException), HandleValidationException },
			};
	}

	public override void OnException(ExceptionContext context)
	{
		HandleException(context);

		base.OnException(context);
	}

	private void HandleException(ExceptionContext context)
	{
		Type type = context.Exception.GetType();
		if (_exceptionHandlers.ContainsKey(type))
		{
			_exceptionHandlers[type].Invoke(context);
			return;
		}
	}



	private void HandleNotFoundException(ExceptionContext context)
	{
		var exception = (NotFoundException)context.Exception;

		var details = new ProblemDetails()
		{
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
			Title = "The specified resource was not found.",
			Detail = exception.Message
		};

		context.Result = new NotFoundObjectResult(details);

		context.ExceptionHandled = true;
	}

	private void HandleUnauthorizedException(ExceptionContext context)
	{
		var details = new ProblemDetails
		{
			Status = StatusCodes.Status401Unauthorized,
			Title = "Unauthorized",
			Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
		};

		context.Result = new ObjectResult(details)
		{
			StatusCode = StatusCodes.Status401Unauthorized
		};

		context.ExceptionHandled = true;
	}

	private void HandleUnprocessableEntityException(ExceptionContext context)
	{
		var details = new ProblemDetails
		{
			Status = StatusCodes.Status422UnprocessableEntity,
			Title = "Unprocessable Entity",
			Type = "https://tools.ietf.org/html/rfc4918.html#section-11.2"
		};

		context.Result = new ObjectResult(details)
		{
			StatusCode = StatusCodes.Status422UnprocessableEntity
		};

		context.ExceptionHandled = true;
	}

	private void HandleValidationException(ExceptionContext context)
	{
		var exception = (ValidationException)context.Exception;

		var errors = exception.Errors
					.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
					.ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

		var details = new ValidationProblemDetails(errors)
		{
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
		};

		context.Result = new BadRequestObjectResult(details);

		context.ExceptionHandled = true;
	}
}
