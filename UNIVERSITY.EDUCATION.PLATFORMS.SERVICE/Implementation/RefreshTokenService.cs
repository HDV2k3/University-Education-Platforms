using Microsoft.EntityFrameworkCore;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Infrastructure;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;
using UNIVERSITY.EDUCATION.PLATFORMS.SERVICE.Interface;

namespace UNIVERSITY.EDUCATION.PLATFORMS.SERVICE.Implementation
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IApplicationDbContext _db;

        public RefreshTokenService(IApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task SaveTokenAsync(RefreshTokenEntity refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException(nameof(refreshToken));

            await _db.AddAsync(refreshToken, isCommit: true);
        }

        public async Task<RefreshTokenEntity> GetByTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null!;

            return await _db.FirstOrDefaultAsync<RefreshTokenEntity>(
                predicate: x => x.RefreshToken == token
            ) ?? null!;
        }


        public async Task UpdateAsync(RefreshTokenEntity refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException(nameof(refreshToken));

            await _db.UpdateAsync(refreshToken, isCommit: true);
        }

     
        public async Task DeleteByUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return;

            await _db.DeleteRangeAsync<RefreshTokenEntity>(
                predicate: x => x.UserId.ToString() == userId,
                isCommit: true
            );
        }
    }
}
