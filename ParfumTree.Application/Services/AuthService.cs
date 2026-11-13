using Microsoft.AspNetCore.Identity;
using ParfumTree.Application.DTOs;
using ParfumTree.Application.Interfaces;
using ParfumTree.Domain.Common;
using ParfumTree.Domain.Entities;

namespace ParfumTree.Application.Services;

public class AuthService : IAuthService
{
    private const string InvalidCredentialsMessage = "Невірний Email або пароль.";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var email = dto.Email.Trim();

        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return Fail("Користувач з таким Email вже існує.");
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim()
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return Fail("Помилка при створенні користувача.", result.Errors.Select(e => e.Description));
        }

        var roleResult = await _userManager.AddToRoleAsync(user, AppRoles.User);
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return Fail("Не вдалося призначити роль користувачу.", roleResult.Errors.Select(e => e.Description));
        }

        return new AuthResponseDto { IsSuccess = true, Message = "Реєстрація успішна!" };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email.Trim());
        if (user == null)
        {
            return Fail(InvalidCredentialsMessage);
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            return Fail("Забагато невдалих спроб входу. Акаунт тимчасово заблоковано, спробуйте пізніше.");
        }

        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            await _userManager.AccessFailedAsync(user);
            return Fail(InvalidCredentialsMessage);
        }

        await _userManager.ResetAccessFailedCountAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var (token, expiresAt) = _tokenService.CreateToken(user, roles);

        return new AuthResponseDto
        {
            IsSuccess = true,
            Message = "Успішний вхід",
            Token = token,
            ExpiresAt = expiresAt,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            Roles = roles.ToList()
        };
    }

    private static AuthResponseDto Fail(string message, IEnumerable<string>? errors = null) => new()
    {
        IsSuccess = false,
        Message = message,
        Errors = errors?.ToList() ?? new List<string>()
    };
}