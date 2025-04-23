using EXE_API_Backend.Models.Model;
using System.Threading.Tasks;

namespace EXE_API_Backend.Repositories.Interface
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetByUserIdAndTokenAsync(string userId, string token);
        Task RevokeRefreshTokensByUserIdAsync(string userId);
    }
} 