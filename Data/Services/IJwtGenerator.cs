using Data.ETF;

namespace Data.Services
{
    public interface IJwtGenerator
    {
        string CreateToken(UserModel user);
    }
}