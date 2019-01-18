using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TrelloClone.Infra.Extensions
{
    public static class CookieExtensions
    {
        public static void AppendToken (this IResponseCookies cookies, Config config, string email, HttpContext context)
        {
            cookies.Append(
                config.TokenCookie,
                GetToken(email, context.Connection.RemoteIpAddress, config),
                new CookieOptions {
                    Path = "/",
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.Now.AddMinutes(config.TokenExpire)
                });
        }

        private static string GetToken(string email, IPAddress ip, Config config)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.JWTSecurityKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, email),
                new Claim(config.IPClaimType, ip.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "trelloclone",
                audience: "readers",
                expires: DateTime.Now.AddMinutes(config.TokenExpire),
                signingCredentials: signingCredentials,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
