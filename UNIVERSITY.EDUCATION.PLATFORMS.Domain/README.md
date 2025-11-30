
# Luồng Hoạt Động Của Kiến Trúc Database

Hệ thống sử dụng kiến trúc Clean Architecture + DDD kết hợp với EF Core ORM để tổ chức database và domain model theo hướng mô-đun, dễ mở rộng và dễ bảo trì.
Dưới đây là mô tả chi tiết về luồng hoạt động giữa các bảng và các quan hệ trong hệ thống:

🔹 1. Users – Tài khoản trung tâm (Identity Root)

Users là bảng gốc đại diện cho tất cả các tài khoản đăng nhập trong hệ thống.

Luồng hoạt động:

Khi một tài khoản được tạo → bản ghi được lưu trong Users.

Tài khoản được gán loại thông qua UserTypeId.

Nếu tài khoản là sinh viên, sẽ sinh ra hồ sơ tương ứng trong bảng Students.

🔹 2. Students – Hồ sơ mở rộng của User (User Profile)

Students lưu thông tin hồ sơ sinh viên chi tiết.

Luồng hoạt động:

Mỗi Student được liên kết 1–1 với Users qua UserId.

Khi User loại “Student” được tạo → hệ thống tạo một Students profile.

Khi lấy thông tin User → hệ thống tự động load StudentProfile (nếu có).

🔹 3. UserType – Xác định loại tài khoản

Bảng UserType dùng để phân loại:

Student

Teacher

Staff

Admin

Others

Luồng hoạt động:

UserTypeId từ Users → trỏ vào bảng UserType.

Hệ thống dựa vào loại người dùng để kiểm soát UI, logic và phân quyền.

🔹 4. Role & Permission – Phân quyền theo RBAC

Hệ thống áp dụng Role-Based Access Control (RBAC).

Role

Đại diện nhóm quyền, ví dụ:

Admin

Teacher

Student

Manager

Permission

Đại diện hành động cụ thể:

ViewStudentList

EditStudent

ApproveSubject

ManageUsers

RolePermission (n–n)

Quan hệ giữa Role và Permission.

Luồng hoạt động:

Mỗi Role chứa nhiều Permission.

Mỗi Permission có thể thuộc nhiều Role.

🔹 5. UserRole – Quan hệ User và Role (n–n)

UserRole định nghĩa một User thuộc những Role nào.

Luồng hoạt động:

Khi user đăng nhập → hệ thống lấy danh sách Role từ UserRole.

Từ Role → lấy Permission.

Từ Permission → dựng permission tree để kiểm tra truy cập.

🔹 6. Soft Delete & Audit Logging (Theo dõi lịch sử)

Tất cả entity kế thừa từ AuditableBaseEntity, gồm các trường:

CreatedBy

CreatedDate

ModifiedBy

ModifiedDate

IsDeleted

Luồng hoạt động:

SaveChangesAsync tự động gắn CreatedBy/ModifiedBy.

Xóa dữ liệu không xóa thật, mà chuyển IsDeleted = true.

Dễ dàng truy vết, undo hoặc audit.

🔹 7. Tương tác giữa các bảng (Tổng quan luồng dữ liệu)
User đăng nhập → Load User
                    ↓
               Load UserRole
                    ↓
                 Load Role
                    ↓
           Load RolePermission
                    ↓
            Load Permission List

Song song:

User → UserType  → Kiểm soát loại tài khoản
User → StudentProfile (nếu là sinh viên)

Khi ghi dữ liệu:

Entity.Add() → SaveChangesAsync → Ghi log CreatedBy/CreatedDate
Entity.Update() → SaveChangesAsync → Ghi log ModifiedBy/ModifiedDate
Entity.Delete() → IsDeleted = true (soft delete)
