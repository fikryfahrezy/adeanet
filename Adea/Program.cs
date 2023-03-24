using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Adea.Filters;
using Adea.Data;
using Adea.User;
using Adea.Loan;
using Adea.Common;
using Adea.Options;
using Adea.Interface;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using static Adea.Common.RequestFieldMap;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<AppEnvOptions>()
    .Bind(builder.Configuration.GetSection(AppEnvOptions.AppEnv))
    .ValidateDataAnnotations()
    .ValidateOnStart();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.GetSection(AppEnvOptions.AppEnv).Get<AppEnvOptions>()!;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidAudiences = jwtOptions.JwtValidAudiences,
        ValidIssuer = jwtOptions.JwtValidIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.JwtIssuerSigningKey)
        )
    };
});

builder.Services.AddDbContext<LoanLosDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("LoanDatabase")));
builder.Services.AddHealthChecks().AddDbContextCheck<LoanLosDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddSingleton<IFileUploader, FileUploader>();
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


// Migrate latest database changes during startup
// Not recommended way, but for dev purpose is file
// https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#apply-migrations-at-runtime
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LoanLosDbContext>();
    dbContext.Database.Migrate();
}

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/healthz");
app.MapControllers();
app.Run();
