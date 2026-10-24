using prjBusinessLogic;
using prjComponents;
using prjInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Services;

namespace prjApplication.Tool
{
    public partial class ImportsPermSC_LD_V2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.FillDropdownList(ddlAuthorV2, new FpAuthorDAL().GetAllObject(), "AUTHOR_CODE", "AUTHOR_CODE");
            this.FillDropdownList(ddlPurposeV2, new FlyPurposeDAL().GetAllObject(), "PURPOSE_CODE", "PURPOSE_CODE");
        }

        [WebMethod(EnableSession = true)]
        public static PermitCheckResult CheckPermitNumber(ImportRequest request)
        {
            var result = new PermitCheckResult();
            try
            {
                ValidateRequestHeader(request);
                DataTable table = FindExistingPermits(new clsResuftAPI(), request);
                result.Success = true;
                result.Exists = table != null && table.Rows.Count > 0;
                if (result.Exists)
                {
                    result.PermitNumbers = table.AsEnumerable()
                        .Select(row => Cell(row, "PERMNBR_ID"))
                        .Where(value => !String.IsNullOrWhiteSpace(value)).Distinct().ToList();
                    result.Message = "Số phép đã tồn tại: " + String.Join(", ", result.PermitNumbers.ToArray())
                        + ". Bạn có muốn tiếp tục đưa dữ liệu vào danh sách hủy không?";
                }
                else result.Message = "Chưa có số phép này.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public static ImportResult ImportRows(ImportRequest request)
        {
            var result = new ImportResult();
            try
            {
                ValidateRequest(request);
                var api = new clsResuftAPI();
                DataTable existingPermits = FindExistingPermits(api, request);
                if (existingPermits != null && existingPermits.Rows.Count > 0 && !request.AllowExistingPermit)
                    throw new InvalidOperationException("Số phép đã tồn tại. Cần xác nhận trước khi tiếp tục.");
                var selectedRows = request.Rows.Where(x => x != null && x.Selected).ToList();
                string currentUser = CurrentUserName();
                string batchId = Guid.NewGuid().ToString("N");
                result.Total = selectedRows.Count;

                foreach (var row in selectedRows)
                {
                    try
                    {
                        ValidateRow(row);
                        object response = api.GetPostValueApiExtension(
                            "PERM_IMP_V2_PKG",
                            "INSERT_CANCELLATION",
                            BuildCancellationPayload(request, row, currentUser, batchId));

                        int code;
                        if (!Int32.TryParse(Convert.ToString(response, CultureInfo.InvariantCulture), out code) || code <= 0)
                            throw new InvalidOperationException("Procedure trả về " + Convert.ToString(response, CultureInfo.InvariantCulture));

                        result.Imported++;
                        row.StagingId = code;
                        result.ImportedRows.Add(row);
                    }
                    catch (Exception ex)
                    {
                        result.Failed++;
                        result.Errors.Add("Dòng " + row.SourceLine + " - " + (row.Callsign ?? "") + ": " + ex.Message);
                    }
                }

                if (result.Failed == 0 && result.Imported > 0)
                    CheckExistingPermissions(api, selectedRows, result, currentUser);

                result.RequiresReview = result.Success;
                result.Message = result.Success
                    ? "Kiểm tra thành công: " + result.Imported + " dòng đã được đưa vào danh sách hủy và đều tìm thấy phép tương ứng."
                    : "Kiểm tra không thành công. Vui lòng sửa dữ liệu lỗi tại bước 2 trước khi tiếp tục.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public static ApplyResult ApplyCancellation(long[] stagingIds)
        {
            var result = new ApplyResult();
            try
            {
                if (stagingIds == null || stagingIds.Length == 0)
                    throw new ArgumentException("Không có staging ID để xác nhận hủy chuyến.");

                string stagingIdList = String.Join(",", stagingIds
                    .Where(id => id > 0)
                    .Distinct()
                    .Select(id => id.ToString(CultureInfo.InvariantCulture))
                    .ToArray());

                if (String.IsNullOrWhiteSpace(stagingIdList))
                    throw new ArgumentException("Danh sách staging ID không hợp lệ.");

                object value = new clsResuftAPI().GetValueApiExtension(
                    "PERM_IMP_V2_PKG",
                    "APPLY_CANCELLATIONS",
                    new { P_CREATED_BY = CurrentUserName(), P_STAGING_IDS = stagingIdList });
                int code;
                result.Success = Int32.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), out code) && code == 1;
                result.Message = result.Success ? "Hủy chuyến thành công. Quy trình đã hoàn tất."
                    : "Hủy chuyến không thành công. Mã trả về: " + Convert.ToString(value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public static ApplyResult DeleteCancellation(long[] stagingIds)
        {
            var result = new ApplyResult();
            try
            {
                string stagingIdList = StagingIdList(stagingIds);
                object value = new clsResuftAPI().GetValueApiExtension(
                    "PERM_IMP_V2_PKG", "DELETE_CANCELLATIONS",
                    new { P_CREATED_BY = CurrentUserName(), P_STAGING_IDS = stagingIdList });
                int code;
                result.Success = Int32.TryParse(
                    Convert.ToString(value, CultureInfo.InvariantCulture), out code) && code == 1;
                result.Message = result.Success
                    ? "Đã xóa các chuyến hủy đã chọn khỏi danh sách chờ xử lý."
                    : "Xóa danh sách HỦY CHUYẾN không thành công. Mã trả về: "
                        + Convert.ToString(value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public static PendingResult SearchPending(PendingSearch request)
        {
            var result = new PendingResult();
            try
            {
                request = request ?? new PendingSearch();
                DataTable table = new clsResuftAPI().GetTableApiExtension(
                    "PERM_IMP_V2_PKG", "SEARCH_PENDING", new
                    {
                        P_CREATED_BY = CurrentUserName(),
                        P_PERMNBR = Clean(request.PermNbr),
                        P_CALLSIGN = Clean(request.Callsign),
                        P_FROMDATE = OptionalOracleDate(request.FromDate),
                        P_TODATE = OptionalOracleDate(request.ToDate),
                        P_STAGING_IDS = null as string
                    });

                if (table == null)
                    throw new InvalidOperationException(
                        "Khong nhan duoc danh sach cho xu ly tu API.");

                result.Rows = table.AsEnumerable().Select(ToPendingRow).ToList();
                result.Success = true;
                result.Message = "Tim thay " + result.Rows.Count
                    + " dong dang cho xu ly.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        public static ImportResult ReviewPending(long[] stagingIds)
        {
            var result = new ImportResult();
            try
            {
                string idList = StagingIdList(stagingIds);
                string currentUser = CurrentUserName();
                var api = new clsResuftAPI();

                object refreshed = api.GetValueApiExtension(
                    "PERM_IMP_V2_PKG", "REFRESH_VALIDATION",
                    new { P_CREATED_BY = currentUser, P_STAGING_IDS = idList });
                if (Convert.ToString(refreshed, CultureInfo.InvariantCulture) != "1")
                    throw new InvalidOperationException(
                        "Khong the kiem tra lai du lieu huy cho xu ly.");

                DataTable staged = api.GetTableApiExtension(
                    "PERM_IMP_V2_PKG", "SEARCH_PENDING", new
                    {
                        P_CREATED_BY = currentUser,
                        P_PERMNBR = null as string,
                        P_CALLSIGN = null as string,
                        P_FROMDATE = null as string,
                        P_TODATE = null as string,
                        P_STAGING_IDS = idList
                    });
                if (staged == null)
                    throw new InvalidOperationException(
                        "Khong doc duoc du lieu huy cho xu ly.");

                result.ImportedRows = staged.AsEnumerable()
                    .Select(ToImportRow).ToList();
                result.Total = stagingIds.Where(x => x > 0).Distinct().Count();
                result.Imported = result.ImportedRows.Count;
                CheckExistingPermissions(
                    api, result.ImportedRows, result, currentUser);
                result.RequiresReview = result.Success;
                result.Message = result.Success
                    ? "Kiem tra thanh cong " + result.Imported
                        + " dong. Co the xac nhan huy chuyen."
                    : "Co du lieu khong con khop voi phep SC. "
                        + "Vui long kiem tra cac dong loi.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        private static void CheckExistingPermissions(
            clsResuftAPI api, IList<ImportRow> selectedRows,
            ImportResult result, string currentUser)
        {
            string idList = StagingIdList(selectedRows
                .Select(x => (long)x.StagingId).ToArray());
            object validation = api.GetValueApiExtension(
                "PERM_IMP_V2_PKG", "REFRESH_VALIDATION",
                new { P_CREATED_BY = currentUser, P_STAGING_IDS = idList });
            if (Convert.ToString(validation, CultureInfo.InvariantCulture) != "1")
                throw new InvalidOperationException(
                    "Khong the cap nhat ket qua kiem tra staging V2.");

            DataTable table = api.GetTableApiExtension(
                "PERM_IMP_V2_PKG", "GET_CANCELLATION_MATCHES",
                new { P_CREATED_BY = currentUser, P_STAGING_IDS = idList });
            if (table == null) throw new InvalidOperationException("Không nhận được kết quả kiểm tra phép từ API.");

            int matchedSources = 0;
            foreach (ImportRow source in selectedRows)
            {
                string stagingId = source.StagingId.ToString(CultureInfo.InvariantCulture);
                List<DataRow> matches = table.AsEnumerable()
                    .Where(row => Cell(row, "STAGING_ID") == stagingId)
                    .ToList();

                if (matches.Count == 0)
                {
                    result.Errors.Add("Dòng " + source.SourceLine + " - " + source.Callsign
                        + ": không tìm thấy chuyến bay/phép SC tương ứng để hủy.");
                    continue;
                }

                matchedSources++;
                foreach (DataRow matched in matches)
                    result.CancelledFlights.Add(ToCancellationRow(matched));
            }
            result.Success = result.Failed == 0 && result.Imported == result.Total
                && matchedSources == selectedRows.Count;
        }

        private static DataTable FindExistingPermits(clsResuftAPI api, ImportRequest request)
        {
            return api.GetTableApiExtension("PERM_PKG", "validFlightNbr", new
            {
                P_AUTHOR = Clean(request.Author), P_FLIGHT_TYPE = "SC",
                P_FLIGHTNBR = Clean(request.PermNbr), P_PERMTYPE = "LD"
            });
        }

        private static string Cell(DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) && row[column] != DBNull.Value
                ? Convert.ToString(row[column], CultureInfo.InvariantCulture).Trim().ToUpperInvariant() : "";
        }

        private static string DateCell(DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return "";
            DateTime date;
            return DateTime.TryParse(Convert.ToString(row[column], CultureInfo.InvariantCulture), out date)
                ? date.ToString("dd-MM-yyyy") : Convert.ToString(row[column], CultureInfo.InvariantCulture);
        }

        private static CancellationRow ToCancellationRow(DataRow row)
        {
            return new CancellationRow
            {
                Callsign = Cell(row, "CALLSIGN"), PermNbr = Cell(row, "PERMNBR_ID"),
                FromDate = DateCell(row, "FROMDATE"), ToDate = DateCell(row, "TODATE"),
                FromAirp = Cell(row, "FROM_AIRP"), ToAirp = Cell(row, "TO_AIRP"), Daily = Cell(row, "DAILY_PHEP"),
                Etd = Cell(row, "ETD"), Eta = Cell(row, "ETA"), Oper = Cell(row, "OPER"),
                PermType = Cell(row, "PERMTYPE"), Remark = Cell(row, "REMARK"), Purpose = Cell(row, "PURPOSE"),
                CancelDaily = Cell(row, "DAILY"), CancelFromDate = DateCell(row, "HUY_FROMDATE"),
                CancelToDate = DateCell(row, "HUY_TODATE")
            };
        }

        private static object BuildCancellationPayload(
            ImportRequest request, ImportRow row,
            string currentUser, string batchId)
        {
            return new
            {
                P_CALLSIGN = Clean(row.Callsign), P_FROMDATE = OracleDate(row.FromDate), P_TODATE = OracleDate(row.ToDate),
                P_DAILY = Clean(row.Daily), P_CRAFT = NormalizeCraft(row.Craft), P_FROM_AIRP = Clean(row.FromAirp),
                P_TO_AIRP = Clean(row.ToAirp), P_ETD = Clean(row.Etd), P_ETA = Clean(row.Eta), P_VIA = Clean(row.Via),
                P_REMARK = Clean(row.Remark), P_PERMTYPE = "LD", P_FLIGHTTYPE = Clean(request.FlightType),
                P_PERMNBR = Clean(request.PermNbr), P_SEASON = Clean(request.Season), P_AUTHOR = Clean(request.Author),
                P_PERMDATE = OracleDate(request.PermDate), P_PURPOSE = Clean(request.Purpose), P_VERSION = Clean(request.Version),
                P_REGISTRATION = Clean(request.Registration), P_CREATED_BY = currentUser,
                P_IMPORT_BATCH_ID = batchId
            };
        }

        private static PendingRow ToPendingRow(DataRow row)
        {
            long id;
            Int64.TryParse(Cell(row, "STAGING_ID"), out id);
            return new PendingRow
            {
                StagingId = id,
                BatchId = Cell(row, "IMPORT_BATCH_ID"),
                PermNbr = Cell(row, "PERMNBR"),
                Callsign = Cell(row, "CALLSIGN"),
                FromDate = DateCell(row, "FROMDATE"),
                ToDate = DateCell(row, "TODATE"),
                Daily = Cell(row, "DAILY"),
                Craft = Cell(row, "CRAFT"),
                FromAirp = Cell(row, "FROM_AIRP"),
                ToAirp = Cell(row, "TO_AIRP"),
                Etd = Cell(row, "ETD"),
                Eta = Cell(row, "ETA"),
                Via = Cell(row, "VIA"),
                Remark = Cell(row, "REMARK"),
                Oper = Cell(row, "OPER"),
                Status = Cell(row, "PROCESS_STATUS"),
                ErrorMessage = Cell(row, "ERROR_MESSAGE"),
                CreatedAt = Cell(row, "CREATED_AT")
            };
        }

        private static ImportRow ToImportRow(DataRow row)
        {
            PendingRow pending = ToPendingRow(row);
            return new ImportRow
            {
                Selected = true,
                StagingId = pending.StagingId,
                Callsign = pending.Callsign,
                FromDate = pending.FromDate,
                ToDate = pending.ToDate,
                Daily = pending.Daily,
                Craft = pending.Craft,
                FromAirp = pending.FromAirp,
                ToAirp = pending.ToAirp,
                Etd = pending.Etd,
                Eta = pending.Eta,
                Via = pending.Via,
                Remark = pending.Remark
            };
        }

        private static string CurrentUserName()
        {
            T_Users user = MenuCache.ResolveCurrentUser(new UserDAL());
            if (user == null || String.IsNullOrWhiteSpace(user.UserName))
                throw new UnauthorizedAccessException(
                    "Phien dang nhap da het han. Vui long dang nhap lai.");
            return Clean(user.UserName);
        }

        private static string StagingIdList(IEnumerable<long> stagingIds)
        {
            string value = stagingIds == null ? "" : String.Join(",",
                stagingIds.Where(id => id > 0).Distinct()
                    .Select(id => id.ToString(CultureInfo.InvariantCulture))
                    .ToArray());
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Danh sach staging ID khong hop le.");
            return value;
        }

        private static string OptionalOracleDate(string value)
        {
            return String.IsNullOrWhiteSpace(value) ? null : OracleDate(value);
        }

        private static void ValidateRequest(ImportRequest request)
        {
            ValidateRequestHeader(request);
            if (request.Rows == null || !request.Rows.Any(x => x != null && x.Selected))
                throw new ArgumentException("Chưa chọn dòng dữ liệu hợp lệ.");
        }

        private static void ValidateRequestHeader(ImportRequest request)
        {
            if (request == null) throw new ArgumentException("Không nhận được dữ liệu import.");
            if (String.IsNullOrWhiteSpace(request.PermNbr) || request.PermNbr.Trim().Length > 8)
                throw new ArgumentException("Number bắt buộc và tối đa 8 ký tự.");
            OracleDate(request.PermDate);
        }

        private static void ValidateRow(ImportRow row)
        {
            if (String.IsNullOrWhiteSpace(row.Callsign)) throw new ArgumentException("Thiếu callsign");
            OracleDate(row.FromDate); OracleDate(row.ToDate);
            if (String.IsNullOrWhiteSpace(row.FromAirp) || row.FromAirp.Trim().Length < 3 || row.FromAirp.Trim().Length > 4) throw new ArgumentException("FROM không hợp lệ");
            if (String.IsNullOrWhiteSpace(row.ToAirp) || row.ToAirp.Trim().Length < 3 || row.ToAirp.Trim().Length > 4) throw new ArgumentException("TO không hợp lệ");
            if (String.IsNullOrWhiteSpace(row.Etd) || String.IsNullOrWhiteSpace(row.Eta)) throw new ArgumentException("ETD/ETA không hợp lệ");
        }

        private static string OracleDate(string value)
        {
            DateTime date;
            string[] formats = { "yyyy-MM-dd", "dd-MM-yyyy", "dd/MM/yyyy", "dd-MMM-yy", "dd-MMM-yyyy" };
            if (!DateTime.TryParseExact(value == null ? "" : value.Trim(), formats, CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces, out date))
                throw new ArgumentException("Ngày không hợp lệ: " + value);
            return date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        }

        private static string NormalizeCraft(string value)
        {
            string craft = Clean(value);
            if (craft == "321" || craft == "320" || craft == "319") return "A" + craft;
            if (craft == "787") return "B787";
            return craft;
        }

        private static string Clean(string value) { return (value ?? "").Trim().ToUpperInvariant(); }

        public class ImportRequest
        {
            public string Action { get; set; } public string Format { get; set; }
            public string PermNbr { get; set; } public string PermDate { get; set; } public string Author { get; set; }
            public string Version { get; set; } public string Season { get; set; } public string Purpose { get; set; }
            public string FlightType { get; set; } public string Registration { get; set; }
            public bool AllowExistingPermit { get; set; }
            public List<ImportRow> Rows { get; set; }
        }

        public class ImportRow
        {
            public int SourceLine { get; set; } public bool Selected { get; set; } public string Callsign { get; set; }
            public string FromDate { get; set; } public string ToDate { get; set; } public string Daily { get; set; }
            public string Craft { get; set; } public string FromAirp { get; set; } public string ToAirp { get; set; }
            public string Etd { get; set; } public string Eta { get; set; } public string Via { get; set; }
            public string Remark { get; set; }
            public long StagingId { get; set; }
        }

        public class ImportResult
        {
            public ImportResult()
            {
                Errors = new List<string>();
                ImportedRows = new List<ImportRow>();
                CancelledFlights = new List<CancellationRow>();
            }
            public bool Success { get; set; } public bool RequiresReview { get; set; }
            public int Total { get; set; } public int Imported { get; set; } public int Failed { get; set; }
            public string Message { get; set; } public List<string> Errors { get; set; }
            public List<ImportRow> ImportedRows { get; set; }
            public List<CancellationRow> CancelledFlights { get; set; }
        }

        public class ApplyResult { public bool Success { get; set; } public string Message { get; set; } }

        public class PendingSearch
        {
            public string PermNbr { get; set; }
            public string Callsign { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
        }

        public class PendingResult
        {
            public PendingResult() { Rows = new List<PendingRow>(); }
            public bool Success { get; set; }
            public string Message { get; set; }
            public List<PendingRow> Rows { get; set; }
        }

        public class PendingRow
        {
            public long StagingId { get; set; }
            public string BatchId { get; set; }
            public string PermNbr { get; set; }
            public string Callsign { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string Daily { get; set; }
            public string Craft { get; set; }
            public string FromAirp { get; set; }
            public string ToAirp { get; set; }
            public string Etd { get; set; }
            public string Eta { get; set; }
            public string Via { get; set; }
            public string Remark { get; set; }
            public string Oper { get; set; }
            public string Status { get; set; }
            public string ErrorMessage { get; set; }
            public string CreatedAt { get; set; }
        }

        public class PermitCheckResult
        {
            public PermitCheckResult() { PermitNumbers = new List<string>(); }
            public bool Success { get; set; } public bool Exists { get; set; }
            public string Message { get; set; } public List<string> PermitNumbers { get; set; }
        }

        public class CancellationRow
        {
            public string Callsign { get; set; } public string PermNbr { get; set; }
            public string FromDate { get; set; } public string ToDate { get; set; }
            public string FromAirp { get; set; } public string ToAirp { get; set; }
            public string Daily { get; set; } public string Etd { get; set; } public string Eta { get; set; }
            public string Oper { get; set; } public string PermType { get; set; }
            public string Remark { get; set; } public string Purpose { get; set; }
            public string CancelDaily { get; set; } public string CancelFromDate { get; set; }
            public string CancelToDate { get; set; }
        }
    }
}
