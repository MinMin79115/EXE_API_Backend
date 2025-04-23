using System;
using System.Threading.Tasks;

namespace EXE_API_Backend.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IEmailHistoryRepository EmailHistoryRepository { get; }
        // Thêm các repository khác ở đây khi cần
        Task<int> CommitAsync();
    }
} 