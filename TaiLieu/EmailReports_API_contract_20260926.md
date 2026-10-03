# Hop dong API EmailReports (ban giao backend)

Ngay: 2026-09-26. Day la **yeu cau trien khai**, chua phai tinh nang dang chay tren web.

## 1. Khong them Web.config key

Web da co `APIEmail = http://172.29.79.49:8081/api/reports/emails`,
`APIIncoming = http://172.29.79.49:8081/api/reports/incoming`, va
`APIEmailJobs = http://172.29.79.49:8081/api/jobs/`.

Web tiep tuc goi hai URL danh sach da cau hinh. URL thong ke duoc tao bang cach
**them `/summary` vao phan path** cua `APIEmail` hoac `APIIncoming` (URI builder,
truoc query string, cung origin); URL chi tiet them `/{id}` vao path. Khong them
key `APIEmailSummary`, `APIIncomingSummary` hay `APIIncomingDetail`.

Luot tim kiem: lay tong hop sau loc va duy nhat trang 0 voi 25/50/100 dong.
Chuyen trang/doi kich thuoc: chi lay trang dang xem. Bam Xem: moi lay chi
tiet email va job neu can. Khong quet het trang hoac goi job tung dong de dem KPI.

## 2. Mo rong GET {APIEmail} va GET {APIIncoming}

Ca hai API danh sach phai nhan cung bo tham so:

- `query` (tuy chon): tim khong phan biet hoa/thuong theo subject/sender/body
  neu co luu; backend xac nhan chinh xac cac truong duoc tim kiem.
- `processingStatus` (tuy chon): khop chinh xac ma trang thai.
- `from`, `to` (tuy chon): moc ngay/gio ISO-8601, bao gom ca hai dau.
  Web hien gui `YYYY-MM-DDT00:00:00` va `YYYY-MM-DDT23:59:59`.
  Backend/web phai thong nhat timezone, khong lech ngay giua list/summary.
- `fileType` (MOI, tuy chon): `word`, `excel`, `pdf`, `image`, `none`;
  bo trong = tat ca. Mac dinh tren UI hien la Word.
- `page` zero-based; `size` 25/50/100, gioi han toi da 100.

Loc file theo `storedFileName`, neu rong thi `attachmentName`. Mo rong
khong phan biet hoa/thuong: word=doc/docx/rtf;
excel=xls/xlsx/xlsm/csv; pdf=pdf;
image=png/jpg/jpeg/gif/bmp/tif/tiff/webp; none=khong co ten file.
Duoi khac khong thuoc cac nhom tren. Backend can dung cung mot quy tac loc
trong list va summary. **Loc tat ca dieu kien truoc COUNT va LIMIT/OFFSET.**
Incoming hien chi cong bo `page`, `size`: can bo sung bo loc thuc su.

```http
GET {APIEmail}?query=ABC&processingStatus=SAVED&fileType=word&from=2026-09-20T00%3A00%3A00&to=2026-09-20T23%3A59%3A59&page=0&size=50
GET {APIIncoming}?fileType=word&page=1&size=50
```

Giu schema dang co `content`, `page`, `size`, `totalElements`,
`totalPages`, `hasNext`, `hasPrevious` va cac truong email hien co.
`totalElements` dem toan bo tap **sau loc, truoc phan trang**. Vi du co
120 file Word: size=50 thi trang 0/1 moi trang 50 dong, trang 2 la 20;
`totalElements=120` o ca 3 trang. Sap xep on dinh (vi du
`receivedAt DESC, id DESC`), co khoa duy nhat khi trung thoi gian.

Moi item can co `databaseState` va `hasDatabaseData` de web to mau dong
ma khong phai goi `/jobs/{id}` cho moi item:

```json
{"id":101,"syncJobId":"job-101","subject":"ABC",
 "storedFileName":"abc.docx","processingStatus":"SAVED",
 "databaseState":"found","hasDatabaseData":true}
```

`databaseState`: `found`, `missing`, `error`, `pending`, `unknown`.
`found` CHI khi phep bay thuc su ton tai trong DB ma web hien dang kiem tra
(bang SC hoac NO). Co `targetPermId` hoac job `SUCCESS` **chua du** de ket
luan DB co ban ghi. Backend can xac nhan cach truy cap/dong bo trang thai
voi DB do; neu khong kiem chung duoc thi tra `unknown`, khong tra `found`.
Neu chua the them truong nay, web khong the bao dam KPI/mau dong chinh xac
ma khong goi tung job va kiem tra DB.

## 3. Summary tren cung URL goc

- Mo rong `GET {APIEmail}/summary` (hien da co) cho cung bo loc
  `query`, `processingStatus`, `from`, `to`, `fileType`.
- Bo sung `GET {APIIncoming}/summary` voi dung schema/bo loc tuong tu.
- Khong can `page`/`size` cho summary. Neu can `sender` rieng thi bo loc nay
  phai duoc ap dung giong nhau tren ca list lan summary.
- Dung chung predicate loc va nguon kiem chung DB voi list.

`/emails/summary` dang chay hien chi nhan `from`/`to`, tra `totalRecords`,
`processingStatusCounts`, `acknowledgementStatusCounts`; **khong** phai
so file co/khong co du lieu DB sau tat ca bo loc. Giu cac truong cu cho
client khac, bo sung:

```json
{"totalFiles":120,"successFiles":65,"errorFiles":43,
 "pendingFiles":9,"unknownFiles":3,"asOf":"2026-09-26T10:15:30+07:00"}
```

- `successFiles`: `found`, da xac nhan phep bay o bang SC/NO.
- `errorFiles`: `missing` (da xac minh khong co phep bay) hoac `error`
  (loi xu ly da xac minh). **Khong** dem loi mang/DB tra cuu vao day.
- `pendingFiles`: worker/job chua xong, chua the ket luan.
- `unknownFiles`: khong xac minh duoc DB, gom ca loi tra cuu.
- Bat buoc `totalFiles = successFiles + errorFiles + pendingFiles +
  unknownFiles`; voi cung snapshot va bo loc, `totalFiles` bang
  `list.totalElements`.

Du lieu co the thay doi giua hai request. De nghiem thu doi chieu chinh xac,
backend nen chon mot trong hai: (a) `snapshotId` tu summary va list nhan lai
`snapshotId`; hoac (b) nhung summary vao chinh response list de loc va dem
trong cung snapshot (van co the giu `/summary` cho count-only). Ghi ro
phuong an backend chon. Khi DB/count loi, tra HTTP error ro rang,
**khong tra tong bang 0 gia**.

## 4. Chi tiet va tuong thich

- `GET {APIEmail}/{id}` da co: giu de tai body/chi tiet luc bam Xem.
- Neu list Incoming khong co du chi tiet, bo sung `GET {APIIncoming}/{id}`;
  suy URL tu `APIIncoming`, khong them Web.config key.
- `APIEmailJobs` tiep tuc dung khi mo chi tiet phep bay, khong dung de tinh
  KPI tren ca danh sach.
- Khong thay doi behavior cu khi khong truyen tham so moi. Khong tra
  credential trong response.
- Sau khi backend deploy, web can thay `loadAllItems` hien tai bang goi
  trang dang xem va summary. **Commit web `21a7456` van dang tai het trang**;
  tai lieu nay khong khang dinh web da su dung API moi.

## 5. Dieu kien nghiem thu backend

1. Cung bo loc/snapshot: `summary.totalFiles == list.totalElements`
   cho ca Email va Incoming.
2. Co >=100 file Word: `page=0&size=50` va `page=1&size=50` deu co
   chinh xac 50 file Word; khong bo sot Word o cac trang unfiltered sau.
3. Ket hop query + status + ngay + fileType; test ranh gioi ngay,
   duoi file viet hoa, `none`, duoi la va tap rong.
4. Fixture du 5 trang thai DB, co ca `targetPermId` khong ton tai trong
   SC/NO (phai la `missing`). Pending/loi tra cuu khong cong vao error.
5. Sap xep on dinh, khong trung/bo sot giua cac trang; loi DB/count phai
   tra loi thay vi so 0 hop le gia.
