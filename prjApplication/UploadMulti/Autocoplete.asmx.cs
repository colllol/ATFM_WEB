using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using prjBusinessLogic;
using prjBusinessLogic.DAL;
using prjInfo;
using prjComponents;

namespace prjApplication.UploadMulti
{
    /// <summary>
    /// Summary description for Autocoplete
    /// </summary>
    [WebService]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Autocoplete : System.Web.Services.WebService
    {
        //BenxeDAL dalbenxe = new BenxeDAL();
        //Donvi_VantaiDAL daldvvt = new Donvi_VantaiDAL();
        //XeDAL dalxe = new XeDAL();
        //LoaiviphamDAL dalloaivipham = new LoaiviphamDAL();

        [WebMethod]
        public string[] GetCompletionList(string prefixText, int count, string contextKey)
        {
            if (count == 0)
            {
                count = 20;
            }
            DataTable dt = GetRecords(prefixText, contextKey);
            List<string> itemsComplete = new List<string>(count);
            string item = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (int.Parse(contextKey) == 1)
                    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["Ten_benxe"].ToString(), dt.Rows[i]["ID"].ToString());
                else if (int.Parse(contextKey) == 2)
                    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["Ten_donvi"].ToString(), dt.Rows[i]["Ma_donvi"].ToString());
                else if (int.Parse(contextKey) == 3)
                    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["Bien_kiemsoat"].ToString(), dt.Rows[i]["ID"].ToString());
                else if (int.Parse(contextKey) == 4)
                    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["Ten_loai_vi_pham"].ToString(), dt.Rows[i]["ID"].ToString());
                else if (int.Parse(contextKey) == 5)
                    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["Hinhthucxuly"].ToString(), dt.Rows[i]["ID"].ToString());
                else if (int.Parse(contextKey) == 6)
                    item = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows[i]["Tentinh"].ToString(), dt.Rows[i]["ID"].ToString());
                itemsComplete.Add(item);
            }
            return itemsComplete.ToArray();
        }

        public DataTable GetRecords(string prefixText, string contextKey)
        {
            DataTable _dt = new DataTable();

            string _sql = string.Empty;

            UltilFunc ulti = new UltilFunc();
            if (int.Parse(contextKey) == 1)
            {
                _sql = "select top 20 ID,  Ten_benxe from T_BenXe where Ten_benxe LIKE N'%" + prefixText.Trim() + "%' and Ten_benxe is not null ";
                _dt = ulti.ExecSqlDataSet(_sql).Tables[0];
            }
            else if (int.Parse(contextKey) == 2)
            {
                _sql = "select top 20 Ma_donvi,  Ten_donvi from T_Donvi_Vantai where Ten_donvi LIKE N'%" + prefixText.Trim() + "%' and Ten_donvi is not null ";
                _dt = ulti.ExecSqlDataSet(_sql).Tables[0];
            }
            else if (int.Parse(contextKey) == 3)
            {
                _sql = "select top 20 ID,  Bien_kiemsoat from T_Xe where Bien_kiemsoat LIKE N'%" + prefixText.Trim() + "%' and Bien_kiemsoat is not null ";
                _dt = ulti.ExecSqlDataSet(_sql).Tables[0];
            }
            else if (int.Parse(contextKey) == 4)
            {
                _sql = "select top 20 ID, Ten_loai_vi_pham from T_Loai_vi_pham where Ten_loai_vi_pham LIKE N'%" + prefixText.Trim() + "%' and Ten_loai_vi_pham is not null ";
                _dt = ulti.ExecSqlDataSet(_sql).Tables[0];
            }
            else if (int.Parse(contextKey) == 5)
            {
                _sql = "select top 20 ID, Hinhthucxuly from T_HinhThuc_XuLy where Hinhthucxuly LIKE N'%" + prefixText.Trim() + "%' and Hinhthucxuly is not null ";
                _dt = ulti.ExecSqlDataSet(_sql).Tables[0];
            }
            else if (int.Parse(contextKey) == 6)
            {
                _sql = "select top 20 ID, Tentinh from T_Tinhthanh where Tentinh LIKE N'%" + prefixText.Trim() + "%' and Tentinh is not null ";
                _dt = ulti.ExecSqlDataSet(_sql).Tables[0];
            }
            return _dt;

        }
    }
}
