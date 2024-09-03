using Kcd.Common;
using Kcd.Common.Enums;
using Kcd.Common.Exceptions;
using Kcd.Identity.Entities;
using Kcd.Identity.Helpers;
using Kcd.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stayr.Backend.Common.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kcd.Identity.Services;

public class AuthService(UserManager<KcdUser> userManager,
    SignInManager<KcdUser> signInManager,
    RoleManager<KcdRole> roleManager,
    ISystemClock clock,
    ILogger<AuthService> logger,
    IOptions<JwtSettings> jwtSettings) : IAuthService
{
    private readonly UserManager<KcdUser> _userManager = userManager;
    private readonly SignInManager<KcdUser> _signInManager = signInManager;
    private readonly RoleManager<KcdRole> _roleManager = roleManager;
    private readonly ISystemClock _clock = clock;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    //private readonly IServiceBusMessagePublisher _messagePublisher; TODO

    public async Task<AuthResponse> LoginAsync(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new NotFoundException($"User with {request.Email} not found.", request.Email);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (result.Succeeded == false)
        {
            throw new BadRequestException($"Credentials for '{request.Email} aren't valid'.");
        }

        var token = await GenerateToken(user);

        await _userManager.UpdateAsync(user);

        return new AuthResponse
        {
            Token = token
        };
    }

    public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
    {
        var user = new KcdUser
        {
            Email = request.Email,
            Name = request.Name,
            UserName = request.UserName,
            LockoutEnd = _clock.UtcNow,
            Company = request.Company,
            Country = request.Country,
            Referral = request.Referral,
            AvatarId = request.AvatarId,
            LockoutEnabled = true,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, Roles.User.ToString());

            return new RegistrationResponse() { UserId = user.Id };
        }
        else
        {
            string error = LoggingHelper.GetFullIdentityError(result.Errors);

            _logger.LogError(LogEvents.Api.RegistrationError, error);

            throw new BadRequestException(error);
        }
    }

    private async Task<string> GenerateToken(KcdUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        List<Claim> roleClaims = new List<Claim>();
        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is not null)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, roleName));
                roleClaims.AddRange(await _roleManager.GetClaimsAsync(role));
            }
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(Constants.Country_Claim_Type, user.Country),
            new Claim(Constants.Company_Claim_Type, user.Company),
            new Claim(Constants.Avatar_Id_Claim_Type, user.AvatarId),
            new Claim(Constants.Uid_Claim_Type, user.Id)
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
           issuer: _jwtSettings.Issuer,
           audience: _jwtSettings.Audience,
           claims: claims,
           expires: _clock.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes).UtcDateTime,
           signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}
