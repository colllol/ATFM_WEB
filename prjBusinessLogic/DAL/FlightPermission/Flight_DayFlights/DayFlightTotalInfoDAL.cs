using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace prjBusinessLogic
{
    public class DayFlightTotalInfoDAL
    {
        //get info #topBar1
        public DataTable GetAll()
        {
            return new clsResuftAPI().GetTableObj("api/DayFlights/GetDaylyFlightInfo/");
        }
        public DataTable GetInfoTopbar1_Hcm(string dsSanBay)
        {
            return new clsResuftAPI().GetTableApiExtension("FLIGHT_DAYFLIGHT", "GetDivInfo_Hcm", new { P_LISTHCM = dsSanBay });
        }
        public DataTable GetInfoTopbar1_Dng(string dsSanBay)
        {
            return new clsResuftAPI().GetTableApiExtension("FLIGHT_DAYFLIGHT", "GetDivInfo_Dng", new { P_LISTDNG = dsSanBay });
        }
        //get info #topBar3
        public DataTable GetInfoTopBar3()
        {
            return new clsResuftAPI().GetPostTableApiExtension("FLIGHT_DAYFLIGHT", "GetInfoTopBar3", null);
        }
        public DataTable GetAllRealPlanMessage(DateTime date, string callSign)
        {
            return new clsResuftAPI().GetTableApiExtension("FLIGHT_DAYFLIGHT", "FlightInfoExtension_Message", new { P_DATEPK = date, P_FLIGHTNBR = callSign });
        }
        public DataTable GetAllRealPlanMessageExt(string id)
        {
            return new clsResuftAPI().GetTableApiExtension("FLIGHT_DAYFLIGHT", "FlightInfoExtension_MessageExt", new { P_ID=id });
        }
        public DataTable GetInfoChangeById(string id)
        {
            return new clsResuftAPI().GetTableObjectById("api/DayFlights/GetById/", id);
        }
        public bool AccessInfoChange(string id)
        {
            var ax = new clsResuftAPI().GetValueApiExtension("FLIGHT_DAYFLIGHT", "AccessInfoChange"
                , new { P_VALUE = (Int64.Parse(id)) });
            if (ax.ToString() == "-1") return false;
            return true;
        }
    }
    public class DayFlightSetColorDAL
    {
        public DataTable GetTableColor(string idUser)
        {
            return new clsResuftAPI().GetTableApiExtension("FLIGHT_DAYFLIGHT", "GetTableColorByUser", new { P_USERID = idUser });
        }
        public bool SetDefaultColor(string idUser, string cssClass)
        {
            return new clsResuftAPI().GetValueApiExtension("FLIGHT_DAYFLIGHT", "SetDefaultColor", new { P_USERID = idUser, P_CSSCLASSNAME = cssClass }).ToString() == "1" ? true : false;
        }
        public bool UpdateColorByIdUser(string idUser, string elementDom, string customColor, string classColor)
        {
            return new clsResuftAPI().GetValueApiExtension("FLIGHT_DAYFLIGHT", "UpdateColorByIdUser", new { P_IDUSER = idUser, P_ELEMENTDOM = elementDom, P_COLORCUSTOM = customColor, P_CSSCLASSNAME = classColor }).ToString() == "1" ? true : false;
        }
        public bool RuntimeSoundNotification()
        {
            return true;
            /*
            DataTable dt = new clsResuftAPI().GetTableApiExtension("FLIGHT_DAYFLIGHT", "GetStatusMessageRecice", null);
            if (dt == null) return false;
            if (dt.Rows[0]["NSTATUS"].ToString() == "1" && dt.Rows[0]["ISSOUND"].ToString() == "1")
                return true;
            return false;
            */
        }
    }
}
