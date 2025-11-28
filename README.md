# University Education Platforms  
### Monolithic Clean Architecture (.NET 9 + Aspire + EF Core + Swagger)

Dự án **University Education Platforms** được xây dựng theo chuẩn **Clean Architecture** và chạy dưới dạng **Monolithic Application** kết hợp với **.NET Aspire AppHost** để quản lý runtime & service orchestration.

---

# 🏗 Kiến trúc tổng thể

Dự án được chia thành các layer tách biệt nhằm đảm bảo:

- **Tách biệt trách nhiệm**
- **Dễ bảo trì – mở rộng**
- **Đảm bảo business không phụ thuộc UI/Infrastructure**
- **Dễ test – dễ thay thế công nghệ**

Sơ đồ hướng phụ thuộc:
src/
├── UNIVERSITY.EDUCATION.PLATFORMS → API (Web API .NET 9)
├── UNIVERSITY.EDUCATION.PLATFORMS.Web → Web MVC UI (Bootstrap 5.3 + jQuery 3.7) (optional)
├── UNIVERSITY.EDUCATION.PLATFORMS.Application → Application Layer (Use Cases / Services)
├── UNIVERSITY.EDUCATION.PLATFORMS.Domain → Domain Layer (Entities, Enums, Rules)
├── UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure → EF Core, DbContext, Repository
├── UNIVERSITY.EDUCATION.PLATFORMS.ServiceDefaults → Logging, Telemetry, HealthCheck
└── UNIVERSITY.EDUCATION.PLATFORMS.AppHost → Aspire host (DistributedApplication)

---

# 🧩 Vai trò từng layer

## 🔹 **Domain**
- Chỉ chứa business rule thuần
- Entity, ValueObject, Enum
- Không phụ thuộc bất kỳ layer nào khác

## 🔹 **Application**
- Chứa logic nghiệp vụ (UseCases, Services)
- Chỉ phụ thuộc Domain
- Không đụng đến DB, EF Core, Swagger

## 🔹 **Infrastructure**
- Chứa DbContext, Repository, external services
- Dùng EF Core + SQL Server/PostgreSQL
- Phụ thuộc Domain

## 🔹 **API (Presentation Layer)**
- Cung cấp REST API
- Áp dụng DI từ Application & Infrastructure
- Định nghĩa Swagger, Filters, Versioning

## 🔹 **Web/MVC UI**
- Giao diện người dùng (HTML, Bootstrap 5.3, jQuery 3.7) (optional)

## 🔹 **ServiceDefaults**
- Logging
- Tracing
- OpenTelemetry
- Health checks

## 🔹 **AppHost**
- .NET Aspire Distributed Application host
- Điều khiển chạy các project trong monolithic mode

---

# 🧰 Dependency Injection (DI)

## 🔹 Author
- Huynh Dac Viet

