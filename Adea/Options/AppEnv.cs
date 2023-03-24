using System.ComponentModel.DataAnnotations;

namespace Adea.Options;

public class AppEnvOptions
{
    public const string AppEnv = "AppEnv";

    [Required]
    [MinLength(0)]
    public string UploadDestinationDirname { get; set; } = "";

    [Required]
    public IEnumerable<string> JwtValidAudiences { get; set; } = new List<string>();

    [Required]
    [MinLength(0)]
    public string JwtValidIssuer { get; set; } = "";

    [Required]
    [MinLength(44)]
    public string JwtIssuerSigningKey { get; set; } = "";
}