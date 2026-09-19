using System;
using System.Collections.Generic;
using System.Text;
using prjInfo;
using HPCShareDLL;
using System.Data;


namespace prjBusinessLogic
{
   public class ReportDAL
    {
       public DataSet Report_Banve(int matinh, int dvvt, DateTime ngaydi)
       {
           DataSet _ds = null;
           try
           {
               _ds = HPCDataProvider.Instance().GetStoreDataSet("Report_Banve", new string[] { "@Ma_tinh_den", "@Ma_DVVT", "@Ngay_di" }, new object[] { matinh, dvvt,ngaydi });
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return _ds;
       }
    }
}
