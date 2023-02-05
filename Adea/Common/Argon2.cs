using System.Text;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using Adea.Exceptions;

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
    public byte[]? AssociatedData = null;
    public byte[]? KnownSecret = null;
}

public class Argon2
{

    private const int defaultBytes = 16;
    private const int defaultDegreeOfParallelism = 16;
    private const int defaultMemorySize = 8192;
    private const int defaultIterations = 15;
    private const byte[] defaultAssociatedData = null;
    private const byte[] defaultKnownSecret = null;
    private const int defaultKeyLen = 16;
    private const int defaultSaltLen = 16;

    private static byte[] HashToArgon2i(byte[] word, byte[] salt, int bytes, Argon2Param config)
    {


        var subject = new Argon2i(word)
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

    private static byte[] HashToArgon2d(byte[] word, byte[] salt, int bytes, Argon2Param config)
    {


        var subject = new Argon2d(word)
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

    private static byte[] HashToArgon2id(byte[] word, byte[] salt, int bytes, Argon2Param config)
    {


        var subject = new Argon2id(word)
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

    public static string Hash(Argon2Type type, string word, Argon2Param? config, int keyLen = defaultKeyLen, int saltLen = defaultSaltLen)
    {
        var hashConfig = config != null ? config : new Argon2Param();
        var salt = RandomNumberGenerator.GetBytes(saltLen);
        var byteWord = Encoding.ASCII.GetBytes(word);

        var hashedBytes = type switch
        {
            Argon2Type.I => HashToArgon2i(byteWord, salt, keyLen, hashConfig),
            Argon2Type.D => HashToArgon2d(byteWord, salt, keyLen, hashConfig),
            Argon2Type.ID => HashToArgon2id(byteWord, salt, keyLen, hashConfig),
            _ => HashToArgon2id(byteWord, salt, keyLen, hashConfig),
        };

        return $"$argon2{Argon2TypeToString(type)}$m={hashConfig.MemorySize},t={hashConfig.Iterations},p={hashConfig.DegreeOfParallelism}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hashedBytes)}";
    }

    public static void VerifyAndThrow(string word, string hashedWord)
    {
        var hashedWords = hashedWord.Split("$");
        if (hashedWords.Length != 5)
        {
            throw new UnprocessableEntityException("Not valid argon hashed string");
        }

        var type = hashedWords[1].Split("argon2");
        if (type.Length != 2)
        {
            throw new UnprocessableEntityException("Not valid argon type");
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

        argon2Param.TryGetValue("t", out iterations);
        argon2Param.TryGetValue("p", out degreeOfPrallelism);
        argon2Param.TryGetValue("m", out memorySize);

        var byteWord = Encoding.ASCII.GetBytes(word);
        var decodedSalt = Convert.FromBase64String(hashedWords[3]);
        var decodedHash = Convert.FromBase64String(hashedWords[4]);
        var keyLen = decodedHash.Length;

        var newHashedBytes = StringToArgon2Type(type[1]) switch
        {
            Argon2Type.I => HashToArgon2i(byteWord, decodedSalt, keyLen, new Argon2Param()
            {
                Iterations = iterations,
                DegreeOfParallelism = degreeOfPrallelism,
                MemorySize = memorySize,
                AssociatedData = defaultAssociatedData,
                KnownSecret = defaultKnownSecret
            }),
            Argon2Type.D => HashToArgon2d(byteWord, decodedSalt, keyLen, new Argon2Param()
            {
                Iterations = iterations,
                DegreeOfParallelism = degreeOfPrallelism,
                MemorySize = memorySize,
                AssociatedData = defaultAssociatedData,
                KnownSecret = defaultKnownSecret
            }),
            Argon2Type.ID => HashToArgon2id(byteWord, decodedSalt, keyLen, new Argon2Param()
            {
                Iterations = iterations,
                DegreeOfParallelism = degreeOfPrallelism,
                MemorySize = memorySize,
                AssociatedData = defaultAssociatedData,
                KnownSecret = defaultKnownSecret
            }),
            _ => HashToArgon2id(byteWord, decodedSalt, keyLen, new Argon2Param()
            {
                Iterations = iterations,
                DegreeOfParallelism = degreeOfPrallelism,
                MemorySize = memorySize,
                AssociatedData = defaultAssociatedData,
                KnownSecret = defaultKnownSecret
            }),
        };

        if (!decodedHash.SequenceEqual(newHashedBytes))
        {
            throw new UnauthorizedException("Password not match");
        }
    }

    private static string Argon2TypeToString(Argon2Type type) => type switch
    {
        Argon2Type.I => "i",
        Argon2Type.D => "d",
        Argon2Type.ID => "id",
        _ => "id",
    };

    private static Argon2Type StringToArgon2Type(string type) => type switch
    {
        "i" => Argon2Type.I,
        "d" => Argon2Type.D,
        "id" => Argon2Type.ID,
        _ => Argon2Type.ID,
    };
}