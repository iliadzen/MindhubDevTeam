namespace ItHappened.Infrastructure
{
    public interface IHasher
    {
        string MakeSaltedHash(string source);
        bool VerifySaltedHash(string source, string hash);
    }
}