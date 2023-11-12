using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace service.Services;

public class JwtOptions
{
    public required byte[] Secret { get; init; }
    public required TimeSpan Lifetime { get; init; }
    public string? Address { get; set; }
}

public class JwtService(JwtOptions options)
{
    private const string SignatureAlgorithm = SecurityAlgorithms.HmacSha512;
    public string IssueToken(SessionData data)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.CreateEncodedJwt(new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(options.Secret),
                SignatureAlgorithm
            ),
            Issuer = options.Address,
            Audience = options.Address,
            Expires = DateTime.UtcNow.Add(options.Lifetime),
            Claims = data.ToDictionary()
        });
        return token;
    }

    public SessionData ValidateAndDecodeToken(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var principal = jwtHandler.ValidateToken(token, new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(options.Secret),
            ValidAlgorithms = new[] { SignatureAlgorithm },

            // Default value is true already.
            // They are just set here to emphasise the importance.
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,

            ValidAudience = options.Address,
            ValidIssuer = options.Address,

            // Set to 0 when validating on the same system that created the token
            ClockSkew = TimeSpan.FromSeconds(0)
        }, out var securityToken);
        return SessionData.FromDictionary(new JwtPayload(principal.Claims));
    }
}