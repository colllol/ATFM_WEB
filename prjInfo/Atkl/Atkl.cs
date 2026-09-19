using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
   public class Atkl
    {
        public int ID { get; set; }
        public DateTime DATE_EXPLOIT { get; set; }
        public string CRAFT_TYPE { get; set; }
        public string REGISTRATION { get; set; }
        public string CALLSIGN { get; set; }
        public string FROM_AIRPORT { get; set; }
        public string TO_AIRPORT { get; set; }
        public string ETD_FIRST { get; set; }
        public string ETA_FIRST { get; set; }
        public string ETD_CHANGE { get; set; }
        public string ETA_CHANGE { get; set; }
        public string FPL_START { get; set; }
        public string FPL_SUM { get; set; }
        public string AREA_PARKING { get; set; }
        public string AREA_EOBT { get; set; }
        public string AREA_AOBT { get; set; }
        public string AREA_START_UP { get; set; }
        public string AREA_PUSH_BACK { get; set; }
        public string AREA_START_RUN { get; set; }
        public string AREA_TWR_APP { get; set; }
        public string TWR_CHC { get; set; }
        public string TWR_TOPOINT_FLY_HOUR { get; set; }
        public string TWR_FLY { get; set; }
        public string TWR_APP { get; set; }
        public string TWR_FLY_REALY { get; set; }
        public string APPROACH_TRANSFER { get; set; }
        public string APPROACH_POINT { get; set; }
        public string APPROACH_NEXTAREA { get; set; }
        public string APPROACH_RADAR { get; set; }
        public string APPROACH_RADAR_POINT { get; set; }
        public string APPROACH_RADAR_NEXTAREA { get; set; }
        public string DISTANCE_APP_TRANSFER { get; set; }
        public string DISTANCE_APP_POINT { get; set; }
        public string DISTANCE_APP_NEXTAREA { get; set; }
        public string DISTANCE_ADJACENT { get; set; }
        public string DISTANCE_ADJACENT_POINT { get; set; }
        public string DISTANCE_ADJACENT_NEXTAREA { get; set; }
        public string DISTANCE_NEXT_TRANSFER { get; set; }
        public string DISTANCE_NEXT_POINT { get; set; }
        public string DISTANCE_NEXT_NEXTAREA { get; set; }
        public string APPROACH_TRANSFER_1 { get; set; }
        public string APPROACH_POINT_1 { get; set; }
        public string APPROACH_NEXTAREA_1 { get; set; }
        public string APPROACH_RADAR_1 { get; set; }
        public string APPROACH_RADAR_POINT_1 { get; set; }
        public string APPROACH_RADAR_NEXTAREA_1 { get; set; }
        public string TWR_APP_OVER { get; set; }
        public string TWR_TRANSFER_POINT { get; set; }
        public string TWR_TOUCH_DOWN { get; set; }
        public string TWR_CHC_OVER { get; set; }
        public string TWR_GCU { get; set; }
        public string AREA_START_UP_OVER { get; set; }
        public string AREA_SHUT_DOWN { get; set; }
        public string AREA_AIBT { get; set; }
        public string ATD { get; set; }
        public string ATA { get; set; }
        public string ROUTE_PERMISSION { get; set; }
        public string ROUTE_REALITY { get; set; }
        public string REMARK { get; set; }
        public Int64 Record_Sum { get; set; }
    }
    public class clsSearchAtkl : Atkl
    {
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public int PAGE_INDEX { get; set; }
        public int PAGE_SIZE { get; set; }
    }
}
