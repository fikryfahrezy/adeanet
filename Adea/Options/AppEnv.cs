using System.ComponentModel.DataAnnotations;

namespace Adea.Options;

public record JwtOptions
{
    [Required]
    public IEnumerable<string> ValidAudiences { get; set; } = new List<string>();

    [Required]
    public string ValidIssuer { get; set; } = "";

    [Required]
    public string IssuerSigningKey { get; set; } = "";
}

public class AppEnvOptions
{
    public const string AppEnv = "AppEnv";

    [Required]
    [StringLength(255)]
    public string UploadDestinationPath { get; set; } = "";

    [Required]
    public JwtOptions Jwt { get; set; } = new JwtOptions();
}