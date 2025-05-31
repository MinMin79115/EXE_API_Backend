using MongoDB.Driver;
using System.Threading.Tasks;
using EXE_API_Backend.Models.Database;
using EXE_API_Backend.Models.Model;
using EXE_API_Backend.Repositories.Interface;
using EXE_API_Backend.Repositories.Implement;

namespace EXE_API_Backend.Repositories.Implement
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ExeApiDBContext context) : base(context, "RefreshTokens")
        {
        }

        public async Task<RefreshToken> GetByUserIdAndTokenAsync(string userId, string token)
        {
            var filter = Builders<RefreshToken>.Filter.And(
                Builders<RefreshToken>.Filter.Eq(rt => rt.UserId, userId),
                Builders<RefreshToken>.Filter.Eq(rt => rt.Token, token)
            );
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task RevokeRefreshTokensByUserIdAsync(string userId)
        {
            var filter = Builders<RefreshToken>.Filter.Eq(rt => rt.UserId, userId);
            await _collection.DeleteManyAsync(filter);
        }
    }
} 