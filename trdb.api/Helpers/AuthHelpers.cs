using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using trdb.entity.Helpers;

namespace trdb.api.Helpers
{
    public class AuthHelpers
    {
        public static bool Authorize(HttpContext context, int AuthorizedAuthType)
        {
            return ValidateToken(ReadBearerToken(context), AuthorizedAuthType);
        }

        public static string? ReadBearerToken(HttpContext context)
        {
            try
            {
                string header = (string)context.Request.Headers["Authorization"];
                if (header != null)
                {
                    return header.Substring(7);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static bool ValidateToken(string? encodedToken, int AuthorizedAuthType)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
                tokenHandler.ValidateToken(encodedToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                //var userID = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                var authType = int.Parse(jwtToken.Claims.First(x => x.Type == "authType").Value);
                if (AuthorizedAuthType == authType)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
