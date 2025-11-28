using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence.Configurations
{

    /// <summary>
    /// Lớp cơ sở cài đặt chuẩn Dispose pattern của .NET,
    /// dùng để giải phóng tài nguyên cho các lớp kế thừa.
    ///
    /// - Ngăn dispose nhiều lần (_disposed)
    /// - Hỗ trợ giải phóng tài nguyên managed/unmanaged
    /// - Cho phép lớp con override Dispose(bool)
    /// - Tự động gọi GC.SuppressFinalize để tối ưu bộ nhớ
    ///
    /// Dùng khi service trong Application có tài nguyên cần giải phóng
    /// (MemoryStream, file, kết nối, buffer,…)
    /// </summary>s

    public abstract class ApplicationDisposable : IDisposable
    {
        protected bool _disposed = false;

        ~ApplicationDisposable() => Dispose(false);


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
            }

            _disposed = true;
        }
    }
}
