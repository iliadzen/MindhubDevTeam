using ItHappened.Domain;

namespace ItHappened.App.Authentication
{
    public interface IJwtIssuer
    {
        string GenerateToken(User user);
    }
}