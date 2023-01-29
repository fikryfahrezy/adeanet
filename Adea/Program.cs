using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Adea.Filters;
using Adea.Data;
using Adea.User;
using Adea.Loan;
using Adea.Common;
using Adea.Options;
using static Adea.Common.RequestFieldMap;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<AppEnvOptions>()
	.Bind(builder.Configuration.GetSection(AppEnvOptions.AppEnv))
	.ValidateDataAnnotations()
	.ValidateOnStart();

builder.Services.AddDbContext<LoanLosDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("LoanDatabase")));

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddSingleton<FileUploader>();
builder.Services.AddScoped<LoanRepository>();
builder.Services.AddScoped<LoanService>();

builder.Services
	.AddControllers(options =>
	{
		options.Filters.Add<ApiExceptionFilterAttribute>();
	});

FluentValidation.ValidatorOptions.Global.PropertyNameResolver = (type, member, expression) =>
{
	if (member != null)
	{
		return GetPropertyName(type, member.Name);
	}
	return null;
};

FluentValidation.ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) =>
{
	if (member != null)
	{
		return GetPropertyName(type, member.Name);
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
