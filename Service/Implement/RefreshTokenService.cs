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
                token = Guid.NewGuid().ToString(),
                userId = userId,
                expiryDate = DateTime.UtcNow.AddDays(7),
                isRevoked = false
            };

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.CommitAsync();
            return refreshToken.token;
        }

        public async Task<bool> ValidateRefreshToken(string userId, string refreshToken)
        {
            var token = await _unitOfWork.RefreshTokenRepository.GetByUserIdAndTokenAsync(userId, refreshToken);
            if (token == null || token.isRevoked || token.expiryDate < DateTime.UtcNow)
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
                token.isRevoked = true;
                await _unitOfWork.RefreshTokenRepository.UpdateAsync(token.id, token);
                await _unitOfWork.CommitAsync();
            }
        }
    }
} 