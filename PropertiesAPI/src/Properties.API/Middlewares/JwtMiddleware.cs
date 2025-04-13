using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Properties.Core.Models;

namespace Properties.API.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
        }

        public async Task Invoke(HttpContext context)
        {

            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var authorizeAttribute = endpoint.Metadata.GetMetadata<AuthorizeAttribute>();
                if (authorizeAttribute == null)
                {
                    await _next(context);
                    return;
                }
            }

            var token = ExtractTokenFromHeader(context);

            if (token != null)
            {
                try
                {
                    ValidateToken(token);
                }
                catch (SecurityTokenExpiredException)
                {
                    await WriteErrorResponse(context, "Token expired.", StatusCodes.Status401Unauthorized);
                    return;
                }
                catch (SecurityTokenInvalidIssuerException)
                {
                    await WriteErrorResponse(context, "Invalid token issuer.", StatusCodes.Status401Unauthorized);
                    return;
                }
                catch (SecurityTokenInvalidAudienceException)
                {
                    await WriteErrorResponse(context, "Invalid token audience.", StatusCodes.Status401Unauthorized);
                    return;
                }
                catch (Exception)
                {
                    await WriteErrorResponse(context, "Invalid token.", StatusCodes.Status401Unauthorized);
                    return;
                }
            }
            else
            {
                await WriteErrorResponse(context, "Missing or invalid token.", StatusCodes.Status401Unauthorized);
                return;
            }

            await _next(context);
        }

        private string ExtractTokenFromHeader(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                return authHeader.Substring("Bearer ".Length).Trim();
            }

            return null;
        }

        private void ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtIssuer,
                ValidateAudience = true,
                ValidAudience = _jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
        }

        private async Task WriteErrorResponse(HttpContext context, string message, int statusCode)
        {
            var response = ApiResponse<string>.Error(
                method: context.Request.Method,
                url: context.Request.Path,
                statusCode: statusCode,
                message: message
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }

    }
}