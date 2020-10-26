using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ItHappened.Infrastructure
{
    public class Sha256Hasher : IHasher
    {
        public const int SaltLength = 32;

        private byte[] GetSalt(int length)
        {
            using RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[length];
            random.GetNonZeroBytes(saltBytes);
            return saltBytes;
        }

        private byte[] JoinBytes(byte[] left, byte[] right)
        {
            byte[] joined = new byte[left.Length + right.Length];
            Buffer.BlockCopy(left, 0, joined, 0, left.Length);
            Buffer.BlockCopy(right, 0, joined, left.Length, right.Length);
            return joined;
        }

        private byte[] GetSaltedHash(byte[] saltBytes, byte[] sourceBytes)
        {
            using SHA256 sha256 = SHA256.Create();
            return JoinBytes(saltBytes, sha256.ComputeHash(JoinBytes(saltBytes, sourceBytes)));
        }

        private byte[] SourceStringToBytes(string s) =>
            Encoding.UTF8.GetBytes(s);

        private string HashBytesToString(byte[] bytes) =>
            Convert.ToBase64String(bytes);

        private byte[] HashStringToBytes(string s) =>
            Convert.FromBase64String(s);

        public string MakeSaltedHash(string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            
            byte[] sourceBytes = SourceStringToBytes(source);
            byte[] saltBytes = GetSalt(SaltLength);
            byte[] hashBytes = GetSaltedHash(saltBytes, sourceBytes);

            return HashBytesToString(hashBytes);
        }

        public bool VerifySaltedHash(string source, string hash)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (hash == null) throw new ArgumentNullException(nameof(hash));

            byte[] hashBytes = HashStringToBytes(hash);
            byte[] sourceBytes = SourceStringToBytes(source);
            byte[] saltBytes = new byte[SaltLength];
            Buffer.BlockCopy(hashBytes, 0, saltBytes, 0, SaltLength);
            
            return GetSaltedHash(saltBytes, sourceBytes).SequenceEqual(hashBytes);
        }
    }
}