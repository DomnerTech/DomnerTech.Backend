using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace DomnerTech.Backend.Application.Helpers;

public static class PasswordHashHelper
{
    private const int SaltSize = 16;        // 128-bit
    private const int HashSize = 32;        // 256-bit
    private const int Iterations = 4;       // Time cost
    private const int MemorySize = 65536;   // 64 MB
    private const int DegreeOfParallelism = 2;

    public static string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySize
        };

        var hash = argon2.GetBytes(HashSize);

        // Combine salt + hash
        var result = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, result, SaltSize, HashSize);

        return Convert.ToBase64String(result);
    }

    public static bool Verify(string password, string storedHash)
    {
        var decoded = Convert.FromBase64String(storedHash);

        var salt = new byte[SaltSize];
        var hash = new byte[HashSize];

        Buffer.BlockCopy(decoded, 0, salt, 0, SaltSize);
        Buffer.BlockCopy(decoded, SaltSize, hash, 0, HashSize);

        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = DegreeOfParallelism,
            Iterations = Iterations,
            MemorySize = MemorySize
        };

        var newHash = argon2.GetBytes(HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, newHash);
    }
}