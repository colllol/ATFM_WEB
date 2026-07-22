using System.Collections.Generic;

namespace QLB.Info
{
    public class SlotsTableRequest
    {
        public string Source { get; set; }
        public string Keyword { get; set; }
        public string FlightDate { get; set; }
        public string Airport { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class SlotsTableColumn
    {
        public string Name { get; set; }
        public string DataType { get; set; }
    }

    public class SlotsTableResponse
    {
        public string Source { get; set; }
        public List<SlotsTableColumn> Columns { get; set; }
        public List<Dictionary<string, string>> Rows { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }

    public class SlotComparisonRequest
    {
        public string CompareDate { get; set; }
        public string Oper { get; set; }
        public string ResultType { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class SlotComparisonBootstrapResponse
    {
        public string DefaultDate { get; set; }
        public List<string> Operators { get; set; }
    }

    public class SlotComparisonRow
    {
        public long Id { get; set; }
        public string ResultType { get; set; }
        public string FlightDate { get; set; }
        public string Oper { get; set; }
        public string Callsign { get; set; }
        public string FromAirp { get; set; }
        public string ToAirp { get; set; }
        public string Etd { get; set; }
        public string Remark { get; set; }
        public string SourceRef { get; set; }
    }

    public class SlotComparisonResponse
    {
        public string CompareDate { get; set; }
        public string Oper { get; set; }
        public string ResultType { get; set; }
        public Dictionary<string, int> Summary { get; set; }
        public List<SlotComparisonRow> Rows { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }
}
