using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using tmatsol_dump.Models.Web_Report.DAL;

namespace tmatsol_dump.Models.Web_Report.Combos
{
    public class Combos 
    {
        string XCUSTID;

        public static SelectListItem[] FI_Web_Report_Details()
        {
            //select Report_Title,Report_Proc_Name from FI_Web_Report_Details

            DataSet dt = new DataSet();
            DAL.DAL NewDal = new DAL.DAL();
            
            
            
            
            string Xerror = "";
            dt = NewDal.GetDataset("select Report_Title as[Name], Report_Title as [Value],Report_Proc_Name from FI_Web_Report_Details", ref Xerror);

            List<SelectListItem> ListDt = new List<SelectListItem>();
            foreach ( DataTable table in dt.Tables)
            {
                foreach(DataRow DR in table.Rows)
                {
                    ListDt.Add(new SelectListItem { Text =  DR[0].ToString() , Value = DR[1].ToString() });
                }
            }
            dt.Dispose();
            return ListDt.ToArray();         
        }
        public static SelectListItem[] Cashier_Settle_Date()
        {
            List<SelectListItem> List = new List<SelectListItem>();

            List.Add(new SelectListItem { Text = "Cashier Date", Value = "Cashier Date" });
            List.Add(new SelectListItem { Text = "Settled Date", Value = "Settled Date" });
            List.Add(new SelectListItem { Text = "Sale Date", Value = "Sale Date" });
            //ViewData["Filter"] = Filter;

            return List.ToArray();
        }    
    }
}