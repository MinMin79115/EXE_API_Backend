using System.Threading.Tasks;

namespace EXE_API_Backend.Services
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshToken(string userId);
        Task<bool> ValidateRefreshToken(string userId, string refreshToken);
        Task RevokeRefreshToken(string userId, string refreshToken);
    }
} 