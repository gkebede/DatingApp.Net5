using DatingApp_API.interfaces;
using DatingAppAPI.Models;
using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp_API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            // IN ORDER TO MAKE SURE THE TOKEN IS A VALID TOKEN THE SERVER NEED TO SIGN THIS
            //  "TOKEN" THAT IS WHY WE DO THE FOLLWING SECTOIN generating key
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUsers user)
        {
            // OTHER WAY OF instantiating  AppUsers OBJECT
            var claims = new List<Claim>
            {
                   // userNameClaim
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            // generating SigningCredentials
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // USER = new ClaimsIdentity(claims)
                //PASSWORD = creds
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            // token => HOLDS BOTH the 'user && password as a string'
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
