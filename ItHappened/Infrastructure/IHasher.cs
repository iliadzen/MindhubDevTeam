namespace ItHappened.Infrastructure
{
    public interface IHasher
    {
        string MakeHash(string source);
        bool VerifyHash(string source, string hash);
    }
}