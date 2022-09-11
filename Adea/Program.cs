using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Adea.Filters;
using Adea.Data;
using Adea.Services.Loan;
using Adea.Services.UserService;

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
	.AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
