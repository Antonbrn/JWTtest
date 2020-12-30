using System;
namespace JWTtest
{
    public interface IJwtAuthManager
    {
        string Authenticate(string userid, string password);
    }
}
