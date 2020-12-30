using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JWTtest
{
    public class JwtAuthManager : IJwtAuthManager
    {
        private readonly string _key;

        public JwtAuthManager(string key)
        {
            _key = key;
        }

        private readonly Dictionary<string, string> users = new Dictionary<string, string>
        {
            {"test1", "password1"}, {"test2", "password2"}, {"test3", "password3"}

        };

        //Logic for Authentication
        public string Authenticate(string username, string password)
        {
            if(!users.Any(u => u.Key == username && u.Value == password))
            {
                return null;
            }

            //Create a token handler
            var tokenhandler = new JwtSecurityTokenHandler();

            //The key is nothing but a Byte, the _key is coming in from the constructor.
            var tokenKey = Encoding.ASCII.GetBytes(_key);

            //Creates the tokenDescriptor where we define what the token should hold for us with Claims.
            //Set the expiredate for the token.
            //How to sign the token, we are passing a key and the algorithm to use.

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //FACT about the user, such as 'name' or 'role'
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                }),

                //How long should the token exist
                Expires = DateTime.UtcNow.AddHours(1),

                //Securitykey and define which algorithm to use
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            //This one will give us back the securityToken
            var token = tokenhandler.CreateToken(tokenDescriptor);

            //Return the string JWT Token
            return tokenhandler.WriteToken(token);
            

        }
    }
}
