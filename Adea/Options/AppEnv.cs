using System.ComponentModel.DataAnnotations;

namespace Adea.Options;

public class AppEnvOptions
{
	public const string AppEnv = "AppEnv";

	[Required]
	[StringLength(255)]
	public string UploadDestinationPath { get; set; } = "";
}