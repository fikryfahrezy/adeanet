using System.Text;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace Adea.Common;

public enum Argon2Type
{
	I,
	D,
	ID,
}


public record Argon2Param
{
	public int DegreeOfParallelism = 16;
	public int MemorySize = 8192;
	public int Iterations = 15;
	public byte[] AssociatedData = null;
	public byte[] KnownSecret = null;
}

public class Argon2
{

	private static int defaultBytes = 16;
	private static int defaultDegreeOfParallelism = 16;
	private static int defaultMemorySize = 8192;
	private static int defaultIterations = 15;
	private static byte[] defaultAssociatedData = null;
	private static byte[] defaultKnownSecret = null;

	private static byte[] HashToArgon2id(string word, byte[] salt, int bytes, Argon2Param config)
	{


		var subject = new Argon2id(Encoding.ASCII.GetBytes(word))
		{
			DegreeOfParallelism = config.DegreeOfParallelism,
			Iterations = config.Iterations,
			MemorySize = config.MemorySize,
			Salt = salt,
		};

		if (config.AssociatedData != null)
		{
			subject.AssociatedData = config.AssociatedData;
		}

		if (config.KnownSecret != null)
		{
			subject.KnownSecret = config.KnownSecret;
		}

		return subject.GetBytes(bytes);

	}

	public static string Hash(Argon2Type type, string word, int bytes, Argon2Param config)
	{
		var salt = RandomNumberGenerator.GetBytes(bytes);

		var hashedBytes = type switch
		{
			_ => HashToArgon2id(word, salt, bytes, config),
		};

		return $"$argon2{Argon2TypeToString(type)}$m={config.MemorySize},t={config.Iterations},p={config.DegreeOfParallelism},b={bytes}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hashedBytes)}";
	}

	public static bool Verify(string word, string hashedWord)
	{
		var hashedWords = hashedWord.Split("$");
		if (hashedWords.Length != 5)
		{
			return false;
		}

		var type = hashedWords[1].Split("argon2");
		if (type.Length != 2)
		{
			return false;
		}

		var argon2Param = new Dictionary<string, int>();
		foreach (string param in hashedWords[2].Split(","))
		{
			var splittedParam = param.Split("=");
			if (splittedParam.Length != 2)
			{
				continue;
			}
			argon2Param.Add(splittedParam[0], Int32.Parse(splittedParam[1]));
		}


		var iterations = defaultDegreeOfParallelism;
		var degreeOfPrallelism = defaultDegreeOfParallelism;
		var memorySize = defaultMemorySize;
		var bytes = defaultBytes;

		argon2Param.TryGetValue("t", out iterations);
		argon2Param.TryGetValue("p", out degreeOfPrallelism);
		argon2Param.TryGetValue("m", out memorySize);
		argon2Param.TryGetValue("b", out bytes);

		var newHashedBytes = StringToArgon2Type(type[1]) switch
		{
			_ => HashToArgon2id(word, Convert.FromBase64String(hashedWords[3]), bytes, new Argon2Param()
			{
				Iterations = iterations,
				DegreeOfParallelism = degreeOfPrallelism,
				MemorySize = memorySize,
				AssociatedData = defaultAssociatedData,
				KnownSecret = defaultKnownSecret
			}),
		};

		return Convert.FromBase64String(hashedWords[4]).SequenceEqual(newHashedBytes);
	}

	private static string Argon2TypeToString(Argon2Type type) => type switch
	{
		_ => "id",
	};

	private static Argon2Type StringToArgon2Type(string type) => type switch
	{
		_ => Argon2Type.ID,
	};
}