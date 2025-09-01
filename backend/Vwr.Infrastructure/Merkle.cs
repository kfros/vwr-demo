using Nethereum.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace Vwr.Infrastructure;

public static class Merkle
{
    static readonly Sha3Keccack Keccak = Sha3Keccack.Current;

    public static byte[] KeccakPlaceHolder(byte[] input)
    {
        // Placeholder for Keccak-256 hashing. Use SHA256 as a substitute. In production, replace with a proper Keccak-256 implementation.
        using var sha = SHA256.Create();
        return sha.ComputeHash(input);
    }

    static BigInteger BytesToBig(byte[] x) => new BigInteger(new byte[] { 0 }.Concat(x).Reverse().ToArray());

    static byte[] HashPair(byte[] a, byte[] b)
    {
        var (x, y) = (BytesToBig(a) <= BytesToBig(b)) ? (a, b) : (b, a);
        var concat = new byte[x.Length + y.Length];
        Buffer.BlockCopy(x, 0, concat, 0, x.Length);
        Buffer.BlockCopy(y, 0, concat, x.Length, y.Length);
        return Keccak.CalculateHash(concat);
    }

    public static byte[] Root(IEnumerable<byte[]> leaves)
    {
        var level = leaves.Select(x => x).ToList();
        if (level.Count == 0) throw new InvalidOperationException("No leaves");
        while (level.Count > 1)
        {
            var next = new List<byte[]>(capacity: (level.Count + 1) / 2);
            for (int i = 0; i < level.Count; i += 2)
            {
                if (i + 1 == level.Count) next.Add(level[i]);     // odd — promote
                else next.Add(HashPair(level[i], level[i + 1]));
            }
            level = next;
        }
        return level[0];
    }

    public static string ToHex32(byte[] bytes) => "0x" + BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
}