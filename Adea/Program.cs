using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Adea.Filters;
using Adea.Services.Data;
using Adea.Services.User;
using static Adea.Services.Common.JsonPropertyUtil;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LoanLosDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("LoanLosContextPSQL")));

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

// builder.Services.AddScoped<LoanRepository>();
// builder.Services.AddScoped<LoanService>();

builder.Services
	.AddControllers(options =>
	{
		options.Filters.Add<ApiExceptionFilterAttribute>();
	})
	.AddJsonOptions(option =>
	{
		option.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
	});

FluentValidation.ValidatorOptions.Global.PropertyNameResolver = (type, member, expression) =>
{
	if (member != null)
	{
		return GetJsonPropertyName(type, member.Name);
	}
	return null;
};

FluentValidation.ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) =>
{
	if (member != null)
	{
		return GetJsonPropertyName(type, member.Name);
	}
	return null;
};

builder.Services
	.AddFluentValidationAutoValidation()
	.AddFluentValidationClientsideAdapters();


builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
