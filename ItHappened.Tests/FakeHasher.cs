using ItHappened.Infrastructure;

namespace ItHappened.Tests
{
    public class FakeHasher : IHasher
    {
        public string MakeSaltedHash(string source)
        {
            return source;
        }

        public bool VerifySaltedHash(string source, string hash)
        {
            return source == hash;
        }
    }
}