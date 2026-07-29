using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
namespace prjBusinessLogic
{
    public class DayFlightsDAL
    {
        public bool InsertObject(DayFlights obj)
        {
            return new clsResuftAPI<DayFlights>().InsertObj(obj, "api/DayFlights/Insert");
        }
        public bool UpdateObject(DayFlights obj)
        {
            return new clsResuftAPI<DayFlights>().UpdateObj(obj, "api/DayFlights/Update");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<DayFlights>().DeleteObj(id, "api/DayFlights/Delete/");
        }
        public bool MoveObject(string id)
        {
            //return new clsResuftAPI<DayFlights>().DeleteObj(id, "api/DayFlights/MoveDate/");
            var ax = new clsResuftAPI().GetValueApiExtension("FLIGHT_DAYFLIGHT", "OMOVEDATE"
               , new { P_ID = id });
            if (ax.ToString() == "-1") return false;
            return true;
        }
        public DayFlights GetOneObject(string id)
        {
            return new clsResuftAPI<DayFlights>().GetOneObj(id, "api/DayFlights/GetById/");
        }
        public List<DayFlights> GetAllObject()
        {
            return new clsResuftAPI<DayFlights>().GetListObj("api/DayFlights/GetAll");
        }
        public List<DayFlights> GetPageObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<DayFlights>().GetListObj("api/DayFlights/GetPage", pageSize, pageIndex, where);
        }
        public List<DayFlights> GetPageTrungLap(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<DayFlights>().GetListObj("api/DayFlights/GetPageTrungLap", pageSize, pageIndex, where);
        }
        public string InsertReturnId(DayFlights obj)
        {
            return new clsResuftAPI<DayFlights>().InsertReturnId(obj, "api/DayFlights/Insert");
        }
        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/DayFlights/GetAll");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/DayFlights/GetPage/", pageSize, pageIndex, where);
        }
        public System.Data.DataTable GetHistoryById(string id)
        {
            return new clsResuftAPI().GetTableHistory("api/DayFlights/GetHistoryById", id);
        }
        public System.Data.DataTable GetPermissionByFlightId(string id)
        {
            return new clsResuftAPI().GetTableObj(
                "api/DayFlights/GetPermBy?ID=" +
                Uri.EscapeDataString(id ?? string.Empty));
        }
        public System.Data.DataTable GetPermissionLinkFiles(string id, string permType)
        {
            return new clsResuftAPI().GetTableObj(
                "api/DayFlights/GetLinkFile?id=" +
                Uri.EscapeDataString(id ?? string.Empty) +
                "&permtype=" +
                Uri.EscapeDataString(permType ?? string.Empty));
        }
        public bool RestoreRecode(string id, string version, string idUser)
        {
            return new clsResuftAPI().RestoreRecord("api/DayFlights/RestoreHis", id, version, idUser);
        }
        public System.Data.DataTable GetTableDelete(string where)
        {
            return new clsResuftAPI().GetTableObj("api/DayFlights/GetRecordDeleted", where);
        }
        public System.Data.DataTable GetTableWithValueSearch(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/DayFlights/GetDaylyFlightBySearch", obj);
            
        }

        public System.Data.DataTable GetTableWithValueSearch_New(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/DayFlights/GetDaylyFlightBySearch_New", obj);            
        }
        public System.Data.DataTable GetTableNgayNCong1(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/DayFlights/GetDaylyFlightNgayNCong", obj);
        }
        public System.Data.DataTable GetTableNgayNTru1(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/DayFlights/GetDaylyFlightNgayNTru", obj);
        }
        public System.Data.DataTable GetTableWithValueSearch_HCM(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/DayFlights/GetDaylyFlightBySearch_HCM", obj);
            //return new clsResuftAPI().GetTableWithObject("api/DayFlights/GetDaylyFlightBy", obj);
        }
        public System.Data.DataTable GetTableWithValueSearch_DNG(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/DayFlights/GetDaylyFlightBySearch_DNG", obj);
            //return new clsResuftAPI().GetTableWithObject("api/DayFlights/GetDaylyFlightBy", obj);
        }

        #region flight haschange
        public System.Data.DataTable GetTableFlightHasChange(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject($"api/DayFlights/GetListFlightChange", obj);
        }
        #endregion
        #region not perm
        public System.Data.DataTable GetTableFlightNotPerm(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject($"api/DayFlights/GetListFlightNotPerm", obj);
        }
        #endregion


        #region atd not null
        public System.Data.DataTable GetFlightAtdNotNull(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject($"api/DayFlights/GetFlightAtdNotNull", obj);
        }
        #endregion


        #region not route
        public System.Data.DataTable GetTableFlightNotRoute(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject($"api/DayFlights/GetListFlightNotRoute", obj);
        }
        #endregion
        #region by search
        public System.Data.DataTable GetTableFlightBySearch(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject($"api/DayFlights/GetListFlightBySearch", obj);
        }
        #endregion

        #region Create Mess Dayly
        public System.Data.DataTable GetTableWithPartNo(string _date)
        {
            return new clsResuftAPI().GetPostTableApiExtension("FLIGHT_DAYFLIGHT", "View_Message", new { P_DATE = _date});           
        }
        public System.Data.DataTable GetTableGroupAddressWithPartNoID(string p_value)
        {
            return new clsResuftAPI().GetPostTableApiExtension("MESSAGE_PKG", "GroupAddress_GetbyID", new { P_VALUE = p_value});
        }
        public System.Data.DataTable GetTableWithPartNoID(string _date, string _partNo,string _messType)
        {
            
            return new clsResuftAPI().GetPostTableApiExtension("FLIGHT_DAYFLIGHT", "View_MessageDetail", new { P_DATE = _date, P_PARTNO = _partNo, P_MESSTYPE = _messType });
        }

        public System.Data.DataTable GetTableWithPartNo_Cancel(string _date)
        {
            return new clsResuftAPI().GetPostTableApiExtension("FLIGHT_DAYFLIGHT", "View_Message_Cancel", new { P_DATE = _date });
        }

        public System.Data.DataTable GetTableWithPartNoID_Cancel(string _date, string _partNo, string _messType)
        {

            return new clsResuftAPI().GetPostTableApiExtension("FLIGHT_DAYFLIGHT", "View_MessageDetail_Cancel", new { P_DATE = _date, P_PARTNO = _partNo, P_MESSTYPE = _messType });
        }



        public System.Data.DataTable GetTableWithPartNoAirPort(string _date)
        {
            return new clsResuftAPI().GetPostTableApiExtension("FLIGHT_DAYFLIGHT", "View_MessageAirPort", new { P_DATE = _date });
        }
        public System.Data.DataTable GetTableWithPartNoIDAirPort(string _date, string _partNo, string _messType)
        {
            return new clsResuftAPI().GetPostTableApiExtension("FLIGHT_DAYFLIGHT", "View_MessageDetailAirPort", new { P_DATE = _date, P_PARTNO = _partNo, P_MESSTYPE = _messType });
        }
        public System.Data.DataTable GetTableWithPartNoZone(string _date)
        {
            return new clsResuftAPI().GetPostTableApiExtension("FLIGHT_DAYFLIGHT", "View_MessageZone", new { P_DATE = _date });
        }
        public System.Data.DataTable GetTableWithPartNoIDZone(string _date, string _partNo, string _messType)
        {
            return new clsResuftAPI().GetPostTableApiExtension("FLIGHT_DAYFLIGHT", "View_MessageDetailZone", new { P_DATE = _date, P_PARTNO = _partNo, P_MESSTYPE = _messType });
        }
        #endregion

        #region Draft
        public bool InsertDraft(DayFlights obj)
        {
            return new clsResuftAPI<DayFlights>().InsertObj(obj, "api/DayFlights/InsertDraft");
        }
        public bool updateDraft(DayFlights obj)
        {
            return new clsResuftAPI<DayFlights>().InsertObj(obj, "api/DayFlights/UpdateDraft");
        }
        public DayFlights GetDraftById(string id)
        {
            return new clsResuftAPI<DayFlights>().GetOneObj(id, "api/DayFlights/GetByIdDraft");
        }
        public List<DayFlights> GetPageDraft(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<DayFlights>().GetListObj("api/DayFlights/GetPageDraft", pageSize, pageIndex, where);
        }
        public List<DayFlights> GetPageDraftTrungLap(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<DayFlights>().GetListObj("api/DayFlights/GetPageDraftTrungLap", pageSize, pageIndex, where);
        }
        public bool GenToDraftByDate(DateTime date, string user)
        {
            Int64 kq = Convert.ToInt64(new clsResuftAPI().GetValueApiExtension("CALENDARDRAFT", "GenToDraftByDate", new { P_USER = user, P_DATE = date }));
            if (kq == -1)
                return false;
            return true;
        }
        public bool AccessDraft(DateTime date, string user)
        {
            Int64 kq = Convert.ToInt64(new clsResuftAPI().GetValueApiExtension("CALENDARDRAFT", "AccessDraft", new { P_USER = user, P_DATE = date }));
            if (kq == -1)
                return false;
            return true;
        }
        public bool DeleteAll(string user)
        {
            Int64 kq = Convert.ToInt64(new clsResuftAPI().GetValueApiExtension("CALENDARDRAFT", "DeleteAll", new { P_USER = user }));
            if (kq == -1)
                return false;
            return true;
        }
        public bool DeleteDraft(string id)
        {
            Int64 kq = Convert.ToInt64(new clsResuftAPI().GetValueApiExtension("CALENDARDRAFT", "oDelete", new { P_ID = id }));
            if (kq == -1)
                return false;
            return true;
        }
        #endregion

        #region ke hoach bay ngay
        public System.Data.DataTable LayKeHoachBayNgay(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/KeHoachBay/GetKeHoachBay", obj);
        }
        public System.Data.DataTable LayKeHoachBayTrungLap(clsDaylyFlightSearch obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/KeHoachBay/GetKeHoachBayTrungLap", obj);
        }
        #endregion

    }
}
