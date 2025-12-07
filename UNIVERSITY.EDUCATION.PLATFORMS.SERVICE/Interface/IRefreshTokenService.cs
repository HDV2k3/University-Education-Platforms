using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Entities;

namespace UNIVERSITY.EDUCATION.PLATFORMS.SERVICE.Interface
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenEntity> GetByTokenAsync(string token);
        Task SaveTokenAsync(RefreshTokenEntity refreshToken);
        Task UpdateAsync(RefreshTokenEntity refreshToken);
        Task DeleteByUserIdAsync(string userId);
    }
}
