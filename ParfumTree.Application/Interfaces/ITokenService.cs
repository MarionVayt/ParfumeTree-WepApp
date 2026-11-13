using ParfumTree.Domain.Entities;

namespace ParfumTree.Application.Interfaces;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) CreateToken(ApplicationUser user, IList<string> roles);
}