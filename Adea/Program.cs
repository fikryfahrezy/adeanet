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
using static Adea.Common.RequestFieldMap;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<AppEnvOptions>()
    .Bind(builder.Configuration.GetSection(AppEnvOptions.AppEnv))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.GetSection(AppEnvOptions.AppEnv).Get<AppEnvOptions>()!;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidAudiences = jwtOptions.Jwt.ValidAudiences,
        ValidIssuer = jwtOptions.Jwt.ValidIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.Jwt.IssuerSigningKey)
        )
    };
});

builder.Services.AddDbContext<LoanLosDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("LoanDatabase")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("jwt_auth", new OpenApiSecurityScheme
    {
        Name = "Bearer",
        BearerFormat = "JWT",
        Scheme = "bearer",
        Description = "Specify the authorization token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
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

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
