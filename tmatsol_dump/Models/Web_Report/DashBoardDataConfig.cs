using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using Newtonsoft.Json;

namespace tmatsol_dump.Models.Web_Report
{
    public class DashBoardDataConfig
    {
        private static void InsertTest(string CustName,string json)
        {
            SqlConnection con = new SqlConnection("Data Source=thinksoftwares.dyndns.org,2433;Initial Catalog=RedHandiKigali;User ID=RedHandiKigali;Password=Trushachampak@123;");
            con.Open();
            using (SqlCommand cmd = new SqlCommand("insert into Temp_test(Temp_test) values('" + CustName + json + DateTime.Now + "') ", con))
            {
                cmd.CommandType = CommandType.Text;
                int result = cmd.ExecuteNonQuery();
                con.Close();
            }
            con.Close();
        }

        private string XX_CustID;
        private string XX_CompNn;
       
        public DashBoardDataConfig(String XCustId, string xCompNaae)
        {
            XX_CustID = XCustId;
            XX_CompNn = xCompNaae;
        }

        public String FetchData(ref string ErrorMsg)
        {
            string json = "";
            Data_Load(ref json, ref ErrorMsg);
        
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(json);

            json = JsonConvert.SerializeXmlNode(doc);            
            return json;
        }
        private void setServerDetails_EF_dcs(string Cust_ID, string Comp_Name, ref string DBServer, ref string DBName, ref string DBuser, ref string DBPassword, ref string AccessPath, ref string loc_code, ref string DBtype, ref string result_message)
        {
            try
            {
                UserDATA data = new UserDATA();
                _DBContext db = new _DBContext();

                DateTime date = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
                                
                var custdata = db.Cust_Recs.Where(x => x.Cust_ID == Cust_ID && x.To_dt >= date && x.Comp_Name == Comp_Name).FirstOrDefault();

                DBServer = custdata.DBServer;
                DBName = custdata.DBName;
                DBuser = custdata.DBuser;
                DBtype = custdata.DBtype;
                DBPassword = data.SQL_PWD;

                //added at 12/07/2022
                //if (!DBNull.Value.Equals(custdata.PIP))
                //if (custdata.PIP is not null)
                if (custdata.PIP != null)
                {
                    if (custdata.PIP.ToString().Trim() != "")
                    {
                        DBServer = custdata.PIP.ToString().Trim();
                    }
                }
                //12/07/2022

                if (custdata.Pwd_Policy.ToString() == "POLICY001")
                {
                    DBPassword = "Kajal@123";
                }
                if (custdata.Pwd_Policy.ToString() == "POLICY002")
                {
                    DBPassword = "Trushachampak@123";
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void Data_Load(ref String pXml, ref string ErrorMsg)
        {
            try
            {
                ServerDetails serl = new ServerDetails();
                string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
                serl.setServerDetails_EF_dcs(XX_CustID, XX_CompNn, ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

                bool cc = new ThinkHPTLM.Reports(true, null, null, null, null, null, null, null).
                    GetReportForOnLineWeb(true, DBuser, DBPassword, DBtype, DBName, DBServer, DBuser, DBPassword, ref pXml);            
                
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }

        }
        public class Dashboard
        {
            public double Earnings { get; set; }
            public double RunningSaleAmount { get; set; }
            public double Total { get; set; }
            public double Expenses { get; set; }
            public int TotalOrders { get; set; }
            public int CancelOrders { get; set; }
            public List<CIOD> CIOData { get; set; }
            public double Dinein { get; set; }
            public double Takeaway { get; set; }
            public double Delivery { get; set; }
            public double Corporate { get; set; }
            public double Drive_Th { get; set; }
            public string Day_Last_Invoice_Detail { get; set; }
            public string ToDayLast_Sync_DT { get; set; }

            public class CIOD
            {
                public string Cashier { get; set; }
                public string In_DT { get; set; }
                public string Out_DT { get; set; }
            }

        }

        //async Task<Dashboard> Getdata()
        //{
        //    string ErrorMsg= "";
        //    try
        //    {
        //        Dashboard model =new Dashboard();
        //       String X_Error = "";

        //        ServerDetails serl = new ServerDetails();

        //        string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";

        //        InsertTest(XX_CustID, XX_CompNn);

        //        serl.setServerDetails_EF_dcs(XX_CustID, XX_CompNn, ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

        //        InsertTest("001" + XX_CustID + XX_CompNn, (DBServer + DBName + DBPassword + DBtype));

        //        double X_ToDayTotalSaleAmount = 0;
        //        double X_ToDayRunningSaleAmount = 0;
        //        string X_ToDayCashierINOutDetails = "";
        //        double X_RDineIn_Amt = 0;
        //        double X_RTakeAway_Amt = 0;
        //        double X_RDelivery_Amt = 0;
        //        double X_RCorporate_Amt = 0;
        //        double X_RDriveTh_Amt = 0;
        //        long X_LastInvNo = 0;
        //        string X_LastInvoiceDT = "";
        //        string X_ToDayLast_Sync_DT = "";
        //        double X_ToDayTotalExpenseAmount = 0;
        //        short X_ToDayTotalOrders = 0;
        //        short X_ToDayVoidItems = 0;
                                
        //        var cc = new ThinkHPTLM.Reports(true, null, null, null, null, null, null, null).GetReportForOnLineWeb(
        //            true, DBuser, DBPassword, DBtype, DBName, DBServer, DBuser, DBPassword,
        //            ref X_ToDayTotalSaleAmount,
        //            ref X_ToDayRunningSaleAmount,
        //            ref X_ToDayCashierINOutDetails,
        //            ref X_RDineIn_Amt,
        //            ref X_RTakeAway_Amt,
        //            ref X_RDelivery_Amt,
        //            ref X_RCorporate_Amt,
        //            ref X_RDriveTh_Amt,
        //            ref X_LastInvNo,
        //            ref X_LastInvoiceDT,
        //            ref X_ToDayLast_Sync_DT,
        //            ref X_ToDayTotalExpenseAmount,
        //            ref X_ToDayTotalOrders,
        //            ref X_ToDayVoidItems,
        //            ref X_Error
        //            );

        //        //var a = new ThinkHPTLM.Reports(true, DBuser, DBPassword, DBtype, DBName, DBServer, DBuser, DBPassword).GetReportForOnLineWeb(
        //        //    ref X_ToDayTotalSaleAmount,
        //        //    ref X_ToDayRunningSaleAmount,
        //        //    ref X_ToDayCashierINOutDetails,
        //        //    ref X_RDineIn_Amt,
        //        //    ref X_RTakeAway_Amt,
        //        //    ref X_RDelivery_Amt,
        //        //    ref X_RCorporate_Amt,
        //        //    ref X_RDriveTh_Amt,
        //        //    ref X_LastInvNo,
        //        //    ref X_LastInvoiceDT,
        //        //    ref X_ToDayLast_Sync_DT,
        //        //    ref X_ToDayTotalExpenseAmount,
        //        //    ref X_ToDayTotalOrders,
        //        //    ref X_ToDayVoidItems,
        //        //    ref X_Error
        //        //    );

        //        InsertTest("002" + XX_CustID + XX_CompNn, (DBServer + DBName + DBPassword + DBtype));

        //        var d = X_ToDayTotalSaleAmount + "," + X_ToDayRunningSaleAmount + "," + X_ToDayCashierINOutDetails + "," + X_RDineIn_Amt + "," + X_RTakeAway_Amt + "," + X_RDelivery_Amt + "," + X_RCorporate_Amt + "," +
        //         X_RDriveTh_Amt + "," + X_LastInvNo + "," + X_LastInvoiceDT + "," + X_ToDayLast_Sync_DT + "," + X_ToDayTotalExpenseAmount + "," +
        //         X_ToDayTotalOrders + "," + X_ToDayVoidItems;
        //        //string X_CashierINOutDetails = "";

        //        //bool X_bool = a.ToDayCashierINOutDetails(ref X_CashierINOutDetails);
        //        //var newlist = new List<Dashboard.CIOD>();
        //        //if (X_bool)
        //        //{
        //        //    DataSet ds = new DataSet();
        //        //    ds.ReadXml(new XmlTextReader(new StringReader(X_CashierINOutDetails)));

        //        //    var newdata = new Dashboard.CIOD();                   
        //        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //        //    {
        //        //        newdata.Cashier = dr["UserName"].ToString();
        //        //        newdata.In_DT = dr["Cashier_IN_Datetime"].ToString();
        //        //        newdata.In_DT = dr["Cashier_IN_Datetime"].ToString();
        //        //    }
        //        //    newlist.Add(newdata);                    
        //        //}
        //        //string temp ="DineIn "+ X_Dine_Amt.ToString() + "TAKEAWAY " + X_Take_Away_Amt.ToString() + "DELIVERY " + X_Delivery_Amt.ToString() + X_Corporate_Amt.ToString() + X_Drive_Throught_Amt.ToString();

        //        InsertTest("003" + XX_CustID + XX_CompNn, (DBServer + DBName + DBPassword + DBtype) + d);

        //        model = new Dashboard
        //        {
        //            TotalOrders = Convert.ToInt32(X_ToDayTotalOrders),
        //            Earnings = X_ToDayTotalSaleAmount,
        //            RunningSaleAmount = X_ToDayRunningSaleAmount,
        //            Total = X_ToDayTotalSaleAmount + X_ToDayRunningSaleAmount,
        //            Expenses = X_ToDayTotalExpenseAmount,
        //            CancelOrders = int.Parse(X_ToDayVoidItems.ToString()),
        //            //CIOData = newlist,
        //            Dinein = X_RDineIn_Amt,
        //            Takeaway = X_RTakeAway_Amt,
        //            Delivery = X_RDelivery_Amt,
        //            Corporate = X_RCorporate_Amt,
        //            Drive_Th = X_RDriveTh_Amt,
        //            Day_Last_Invoice_Detail = X_LastInvNo + ", " + X_LastInvoiceDT,
        //            ToDayLast_Sync_DT = X_ToDayLast_Sync_DT
        //        };

        //        return await Task.FromResult(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMsg = ex.Message;
        //        return null;
        //    }            
        //}
    }
}

