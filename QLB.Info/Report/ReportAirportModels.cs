using System.Collections.Generic;

namespace QLB.Info
{
    public class ReportAirportRangeRequest { public string FromDate { get; set; } public string ToDate { get; set; } }
    public class ReportAirportSummaryRequest : ReportAirportRangeRequest { public string Airport { get; set; } }
    public class ReportAirportDetailsRequest : ReportAirportSummaryRequest
    {
        public string Movement { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class ReportAirportFlightDto
    {
        public int Stt { get; set; }
        public string FlightDate { get; set; }
        public string Callsign { get; set; }
        public string Oper { get; set; }
        public string Registration { get; set; }
        public string PermType { get; set; }
        public string FromAirp { get; set; }
        public string ToAirp { get; set; }
        public string AtdDay { get; set; }
        public string AtaDay { get; set; }
        public string EobtDay { get; set; }
        public string Status { get; set; }
    }

    public class ReportAirportChartResponse
    {
        public string Source { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<ReportAirportFlightDto> Flights { get; set; }
    }

    public class ReportAirportMetricDto
    {
        public string Code { get; set; }
        public int Departures { get; set; }
        public int Arrivals { get; set; }
        public int Total { get; set; }
    }

    public class ReportAirportSummaryResponse
    {
        public string Source { get; set; }
        public int TotalDepartures { get; set; }
        public int TotalArrivals { get; set; }
        public int AirportCount { get; set; }
        public string PeakAirport { get; set; }
        public int PeakTotal { get; set; }
        public List<ReportAirportMetricDto> Airports { get; set; }
    }

    public class ReportAirportDetailsResponse
    {
        public string Airport { get; set; }
        public string Movement { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<ReportAirportFlightDto> Rows { get; set; }
    }
}
