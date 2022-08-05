using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Adea.Filters;
using Adea.Services.Loan;
using Adea.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LoanLosDbContext>(options => options.UseNpgsql("Server=127.0.0.1;Port=5432;Database=loan_los;User Id=postgres;Password=postgres;"));

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient<UserRepository>();
builder.Services.AddScoped<UserServices>();

builder.Services.AddTransient<LoanRepository>();
builder.Services.AddScoped<LoanService>();

builder.Services.AddControllers(options =>
{
	options.Filters.Add<ApiExceptionFilterAttribute>();
	options.Filters.Add<ValidationFilter>();
}).AddFluentValidation(x => x.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
