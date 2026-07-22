using System.Collections.Generic;

namespace QLB.Info
{
    public class AdsBPerformanceRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PermType { get; set; }
        public string Oper { get; set; }
    }

    public class AdsBPerformanceResponse
    {
        public string Source { get; set; }
        public string ServerTime { get; set; }
        public int Total { get; set; }
        public int Ld { get; set; }
        public int Of { get; set; }
        public int OperatorCount { get; set; }
        public List<AdsBPerformanceTrend> Trend { get; set; }
        public List<AdsBPerformanceRow> Rows { get; set; }
    }

    public class AdsBPerformanceTrend
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public int Ld { get; set; }
        public int Of { get; set; }
    }

    public class AdsBPerformanceRow
    {
        public int No { get; set; }
        public long Id { get; set; }
        public string Callsign { get; set; }
        public string Oper { get; set; }
        public string PermType { get; set; }
        public string FromAirp { get; set; }
        public string ToAirp { get; set; }
        public string Etd { get; set; }
        public string Eta { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public string Date { get; set; }
        public string UpdatedAtUtc { get; set; }
    }
}
