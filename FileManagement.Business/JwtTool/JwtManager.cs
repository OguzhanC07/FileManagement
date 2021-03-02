using FileManagement.DataAccess;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.JwtTool
{
    public class JwtManager : IJwtService
    {
        public JwtToken GenerateToken(User user)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConstant.SecretKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(issuer: JwtConstant.Issuer, audience: JwtConstant.Audience, claims: SetClaims(user),notBefore:DateTime.Now,expires:DateTime.Now.AddMinutes(60),signingCredentials:credentials);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtToken jwtToken = new JwtToken();
            jwtToken.Token=handler.WriteToken(token);

            return jwtToken;
        }

        private List<Claim> SetClaims(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            return claims;
        }
    }
}
