# Hướng dẫn triển khai ATFM Web và API lên IIS

## 1. Mô hình khuyến nghị

```text
Internet
   │ HTTPS 443
   ▼
IIS - Website ATFM
   └── /atfm  → prjApplication

IIS - Website API nội bộ
   └── http://127.0.0.1:5080 → QLB.API
             │
             ▼
         Oracle Database
```

Chỉ website `/atfm` nên được public Internet. API nên chạy nội bộ trên cùng server và website gọi API qua `127.0.0.1:5080`.

Không nên public API trực tiếp ở trạng thái hiện tại vì:

- CORS đang cho phép mọi domain (`*`).
- Global authorization đang bị comment.
- Có nhiều endpoint thao tác và xóa dữ liệu.
- Chuỗi kết nối database đang lưu trực tiếp trong `Web.config`.

## 2. Chuẩn bị máy chủ

Khuyến nghị:

- Windows Server 2019, 2022 hoặc mới hơn.
- RAM tối thiểu 8 GB.
- Ổ đĩa trống tối thiểu 20 GB.
- IP tĩnh.
- Có domain, ví dụ `atfm.congty.vn`.
- Server kết nối được Oracle Database qua cổng `1521`.

Trong Server Manager, chọn `Add Roles and Features`, cài `Web Server (IIS)` và bật:

```text
Web Server
├── Common HTTP Features
│   ├── Default Document
│   ├── Static Content
│   ├── HTTP Errors
│   └── HTTP Redirection
├── Application Development
│   ├── ASP.NET 4.8
│   ├── .NET Extensibility 4.8
│   ├── ISAPI Extensions
│   └── ISAPI Filters
├── Security
│   └── Request Filtering
└── Management Tools
    └── IIS Management Console
```

Cài thêm:

- .NET Framework 4.8 Runtime.
- Visual C++ Redistributable x64 nếu Oracle yêu cầu.
- Oracle ODAC 12.1 x64 tương thích `Oracle.DataAccess 4.121.2.0`.

## 3. Việc bảo mật bắt buộc trước khi public

### 3.1. Đổi mật khẩu Oracle

Mật khẩu Oracle hiện đang được lưu trực tiếp trong một số `Web.config`. Cần:

1. Đổi mật khẩu tài khoản Oracle hiện tại.
2. Không commit mật khẩu mới vào Git.
3. Chỉ nhập chuỗi kết nối production trên máy chủ.
4. Giới hạn quyền tài khoản Oracle ở mức cần thiết.

### 3.2. Tắt debug

Trong `Web.config` production:

```xml
<compilation debug="false" targetFramework="4.8" />
```

Không dùng:

```xml
<customErrors mode="Off" />
```

Nên dùng:

```xml
<customErrors mode="RemoteOnly" defaultRedirect="~/Errors/ErrorPage.htm" />
```

### 3.3. Giới hạn CORS API

Nếu trình duyệt cần gọi API trực tiếp, chỉ cho phép domain website:

```csharp
var cors = new EnableCorsAttribute(
    "https://atfm.congty.vn",
    "*",
    "GET,POST,PUT,DELETE"
);
```

Nếu website chỉ gọi API từ code C# phía server thì có thể bỏ CORS.

### 3.4. Không publish file sao lưu

Loại các file sau khỏi gói production:

```text
* - Copy.aspx
* - Copy.aspx.cs
* - Copy.aspx.designer.cs
Masters/* - Copy.Master
```

Không đưa lên server source `.cs`, `.sln`, `.csproj`, thư mục `.git`, `.vs` và `packages`.

## 4. Publish API

1. Mở:

   ```text
   C:\Users\Admin\Documents\GitHub\API\QLB.API.sln
   ```

2. Chọn cấu hình `Release` và `Any CPU`.
3. Chuột phải project `QLB.API` → `Publish`.
4. Chọn `Folder`.
5. Chọn thư mục:

   ```text
   C:\Publish\ATFM_API
   ```

6. Nhấn `Publish`.

Kiểm tra kết quả phải có:

```text
C:\Publish\ATFM_API
├── bin
│   ├── QLB.API.dll
│   ├── QLB.BusinessLogic.dll
│   ├── Oracle.DataAccess.dll
│   ├── Oracle.ManagedDataAccess.dll
│   └── ...
├── Web.config
├── Global.asax
└── ...
```

## 5. Publish website ATFM

1. Mở:

   ```text
   C:\Users\Admin\Documents\APP_V2\APP\ATFM.sln
   ```

2. Chọn `Release` và `Any CPU`.
3. Chuột phải project `prjApplication` → `Publish`.
4. Chọn `Folder`.
5. Chọn:

   ```text
   C:\Publish\ATFM_WEB
   ```

6. Nhấn `Publish`.

Kiểm tra kết quả:

```text
C:\Publish\ATFM_WEB
├── bin
│   ├── prjApplication.dll
│   ├── prjBusinessLogic.dll
│   ├── prjComponents.dll
│   ├── Newtonsoft.Json.dll
│   ├── Oracle.DataAccess.dll
│   └── ...
├── Masters
├── Static
├── Style
├── Scripts
├── Web.config
└── Global.asax
```

## 6. Copy bản publish lên server

Tạo thư mục:

```text
C:\Sites\ATFM\Web
C:\Sites\ATFM\Api
C:\Sites\ATFM\Backup
```

Copy:

```text
C:\Publish\ATFM_WEB → C:\Sites\ATFM\Web
C:\Publish\ATFM_API → C:\Sites\ATFM\Api
```

Không chạy IIS trực tiếp từ thư mục source hoặc GitHub.

Khi cập nhật phiên bản mới:

1. Sao lưu thư mục đang chạy.
2. Dừng Application Pool.
3. Copy bản publish mới.
4. Kiểm tra lại `Web.config` production.
5. Khởi động Application Pool.

## 7. Tạo Application Pool

### API Pool

```text
Name: ATFM-API-Pool
.NET CLR version: v4.0
Managed pipeline mode: Integrated
Enable 32-Bit Applications: False
Start Mode: AlwaysRunning
```

### Web Pool

```text
Name: ATFM-Web-Pool
.NET CLR version: v4.0
Managed pipeline mode: Integrated
Enable 32-Bit Applications: False
```

Không dùng chung Application Pool cho Web và API.

`Enable 32-Bit Applications` phải là `False` vì project tham chiếu `Oracle.DataAccess` AMD64/x64.

## 8. Tạo website API nội bộ

Trong IIS Manager:

1. Chuột phải `Sites` → `Add Website`.
2. Nhập:

   ```text
   Site name: ATFM-API
   Application Pool: ATFM-API-Pool
   Physical path: C:\Sites\ATFM\Api
   Binding type: http
   IP address: 127.0.0.1
   Port: 5080
   Host name: để trống
   ```

3. Kiểm tra trên server:

   ```powershell
   Invoke-WebRequest http://127.0.0.1:5080/
   ```

4. Nếu có Help Page:

   ```text
   http://127.0.0.1:5080/Help
   ```

Không mở port `5080` ra Internet.

## 9. Cấu hình website gọi API

Trong `C:\Sites\ATFM\Web\Web.config`, cấu hình:

```xml
<add key="applicationpath.api" value="http://127.0.0.1:5080/" />
```

Không dùng URL API development sau khi đưa lên production.

## 10. Tạo website public

Tạo thư mục site gốc:

```text
C:\Sites\ATFM\Root
```

Trong IIS tạo website:

```text
Site name: ATFM-PROD
Physical path: C:\Sites\ATFM\Root
Port: 80
Host name: atfm.congty.vn
```

Sau đó chuột phải `ATFM-PROD` → `Add Application`:

```text
Alias: atfm
Application Pool: ATFM-Web-Pool
Physical path: C:\Sites\ATFM\Web
```

URL website:

```text
https://atfm.congty.vn/atfm/
```

Trong `Web.config` website:

```xml
<add key="ApplicationPath" value="https://atfm.congty.vn/atfm" />
```

Không để `http://localhost/atfm`, vì trên máy người dùng, `localhost` chính là máy của họ.

## 11. Cấp quyền thư mục

Cấp `Read`, `Read & Execute`, `List folder contents` cho:

```text
IIS AppPool\ATFM-Web-Pool
IIS AppPool\ATFM-API-Pool
```

Chỉ cấp `Modify` cho các thư mục cần ghi:

```text
Uploads
App_Data
Logs
Temp
```

Không cấp `Everyone: Full Control` cho toàn bộ website.

## 12. Cấu hình Oracle

Do project có cả `Oracle.ManagedDataAccess` và `Oracle.DataAccess`, server có thể cần Oracle Client/ODAC x64 đúng phiên bản.

Kiểm tra kết nối Oracle:

```powershell
Test-NetConnection <IP_ORACLE> -Port 1521
```

Không mở cổng Oracle `1521` ra Internet.

Các lỗi thường gặp:

| Lỗi | Nguyên nhân thường gặp |
|---|---|
| `BadImageFormatException` | App Pool chạy sai 32/64-bit |
| `Could not load Oracle.DataAccess` | Thiếu hoặc sai phiên bản ODAC |
| `ORA-12154` | Sai Data Source hoặc TNS |
| `ORA-12541` | Không kết nối được Oracle Listener |

## 13. Domain, firewall và HTTPS

Tạo DNS:

```text
Type: A
Name: atfm
Value: IP public của server
```

Nếu có router/firewall:

```text
Internet TCP 80  → Server TCP 80
Internet TCP 443 → Server TCP 443
```

Không public các port:

```text
5080
1521
3389
```

Trong IIS → site `ATFM-PROD` → `Bindings` → `Add`:

```text
Type: https
Port: 443
Host name: atfm.congty.vn
SSL certificate: chứng chỉ tương ứng
```

Chỉ chuyển hướng HTTP sang HTTPS sau khi HTTPS đã chạy ổn định.

## 14. Kiểm tra sau triển khai

### API nội bộ

```powershell
Invoke-WebRequest http://127.0.0.1:5080/
```

### Website trên server

```text
http://localhost/atfm/login.aspx
```

### Website theo domain

```text
https://atfm.congty.vn/atfm/login.aspx
```

Kiểm tra lần lượt:

1. Trang đăng nhập mở được.
2. Đăng nhập thành công.
3. Menu tải được.
4. Mở một trang danh sách.
5. Thử thêm/sửa/xóa bằng tài khoản thử nghiệm.
6. Kiểm tra upload và export Excel.
7. Đăng xuất.
8. Test trên máy khác ngoài server.
9. Khởi động lại server và kiểm tra website tự chạy.

## 15. Xử lý lỗi IIS thường gặp

### HTTP 500.19

- Thiếu ASP.NET 4.8 trong IIS.
- Cấu hình `Web.config` không được hỗ trợ.
- Thiếu quyền đọc thư mục.

### Could not load type `prjComponents.Global`

Kiểm tra:

```text
C:\Sites\ATFM\Web\bin\prjComponents.dll
```

### Newtonsoft.Json manifest mismatch

- Chỉ để một `Newtonsoft.Json.dll` trong `bin`.
- Binding redirect phải khớp phiên bản DLL publish.

### HTTP 403.14

- Thư mục phải được tạo thành IIS Application, không chỉ là Virtual Directory.
- ASP.NET 4.8 phải được bật.
- Kiểm tra URL `login.aspx`.

### HTTP 404 API

- `QLB.API` phải là IIS Website/Application.
- App Pool dùng CLR v4.0 Integrated.
- URL API có dạng:

  ```text
  http://127.0.0.1:5080/api/TenController/TenAction
  ```

### Xem log lỗi

Event Viewer:

```text
Event Viewer → Windows Logs → Application
```

IIS log:

```text
C:\inetpub\logs\LogFiles
```

## 16. Checklist trước khi mở Internet

- [ ] Đã đổi mật khẩu Oracle đang có trong source.
- [ ] `debug="false"`.
- [ ] `customErrors` không để `Off`.
- [ ] Không publish file `- Copy`.
- [ ] Không publish source `.cs`, `.sln`, `.csproj`, `.git`.
- [ ] API chỉ binding `127.0.0.1:5080`.
- [ ] Không mở Oracle `1521` ra Internet.
- [ ] Không mở RDP `3389` cho toàn Internet.
- [ ] Website chạy HTTPS.
- [ ] CORS không để `*`.
- [ ] Có backup thư mục web và database.
- [ ] Application Pool chạy 64-bit.
- [ ] Đã kiểm tra đăng nhập, menu, export và upload.
- [ ] Đã thử restart server.

## 17. Tài liệu tham khảo

- [Publish an ASP.NET web app - Microsoft Learn](https://learn.microsoft.com/en-us/visualstudio/deployment/quickstart-deploy-aspnet-web-app?view=visualstudio)
- [ASP.NET Web Deployment to IIS](https://learn.microsoft.com/en-us/aspnet/web-forms/overview/deployment/visual-studio-web-deployment/deploying-to-iis)
- [Create an IIS Website](https://learn.microsoft.com/en-us/iis/get-started/getting-started-with-iis/create-a-web-site)
- [IIS Bindings](https://learn.microsoft.com/en-us/iis/configuration/system.applicationhost/sites/site/bindings/)

