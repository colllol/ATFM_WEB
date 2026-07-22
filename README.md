# ATFM API

ASP.NET Web API 2 / .NET Framework 4.8 cho hệ thống ATFM.

## Cấu hình local

1. Cài Oracle Data Access Components 64-bit và cập nhật `HintPath` nếu cần.
2. Khôi phục các NuGet package của solution.
3. Thay các giá trị `YOUR_ORACLE_*` trong `QLB.API/Web.config` bằng cấu hình môi trường.
4. Thay khóa JWT mẫu bằng secret được quản lý ngoài source control trước khi triển khai.

Không commit credential, khóa JWT thật, thư mục build, package hoặc ODAC binary vào repository.
