using MongoDB.Driver;
using System.Threading.Tasks;
using EXE_API_Backend.Models.Database;
using EXE_API_Backend.Models.Model;
using EXE_API_Backend.Repositories.Interface;
using EXE_API_Backend.Repositories.Implement;

namespace EXE_API_Backend.Repositories.Implement
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ExeApiDBContext context) : base(context, "Users")
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.userName, username);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.email, email);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
} 