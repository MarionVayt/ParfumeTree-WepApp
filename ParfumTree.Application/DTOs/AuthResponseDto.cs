namespace ParfumTree.Application.DTOs;

public class AuthResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public List<string> Errors { get; set; } = new();
}