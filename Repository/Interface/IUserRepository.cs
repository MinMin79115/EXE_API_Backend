using EXE_API_Backend.Models.Model;
using System.Threading.Tasks;

namespace EXE_API_Backend.Repositories.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
    }
} 