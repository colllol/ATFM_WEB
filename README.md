# ATFM Web

Hệ thống quản lý luồng không lưu xây dựng trên ASP.NET Web Forms, .NET Framework 4.8. Repository chứa mã nguồn ứng dụng web và các thư viện nghiệp vụ liên quan.

> Lưu ý: repository không chứa mật khẩu, cấu hình máy thật, DLL thương mại hoặc runtime có giấy phép. Không đưa `Web.config` chứa credential thật lên GitHub.

## Thành phần solution

| Project               | Vai trò                          |
| --------------------- | -------------------------------- |
| `prjApplication`      | Ứng dụng ASP.NET Web Forms chính |
| `prjBusinessLogic`    | Business logic và lớp gọi API    |
| `prjInfo`             | Model/DTO                        |
| `HPCServerDataAccess` | Truy cập dữ liệu legacy          |
| `HPCShareDLL`         | Tiện ích dùng chung              |
| `prjComponents`       | Web controls và helper           |
| `CustomControl`       | Custom Web Forms controls        |

## Yêu cầu bắt buộc

- Windows 10/11 hoặc Windows Server 2019 trở lên.
- Visual Studio 2022 với workload **ASP.NET and web development**.
- .NET Framework 4.8 Developer Pack.
- IIS có ASP.NET 4.x, .NET Extensibility 4.x, ISAPI Extensions, ISAPI Filters và Static Content.
- SQL Server/Oracle Client và API backend tương ứng với môi trường triển khai.
- SAP Crystal Reports Runtime 13.x nếu sử dụng báo cáo Crystal.
- Oracle Data Access Components 64-bit đúng phiên bản của `Oracle.DataAccess.dll` nếu sử dụng Oracle.

## Clone và thiết lập

### 1. Clone đúng nhánh

```powershell
git clone https://github.com/colllol/ATFM_WEB.git
cd ATFM_WEB
git checkout agent/initial-atfm-import
```

### 2. Cài Visual Studio và .NET Framework

Trong Visual Studio Installer chọn:

1. **ASP.NET and web development**.
2. Individual components → **.NET Framework 4.8 SDK**.
3. Individual components → **.NET Framework 4.8 targeting pack**.

Khởi động lại máy sau khi cài runtime Oracle hoặc Crystal Reports.

### 3. Khôi phục NuGet và kiểm tra môi trường

Mở PowerShell tại thư mục repository:

```powershell
Set-ExecutionPolicy -Scope Process Bypass
.\scripts\setup.ps1
```

Để restore và build ngay:

```powershell
.\scripts\setup.ps1 -Build
```

### 4. Cấu hình API, cơ sở dữ liệu và FTP

Mở `prjApplication/Web.config` và thay các giá trị local mẫu:

```xml
<add key="applicationpath.api" value="http://localhost:5080/" />
```

Trong provider `HPCDataProvider`, thay connection string bằng tài khoản riêng của môi trường. Không commit mật khẩu thật. FTP mặc định bị tắt (`UseUpload_FTP=0`); chỉ bật khi đã cấu hình server và credential phù hợp.

Nếu dùng Oracle, thay `OracleConnectionString` trong `appSettings` bằng connection string của môi trường. Giá trị trong repository chỉ là placeholder và không thể đăng nhập cơ sở dữ liệu thật.

API backend phải chạy trước khi đăng nhập. Kiểm tra URL API bằng trình duyệt hoặc PowerShell:

```powershell
Invoke-WebRequest http://localhost:5080/ -UseBasicParsing
```

### 5. Chạy bằng Visual Studio

1. Mở `ATFM.sln` bằng Visual Studio 2022.
2. Restore NuGet nếu Visual Studio yêu cầu.
3. Chọn `prjApplication` → **Set as Startup Project**.
4. Build configuration: `Debug`, platform phù hợp Oracle Client (khuyến nghị `x64`).
5. Chạy bằng IIS Express hoặc cấu hình IIS theo phần dưới.

### 6. Thiết lập IIS

1. Bật Windows Features: Internet Information Services, ASP.NET 4.8, .NET Extensibility 4.8, ISAPI Extensions, ISAPI Filters, Static Content.
2. Tạo Application Pool `ATFM_AppPool`:
   - .NET CLR Version: `v4.0`.
   - Managed pipeline: `Integrated`.
   - Enable 32-Bit Applications: `False` nếu dùng Oracle AMD64.
3. Publish project ra một thư mục, hoặc trỏ site đến thư mục publish.
4. Tạo IIS Application `/atfm`, gán `ATFM_AppPool`.
5. Cấp `Read & Execute` cho `IIS_IUSRS`; cấp `Modify` riêng cho thư mục upload/log nếu cần.
6. Kiểm tra `http://localhost/atfm/Login.aspx`.

Hướng dẫn triển khai chi tiết bổ sung nằm trong `HUONG_DAN_TRIEN_KHAI_IIS.md`.

## Dependency DLL legacy

Một số reference cũ có `HintPath` ngoài solution hoặc phụ thuộc runtime thương mại, ví dụ Crystal Reports, Oracle, Office Interop, TuesPechkin và các control nội bộ. Do giới hạn giấy phép, các DLL này không được tự động tải từ repository public.

Nếu build báo thiếu reference:

1. Cài runtime chính thức tương ứng.
2. Với DLL nội bộ, lấy gói dependency được doanh nghiệp phê duyệt.
3. Đặt DLL vào thư mục được `HintPath` chỉ định hoặc cập nhật `HintPath` thành đường dẫn tương đối hợp lệ trên máy.
4. Trong Visual Studio, kiểm tra References có dấu chấm than vàng hay không.

## Xử lý lỗi thường gặp

### `The reference assemblies for .NETFramework,Version=v4.8 were not found`

Cài **.NET Framework 4.8 Developer Pack/Targeting Pack**, không chỉ cài runtime.

### `MSB3644` hoặc không tìm thấy MSBuild

Cài Visual Studio 2022 Build Tools và workload ASP.NET. Chạy lại `scripts/setup.ps1` từ Developer PowerShell.

### `Could not load file or assembly Oracle.DataAccess`

- Kiểm tra phiên bản `Oracle.DataAccess.dll` khớp Oracle Client.
- Đồng bộ kiến trúc: project/IIS App Pool và Oracle Client đều x64 hoặc đều x86.
- Nếu dùng DLL AMD64, đặt **Enable 32-Bit Applications = False**.
- Kiểm tra binding redirect trong `Web.config`.

### `Could not load file or assembly CrystalDecisions.*`

Cài SAP Crystal Reports Runtime 13.x đúng kiến trúc. Không chỉ copy DLL nếu máy chưa có native runtime.

### `Could not load file or assembly ...` hoặc reference có dấu chấm than

`HintPath` legacy đang trỏ tới thư mục không tồn tại. Cài/copy dependency được cấp phép, sau đó sửa reference thành đường dẫn tương đối. Xóa `bin`/`obj`, restore và build lại.

### NuGet restore báo `NU1101`, `404` hoặc lỗi TLS

```powershell
nuget sources list
dotnet nuget list source
```

Đảm bảo `nuget.org` được bật, proxy cho phép `https://api.nuget.org`, sau đó xóa thư mục `packages` và chạy lại setup.

### HTTP 500.19 trên IIS

- Cài đủ IIS ASP.NET 4.x features.
- Chạy `aspnet_regiis` chỉ với hệ điều hành cũ; Windows mới nên bật feature qua Windows Features.
- Kiểm tra section `system.webServer` có bị khóa ở cấp server không.
- Kiểm tra quyền đọc `Web.config`.

### HTTP 500.21: handler has a bad module

ASP.NET chưa được đăng ký với IIS hoặc App Pool chọn sai CLR. Bật ASP.NET 4.8/.NET Extensibility và dùng CLR v4.0 Integrated.

### HTTP 404.3 với `.aspx`

Thiếu ASP.NET 4.x ISAPI handler. Bật ASP.NET 4.8, ISAPI Extensions và ISAPI Filters trong Windows Features.

### Đăng nhập chậm hoặc không đăng nhập được

- Kiểm tra `applicationpath.api` có truy cập được từ máy IIS.
- Kiểm tra firewall, DNS và timeout API.
- Xem Visual Studio Output với prefix `[Login.Performance]` và `[ATFM.Performance]`.
- Trong DevTools → Network, kiểm tra header `Server-Timing`.

### `Cannot find column [ParrentID]`

Deploy lại DLL mới nhất. Mã menu đã kiểm tra schema API và fallback khi API không trả cột `ParrentID`. Sau deploy, recycle Application Pool để xóa cache assembly/session cũ.

### CSS/JavaScript không cập nhật sau deploy

Static content được cache 30 ngày. Hard refresh (`Ctrl+F5`), xóa browser cache hoặc đổi version query string của asset khi phát hành.

## Kiểm tra trước khi commit

```powershell
.\scripts\setup.ps1 -Build
git status
git diff --check
```

Không commit `bin`, `obj`, `.vs`, `packages`, file upload, file backup, credential hoặc cấu hình production.

## Bảo mật

- Repository hiện là public: tuyệt đối không đưa password, token, private key hoặc địa chỉ nội bộ nhạy cảm vào commit.
- Dùng tài khoản DB có quyền tối thiểu.
- Đổi ngay credential nếu từng bị commit hoặc chia sẻ công khai.
- Review `git diff --cached` trước mỗi lần push.

Thực hiện lấy dữ liệu về từ [colllol/ATFM_WEB.git](https://github.com/colllol/ATFM_WEB.git) nhánh agent/initial-atfm-import

Thực hiện Publish vào thư mục E:\2026\QLB\TrienKhai\ATFM_WEB\prjApplication\bin\Release\PublishOutput
