using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace prjApplication.SLOTS
{
    internal sealed class SlotsApiClient
    {
        private static readonly HttpClient Http = new HttpClient { Timeout = TimeSpan.FromSeconds(120) };

        public SlotsTableApiResponse GetTableData(string source, string keyword, string flightDate,
            string airport, int pageIndex, int pageSize)
        {
            string endpoint = string.Equals(source, "KHH", StringComparison.OrdinalIgnoreCase)
                ? "api/Khh/GetData"
                : "api/SlotAero/GetData";
            return Post<SlotsTableApiResponse>(endpoint, new
            {
                Source = source,
                Keyword = keyword,
                FlightDate = flightDate,
                Airport = airport,
                PageIndex = pageIndex,
                PageSize = pageSize
            });
        }

        public SlotComparisonBootstrapApiResponse GetComparisonBootstrap()
        {
            return Post<SlotComparisonBootstrapApiResponse>("api/SlotComparison/GetBootstrap", new { });
        }

        public SlotComparisonApiResponse GetComparisonResults(string compareDate, string oper,
            string resultType, int pageIndex, int pageSize)
        {
            return Post<SlotComparisonApiResponse>("api/SlotComparison/GetResults", new
            {
                CompareDate = compareDate,
                Oper = oper,
                ResultType = resultType,
                PageIndex = pageIndex,
                PageSize = pageSize
            });
        }

        private static T Post<T>(string relativeUrl, object request)
        {
            string baseUrl = ConfigurationManager.AppSettings["ApplicationPath.API"];
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ConfigurationErrorsException("Thiếu cấu hình ApplicationPath.API.");

            Uri endpoint = new Uri(new Uri(baseUrl.TrimEnd('/') + "/"), relativeUrl);
            string json = JsonConvert.SerializeObject(request);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            using (HttpResponseMessage response = Http.PostAsync(endpoint, content).GetAwaiter().GetResult())
            {
                string responseJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException("API trả về HTTP " + (int)response.StatusCode + ".");

                ApiEnvelope<T> envelope = JsonConvert.DeserializeObject<ApiEnvelope<T>>(responseJson);
                if (envelope == null)
                    throw new InvalidOperationException("Phản hồi API không hợp lệ.");
                if (!string.Equals(envelope.Code, "00", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException(string.IsNullOrWhiteSpace(envelope.Message)
                        ? "API không thể lấy dữ liệu." : envelope.Message);
                if (envelope.ListValue == null)
                    throw new InvalidOperationException("API không trả về dữ liệu.");
                return envelope.ListValue;
            }
        }
    }

    internal sealed class ApiEnvelope<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public T ListValue { get; set; }
    }

    internal sealed class SlotsTableApiColumn
    {
        public string Name { get; set; }
        public string DataType { get; set; }
    }

    internal sealed class SlotsTableApiResponse
    {
        public string Source { get; set; }
        public List<SlotsTableApiColumn> Columns { get; set; }
        public List<Dictionary<string, string>> Rows { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }

    internal sealed class SlotComparisonBootstrapApiResponse
    {
        public string DefaultDate { get; set; }
        public List<string> Operators { get; set; }
    }

    internal sealed class SlotComparisonApiRow
    {
        public string FlightDate { get; set; }
        public string Oper { get; set; }
        public string Callsign { get; set; }
        public string FromAirp { get; set; }
        public string ToAirp { get; set; }
        public string Etd { get; set; }
        public string Remark { get; set; }
    }

    internal sealed class SlotComparisonApiResponse
    {
        public string CompareDate { get; set; }
        public string Oper { get; set; }
        public string ResultType { get; set; }
        public Dictionary<string, int> Summary { get; set; }
        public List<SlotComparisonApiRow> Rows { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }
}
