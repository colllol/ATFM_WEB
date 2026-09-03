# Hướng dẫn sử dụng TracksSyncPython

## 1. Mục đích

`TracksSyncPython` đồng bộ dữ liệu vị trí tàu bay từ bảng PostgreSQL `public.tracks` sang bảng Oracle `T_TRACKS_LOG`.

Trong mỗi lượt chạy, công cụ:

1. Đọc các bản tin vị trí mới từ `public.tracks`.
2. Giữ lại các vị trí nằm trong hoặc trên biên FIR Hà Nội (VVHN) và FIR Hồ Chí Minh (VVHM).
3. Chuẩn hóa callsign và đối chiếu chuyến bay trong `T_DAY_FLIGHTS_GOINGON`.
4. Ghi mới hoặc cập nhật `T_TRACKS_LOG`.
5. Xác minh dữ liệu đã ghi và lưu watermark để lượt sau chỉ xử lý dữ liệu mới.

## 2. Thành phần

| File | Chức năng |
|---|---|
| `main.py` | Mã nguồn chính, hỗ trợ giao diện Windows và dòng lệnh. |
| `TracksSync.sample.json` | Cấu hình mẫu PostgreSQL, Oracle, FIR và watermark. |
| `requirements.txt` | Danh sách thư viện Python cần cài. |
| `TracksSync.watermark.log` | Mốc thời gian đã đồng bộ gần nhất; được tạo/cập nhật khi chạy `sync` hoặc `all`. |
| `ATFM-TracksSync.exe` | Bản giao diện Windows sau khi build. |
| `ATFM-TracksSync-CLI.exe` | Bản dòng lệnh sau khi build. |

## 3. Yêu cầu trước khi chạy

- Windows có Python 3 và `pip` nếu chạy từ mã nguồn.
- Có quyền đọc PostgreSQL, tối thiểu đối với `public.tracks`.
- Có quyền đọc `T_DAY_FLIGHTS_GOINGON` và quyền tạo/sửa/ghi `T_TRACKS_LOG` trên Oracle.
- Có file GeoJSON chứa vùng FIR VVHN và VVHM.
- Máy chạy công cụ kết nối được đến host/port PostgreSQL và Oracle.

Nên dùng tài khoản cơ sở dữ liệu chuyên dụng với đúng quyền cần thiết. Không đưa file cấu hình thật có mật khẩu lên Git.

## 4. Cài đặt để chạy từ mã nguồn

Mở PowerShell tại thư mục gốc project:

```powershell
python -m venv .venv
.\.venv\Scripts\Activate.ps1
python -m pip install --upgrade pip
python -m pip install -r .\tools\TracksSyncPython\requirements.txt
```

Nếu PowerShell chặn script kích hoạt môi trường ảo, có thể chạy trực tiếp:

```powershell
.\.venv\Scripts\python.exe -m pip install -r .\tools\TracksSyncPython\requirements.txt
.\.venv\Scripts\python.exe .\tools\TracksSyncPython\main.py --mode check
```

## 5. Tạo file cấu hình

Sao chép file mẫu và đặt tên `TracksSync.local.json`:

```powershell
Copy-Item .\tools\TracksSyncPython\TracksSync.sample.json .\tools\TracksSyncPython\TracksSync.local.json
```

Cấu hình mẫu:

```json
{
  "postgres": {
    "host": "172.16.1.10",
    "port": 5432,
    "database": "postgres",
    "username": "tracks_reader",
    "password": "MAT_KHAU_POSTGRES"
  },
  "oracle": {
    "host": "172.16.1.20",
    "port": 1521,
    "service_name": "PDBORCL",
    "username": "atfm",
    "password": "MAT_KHAU_ORACLE",
    "flight_table": "T_DAY_FLIGHTS_GOINGON"
  },
  "fir": {
    "geojson_path": "C:\\ATFM\\config\\vietnam-fir-VVHM-VVHN.geojson"
  },
  "watermark_path": "C:\\ATFM\\data\\TracksSync.watermark.log"
}
```

Ý nghĩa các nhóm cấu hình:

- `postgres`: kết nối đến nguồn ADS-B có bảng `public.tracks`.
- `oracle`: kết nối ATFM; `flight_table` mặc định là `T_DAY_FLIGHTS_GOINGON`.
- `fir.geojson_path`: đường dẫn tuyệt đối đến file ranh giới FIR.
- `watermark_path`: nơi lưu thời điểm đã xử lý. Để chuỗi rỗng, công cụ lưu `TracksSync.watermark.log` cạnh `main.py` hoặc cạnh file EXE.

Thứ tự tìm file cấu hình:

1. Đường dẫn trong biến môi trường `TRACKS_CONFIG`.
2. `TracksSync.local.json` cạnh `main.py` hoặc file EXE.
3. `prjApplication\App_Data\TracksSync.local.json` trong project.

Công cụ dùng file đầu tiên tìm thấy. Các giá trị trong file này ghi đè giá trị lấy từ biến môi trường/mặc định.

### Cấu hình bằng biến môi trường

Có thể không dùng file JSON và khai báo các biến sau:

| Biến | Nội dung |
|---|---|
| `TRACKS_PG_HOST`, `TRACKS_PG_PORT` | Host và port PostgreSQL. |
| `TRACKS_PG_DATABASE` | Tên database PostgreSQL. |
| `TRACKS_PG_USERNAME`, `TRACKS_PG_PASSWORD` | Tài khoản PostgreSQL. |
| `TRACKS_ORA_HOST`, `TRACKS_ORA_PORT` | Host và port Oracle. |
| `TRACKS_ORA_SERVICE` | Service name Oracle. |
| `TRACKS_ORA_USERNAME`, `TRACKS_ORA_PASSWORD` | Tài khoản Oracle. |
| `TRACKS_ORA_FLIGHT_TABLE` | Bảng chuyến bay dùng đối chiếu. |
| `TRACKS_FIR_GEOJSON` | Đường dẫn file FIR GeoJSON. |
| `TRACKS_WATERMARK` | Đường dẫn file watermark. |

Ví dụ cho phiên PowerShell hiện tại:

```powershell
$env:TRACKS_CONFIG = 'C:\ATFM\config\TracksSync.local.json'
python .\tools\TracksSyncPython\main.py --mode check
```

## 6. Chạy bằng giao diện Windows

### Từ mã nguồn

```powershell
python .\tools\TracksSyncPython\main.py
```

### Từ file EXE

Mở:

```text
tools\dist\ATFM-TracksSync.exe
```

Các nút trên giao diện:

| Nút | Tác dụng |
|---|---|
| **Kiểm tra đối chiếu** | Đọc nguồn, phân vùng FIR và đối chiếu Oracle nhưng không ghi dữ liệu nghiệp vụ, tương đương `--mode check`. |
| **Ghi T_TRACKS_LOG** | Ghi mới/cập nhật dữ liệu và cập nhật watermark, tương đương `--mode sync`. |
| **Xác minh kết quả** | So sánh dữ liệu dự kiến với dữ liệu hiện có trong `T_TRACKS_LOG`, tương đương `--mode verify`. |
| **Auto** | Lặp quy trình `check -> sync -> verify` thông qua chế độ `all`, theo chu kỳ đã nhập. |
| **Stop** | Yêu cầu dừng Auto. Nếu đang thao tác database, lượt hiện tại hoàn tất an toàn rồi mới dừng. |

`Chu kỳ (giây)` là khoảng nghỉ sau khi một lượt Auto hoàn tất, không phải thời gian tính từ lúc bắt đầu lượt trước. Giá trị phải là số nguyên lớn hơn `0`.

Quy trình vận hành khuyến nghị trên GUI:

1. Bấm **Kiểm tra đối chiếu** và đọc toàn bộ log.
2. Xác nhận đúng file cấu hình, đúng PostgreSQL và không có lỗi FIR/Oracle.
3. Bấm **Ghi T_TRACKS_LOG**.
4. Bấm **Xác minh kết quả**.
5. Chỉ bật **Auto** sau khi một lượt thủ công hoàn tất bình thường.

## 7. Chạy bằng dòng lệnh

### Từ mã nguồn

```powershell
python .\tools\TracksSyncPython\main.py --mode check
python .\tools\TracksSyncPython\main.py --mode sync
python .\tools\TracksSyncPython\main.py --mode verify
python .\tools\TracksSyncPython\main.py --mode all
```

### Từ bản CLI đã build

```powershell
.\tools\dist\ATFM-TracksSync-CLI.exe --mode check
.\tools\dist\ATFM-TracksSync-CLI.exe --mode all
```

Ý nghĩa các mode:

- `check`: chạy thử toàn bộ bước đọc và đối chiếu; không ghi log nghiệp vụ, không cập nhật watermark.
- `sync`: ghi/cập nhật `T_TRACKS_LOG`, sau đó cập nhật watermark.
- `verify`: đọc lại Oracle để thống kê dòng đúng, sai, thiếu, thừa và khóa trùng; không cập nhật watermark.
- `all`: kiểm tra, ghi, xác minh trong một lượt; cập nhật watermark khi hoàn tất bước ghi/xác minh.
- `gui`: mở giao diện, là giá trị mặc định khi không truyền `--mode`.

Lưu ý: `check` và `verify` không ghi dữ liệu nghiệp vụ nhưng quá trình khởi tạo vẫn có thể tạo `T_TRACKS_LOG`, bổ sung cột còn thiếu hoặc tạo unique index phục vụ công cụ.

## 8. Chạy lại toàn bộ dữ liệu

Tham số `--full` bỏ qua watermark và đọc lại toàn bộ `public.tracks`:

```powershell
python .\tools\TracksSyncPython\main.py --mode check --full
python .\tools\TracksSyncPython\main.py --mode sync --full
python .\tools\TracksSyncPython\main.py --mode all --full
```

Chỉ nên dùng `--full` khi:

- chạy lần đầu;
- cần backfill tọa độ cho log cũ;
- watermark sai hoặc bị mất;
- cần tái đối chiếu toàn bộ sau khi sửa dữ liệu nguồn.

Với `sync --full` hoặc `all --full`, công cụ cố gắng điền `LAT/LON` cho các khóa còn phù hợp. Khi không còn dòng thiếu tọa độ, công cụ đặt hai cột thành `NOT NULL`. Nếu vẫn có dữ liệu lịch sử không thể backfill, công cụ giữ nguyên dữ liệu và thông báo số dòng còn thiếu, không tự xóa.

Không nên xóa hoặc sửa watermark trong lúc Auto đang chạy. Trước khi chạy lại toàn bộ trên môi trường thật, nên sao lưu `T_TRACKS_LOG` và kiểm tra trước bằng `check --full`.

## 9. Quy tắc xử lý dữ liệu

- Callsign lấy từ đầu `flight_id_current` đến trước dấu `-`, bỏ khoảng trắng và chuyển thành chữ hoa. Ví dụ `CES6380-2026-07-07` thành `CES6380`.
- Ngày đối chiếu lấy từ `updated_at_utc` và lưu ở dạng `dd-mm-yyyy`.
- Vị trí thuộc VVHN hoặc biên chung VVHN/VVHM có `STATUS = 1`; vị trí thuộc VVHM có `STATUS = 2`.
- Mỗi khóa logic là `CALLSIGN + DATE`; bảng được bảo vệ bằng unique index trên hai trường này.
- Nếu một callsign/ngày có nhiều vị trí, tọa độ và thời gian cập nhật mới nhất được giữ lại.
- `TIME_IN` là lần đầu callsign chạm đường biên của một FIR đơn; `TIME_OUT` là lần chạm đường biên đơn thứ hai. Điểm nằm trong FIR, ngoài FIR hoặc trên đường biên chung VVHN/VVHM không được tính là lần chạm. Các lần chạm đơn tiếp theo bị bỏ qua.
- Không tìm thấy chuyến bay trong bảng đối chiếu vẫn ghi với `PERMTYPE = OTHER`; thông tin hành trình để trống.
- Chuyến bay thiếu `FROM_AIRP` hoặc `TO_AIRP` cũng được ghi với `PERMTYPE = OTHER`.
- Nếu có nhiều chuyến cùng callsign/ngày, công cụ chọn duy nhất dòng có khoảng `ETD-ETA` chứa `updated_at_utc`; khoảng qua nửa đêm được hỗ trợ. Không chọn được duy nhất thì bỏ qua khóa đó.
- Khi khóa đã tồn tại, công cụ cập nhật vị trí/thời gian mới nhất và giữ các mốc FIR đã có. Dòng `OTHER` có thể được bổ sung thông tin chuyến bay ở lần đồng bộ sau.

## 10. Đọc log kết quả

Các dòng quan trọng thường gặp:

```text
Cấu hình: C:\ATFM\config\TracksSync.local.json
PostgreSQL: 172.16.1.10:5432/postgres
Đọc public.tracks: 120 dòng nằm trong FIR cần đối chiếu.
Cần đối chiếu Oracle: 35 callsign/ngày.
Match T_DAY_FLIGHTS_GOINGON: 30; không match: 3; bỏ qua: 2.
Đã thêm mới/cập nhật tọa độ 33 dòng T_TRACKS_LOG.
Watermark mới: 2026-08-27 09:15:30
```

Trong đó:

- `match`: tìm được đúng chuyến bay Oracle.
- `không match`: vẫn ghi dưới loại `OTHER`.
- `bỏ qua`: thường do nhiều chuyến trùng callsign/ngày nhưng không chọn được duy nhất theo `ETD-ETA`.
- thống kê xác minh `đúng/sai/thiếu/thừa/khóa trùng`: dùng để quyết định lượt ghi có đạt yêu cầu hay không.

Một lượt được coi là ổn khi không có traceback, số `sai`, `thiếu`, `khóa trùng` bằng `0`, và watermark mới xuất hiện sau `sync`/`all`.

## 11. Kiểm tra trực tiếp trên Oracle

Ví dụ kiểm tra các bản ghi mới nhất:

```sql
SELECT CALLSIGN, "DATE", FROM_AIRP, TO_AIRP,
       ETD, ETA, STATUS, UPDATED_AT_UTC, PERMTYPE,
       TIME_IN, TIME_OUT, LAT, LON
FROM T_TRACKS_LOG
ORDER BY UPDATED_AT_UTC DESC
FETCH FIRST 50 ROWS ONLY;
```

Kiểm tra khóa trùng:

```sql
SELECT CALLSIGN, "DATE", COUNT(*) AS TOTAL
FROM T_TRACKS_LOG
GROUP BY CALLSIGN, "DATE"
HAVING COUNT(*) > 1;
```

Kiểm tra tọa độ còn thiếu:

```sql
SELECT COUNT(*) AS TOTAL_MISSING_COORDINATES
FROM T_TRACKS_LOG
WHERE LAT IS NULL OR LON IS NULL;
```

## 12. Xử lý sự cố

### Báo cấu hình PostgreSQL chưa đầy đủ

- Kiểm tra log `Cấu hình:` có trỏ đúng file mong muốn không.
- Bổ sung `host`, `username`, `password` trong nhóm `postgres`.
- Kiểm tra JSON không có dấu phẩy thừa và dùng dấu `\\` trong đường dẫn Windows.

### Không kết nối được PostgreSQL hoặc Oracle

```powershell
Test-NetConnection 172.16.1.10 -Port 5432
Test-NetConnection 172.16.1.20 -Port 1521
```

Nếu port thông nhưng vẫn lỗi, kiểm tra database/service name, tài khoản, mật khẩu, quyền truy cập và chính sách firewall từ máy chạy công cụ.

### Không tìm thấy file FIR GeoJSON

- Kiểm tra `fir.geojson_path` là đường dẫn tuyệt đối và file tồn tại.
- Tài khoản chạy EXE phải có quyền đọc file/thư mục.
- Không đổi tên thuộc tính/vùng trong GeoJSON nếu chưa kiểm tra lại cách công cụ nhận diện VVHN/VVHM.

### Watermark không đúng định dạng

Watermark hợp lệ có dạng:

```text
2026-08-27 09:15:30
```

Dừng Auto, sao lưu file watermark, sau đó sửa đúng định dạng hoặc chuyển file cũ sang tên khác và chạy `check --full` trước khi đồng bộ lại.

### Không có dòng mới để xử lý

- Kiểm tra watermark có mới hơn dữ liệu trong `public.tracks` không.
- Kiểm tra `updated_at_utc`, `last_lat`, `last_lon` của nguồn.
- Kiểm tra các tọa độ có thực sự nằm trong FIR.
- Dùng `check --full` để phân biệt lỗi watermark với lỗi dữ liệu/phân vùng.

### Có nhiều dòng bị bỏ qua

Đối chiếu các chuyến trùng `FLIGHTNBR + FLIGHTDATE` trong `T_DAY_FLIGHTS_GOINGON`. Đảm bảo `ETD`, `ETA` hợp lệ và chỉ một khoảng thời gian chứa thời điểm track tương ứng.

### EXE chạy nhưng không thấy cửa sổ dòng lệnh

`ATFM-TracksSync.exe` là bản GUI. Muốn dùng trong script hoặc Task Scheduler, sử dụng `ATFM-TracksSync-CLI.exe`.

## 13. Build file EXE

Tại thư mục gốc project:

```powershell
python -m pip install -r .\tools\TracksSyncPython\requirements.txt
.\tools\build_tracks_sync.ps1
```

Kết quả:

```text
tools\dist\ATFM-TracksSync.exe
tools\dist\ATFM-TracksSync-CLI.exe
```

File trung gian PyInstaller nằm trong `tools\build`. Sau khi build, nên chạy bản CLI với `--mode check` trên cấu hình triển khai trước khi bật GUI/Auto.

## 14. Gợi ý chạy bằng Task Scheduler

Dùng bản CLI và mode `all`, ví dụ action:

```text
Program/script: C:\ATFM\ATFM-TracksSync-CLI.exe
Arguments: --mode all
Start in: C:\ATFM
```

Đặt `TracksSync.local.json` cạnh EXE hoặc khai báo `TRACKS_CONFIG` cho đúng tài khoản chạy task. Không cấu hình nhiều task chạy chồng lấn trên cùng bảng và cùng watermark.

## 15. Checklist vận hành

### Trước khi chạy lần đầu

- [ ] Đã tạo `TracksSync.local.json` và không đưa mật khẩu lên Git.
- [ ] Kết nối được PostgreSQL port 5432 và Oracle port 1521 (hoặc port thực tế).
- [ ] Tài khoản có đủ quyền đọc/ghi cần thiết.
- [ ] File FIR GeoJSON tồn tại và đọc được.
- [ ] Đã chạy `--mode check` thành công.
- [ ] Đã sao lưu `T_TRACKS_LOG` nếu chuẩn bị chạy `--full` trên dữ liệu thật.

### Theo dõi định kỳ

- [ ] Log không có traceback.
- [ ] Số dòng `sai`, `thiếu`, `khóa trùng` bằng `0`.
- [ ] Watermark tiếp tục tăng.
- [ ] Dung lượng bảng `T_TRACKS_LOG` và file log còn trong giới hạn vận hành.
- [ ] Không có nhiều khóa bị bỏ qua do trùng khoảng `ETD-ETA`.
