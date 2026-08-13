using prjBusinessLogic;
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
                result.Total = selectedRows.Count;

                foreach (var row in selectedRows)
                {
                    try
                    {
                        ValidateRow(row);
                        object response = api.GetPostValueApiExtension(
                            "PERM_IMP_PKG",
                            "PERMSC_IMP_INSERT_ALL_OPER",
                            BuildAllOperPayload(request, row));

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
                    CheckExistingPermissions(api, selectedRows, result);

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
        public static ApplyResult ApplyCancellation()
        {
            var result = new ApplyResult();
            try
            {
                object value = new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "impToPerm_Huy", null);
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
        public static ApplyResult DeleteCancellation()
        {
            var result = new ApplyResult();
            try
            {
                object value = new clsResuftAPI().GetValueApiExtension(
                    "PERM_IMP_PKG", "DeleteChuyenHuy_IMP", null);
                int code;
                result.Success = Int32.TryParse(
                    Convert.ToString(value, CultureInfo.InvariantCulture), out code) && code == 1;
                result.Message = result.Success
                    ? "Đã xóa toàn bộ danh sách HỦY CHUYẾN đang chờ xử lý."
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

        private static void CheckExistingPermissions(clsResuftAPI api, IList<ImportRow> selectedRows, ImportResult result)
        {
            DataTable table = api.GetTableApiExtension(
                "PERM_IMP_V2_PKG", "GET_CANCELLATION_MATCHES", null);
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

        private static object BuildAllOperPayload(ImportRequest request, ImportRow row)
        {
            return new
            {
                P_CALLSIGN = Clean(row.Callsign), P_FROMDATE = OracleDate(row.FromDate), P_TODATE = OracleDate(row.ToDate),
                P_DAILY = Clean(row.Daily), P_CRAFT = NormalizeCraft(row.Craft), P_FROM_AIRP = Clean(row.FromAirp),
                P_TO_AIRP = Clean(row.ToAirp), P_ETD = Clean(row.Etd), P_ETA = Clean(row.Eta), P_VIA = Clean(row.Via),
                P_REMARK = Clean(row.Remark), P_PERMTYPE = "LD", P_FLIGHTTYPE = Clean(request.FlightType),
                P_PERMNBR = Clean(request.PermNbr), P_SEASON = Clean(request.Season), P_AUTHOR = Clean(request.Author),
                P_PERMDATE = OracleDate(request.PermDate), P_PURPOSE = Clean(request.Purpose), P_VERSION = Clean(request.Version),
                P_REGISTRATION = Clean(request.Registration), P_ACTION = "HuyChuyen"
            };
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
            public int StagingId { get; set; }
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
