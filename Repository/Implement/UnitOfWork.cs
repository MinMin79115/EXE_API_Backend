using MongoDB.Driver;
using System.Threading.Tasks;
using EXE_API_Backend.Repositories.Interface;
using EXE_API_Backend.Models.Database;

namespace EXE_API_Backend.Repositories.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExeApiDBContext _context;
        private IUserRepository _userRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        private IEmailHistoryRepository _emailHistoryRepository;

        public UnitOfWork(ExeApiDBContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ??= new UserRepository(_context); }
        }

        public IRefreshTokenRepository RefreshTokenRepository
        {
            get { return _refreshTokenRepository ??= new RefreshTokenRepository(_context); }
        }

        public IEmailHistoryRepository EmailHistoryRepository
        {
            get { return _emailHistoryRepository ??= new EmailHistoryRepository(_context); }
        }

        public async Task<int> CommitAsync()
        {
            // Với MongoDB, không cần commit như SQL vì các thao tác là độc lập
            // Trả về 1 để biểu thị thành công
            return await Task.FromResult(1);
        }

        public void Dispose()
        {
            // Không cần dispose đặc biệt với MongoDB
        }
    }
} 