using prjBusinessLogic;
using System;
using System.Collections.Generic;
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
        public static ImportResult ImportRows(ImportRequest request)
        {
            var result = new ImportResult();
            try
            {
                ValidateRequest(request);
                var api = new clsResuftAPI();
                var selectedRows = request.Rows.Where(x => x != null && x.Selected).ToList();
                result.Total = selectedRows.Count;

                foreach (var row in selectedRows)
                {
                    try
                    {
                        ValidateRow(row);
                        string procedure = string.Equals(request.Format, "ALL_OPER", StringComparison.OrdinalIgnoreCase)
                            ? "PERMSC_IMP_INSERT_ALL_OPER"
                            : "PERMSC_IMP_INSERT";

                        object response;
                        if (procedure == "PERMSC_IMP_INSERT_ALL_OPER")
                        {
                            response = api.GetPostValueApiExtension("PERM_IMP_PKG", procedure, BuildAllOperPayload(request, row));
                        }
                        else
                        {
                            response = api.GetPostValueApiExtension("PERM_IMP_PKG", procedure, BuildPayload(request, row));
                        }

                        int code;
                        if (!Int32.TryParse(Convert.ToString(response, CultureInfo.InvariantCulture), out code) || code <= 0)
                            throw new InvalidOperationException("Procedure trả về " + Convert.ToString(response, CultureInfo.InvariantCulture));

                        result.Imported++;
                    }
                    catch (Exception ex)
                    {
                        result.Failed++;
                        result.Errors.Add("Dòng " + row.SourceLine + " - " + (row.Callsign ?? "") + ": " + ex.Message);
                    }
                }

                result.RequiresReview = string.Equals(request.Action, "HuyChuyen", StringComparison.OrdinalIgnoreCase);
                result.Success = result.Failed == 0 && result.Imported > 0;
                result.Message = result.RequiresReview
                    ? "Đã nhập các dòng hủy được chọn vào vùng chờ. Chưa tự động hủy chuyến; cần đối chiếu danh sách phép trước khi áp dụng."
                    : "Đã xử lý " + result.Imported + "/" + result.Total + " dòng được chọn.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                result.Errors.Add(ex.Message);
            }
            return result;
        }

        private static object BuildPayload(ImportRequest request, ImportRow row)
        {
            return new
            {
                P_CALLSIGN = Clean(row.Callsign), P_FROMDATE = OracleDate(row.FromDate), P_TODATE = OracleDate(row.ToDate),
                P_DAILY = Clean(row.Daily), P_CRAFT = NormalizeCraft(row.Craft), P_FROM_AIRP = Clean(row.FromAirp),
                P_TO_AIRP = Clean(row.ToAirp), P_ETD = Clean(row.Etd), P_ETA = Clean(row.Eta), P_VIA = Clean(row.Via),
                P_REMARK = Clean(row.Remark), P_PERMTYPE = "LD", P_PERMNBR = Clean(request.PermNbr),
                P_FLIGHTTYPE = Clean(request.FlightType), P_SEASON = Clean(request.Season), P_OPER = Clean(request.Oper),
                P_AUTHOR = Clean(request.Author), P_PERMDATE = OracleDate(request.PermDate), P_VERSION = Clean(request.Version),
                P_PURPOSE = Clean(request.Purpose), P_ACTION = Clean(request.Action)
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
                P_REGISTRATION = Clean(request.Registration), P_ACTION = Clean(request.Action)
            };
        }

        private static void ValidateRequest(ImportRequest request)
        {
            if (request == null) throw new ArgumentException("Không nhận được dữ liệu import.");
            if (String.IsNullOrWhiteSpace(request.PermNbr) || request.PermNbr.Trim().Length > 8)
                throw new ArgumentException("Number bắt buộc và tối đa 8 ký tự.");
            bool derivesOper = String.Equals(request.Format, "ALL_OPER", StringComparison.OrdinalIgnoreCase);
            if (!derivesOper && (String.IsNullOrWhiteSpace(request.Oper) ||
                String.Equals(request.Oper.Trim(), "ALL_OPER", StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException("Hãng khai thác bắt buộc với format " + request.Format + ".");
            OracleDate(request.PermDate);
            if (request.Rows == null || !request.Rows.Any(x => x != null && x.Selected))
                throw new ArgumentException("Chưa chọn dòng dữ liệu hợp lệ.");
        }

        private static void ValidateRow(ImportRow row)
        {
            if (String.IsNullOrWhiteSpace(row.Callsign)) throw new ArgumentException("Thiếu callsign");
            OracleDate(row.FromDate); OracleDate(row.ToDate);
            if (String.IsNullOrWhiteSpace(row.FromAirp) || row.FromAirp.Trim().Length != 4) throw new ArgumentException("FROM không hợp lệ");
            if (String.IsNullOrWhiteSpace(row.ToAirp) || row.ToAirp.Trim().Length != 4) throw new ArgumentException("TO không hợp lệ");
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
            public string Action { get; set; } public string Format { get; set; } public string Oper { get; set; }
            public string PermNbr { get; set; } public string PermDate { get; set; } public string Author { get; set; }
            public string Version { get; set; } public string Season { get; set; } public string Purpose { get; set; }
            public string FlightType { get; set; } public string Registration { get; set; }
            public List<ImportRow> Rows { get; set; }
        }

        public class ImportRow
        {
            public int SourceLine { get; set; } public bool Selected { get; set; } public string Callsign { get; set; }
            public string FromDate { get; set; } public string ToDate { get; set; } public string Daily { get; set; }
            public string Craft { get; set; } public string FromAirp { get; set; } public string ToAirp { get; set; }
            public string Etd { get; set; } public string Eta { get; set; } public string Via { get; set; }
            public string Remark { get; set; }
        }

        public class ImportResult
        {
            public ImportResult() { Errors = new List<string>(); }
            public bool Success { get; set; } public bool RequiresReview { get; set; }
            public int Total { get; set; } public int Imported { get; set; } public int Failed { get; set; }
            public string Message { get; set; } public List<string> Errors { get; set; }
        }
    }
}
