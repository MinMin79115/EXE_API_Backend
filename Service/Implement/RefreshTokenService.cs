using System;
using System.Threading.Tasks;
using EXE_API_Backend.Models.Model;
using EXE_API_Backend.Repositories.Interface;

namespace EXE_API_Backend.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GenerateRefreshToken(string userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.CommitAsync();
            return refreshToken.Token;
        }

        public async Task<bool> ValidateRefreshToken(string userId, string refreshToken)
        {
            var token = await _unitOfWork.RefreshTokenRepository.GetByUserIdAndTokenAsync(userId, refreshToken);
            if (token == null || token.ExpiresAt < DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }

        public async Task RevokeRefreshToken(string userId, string refreshToken)
        {
            var token = await _unitOfWork.RefreshTokenRepository.GetByUserIdAndTokenAsync(userId, refreshToken);
            if (token != null)
            {
                await _unitOfWork.RefreshTokenRepository.DeleteAsync(token.Id);
                await _unitOfWork.CommitAsync();
            }
        }
    }
} 