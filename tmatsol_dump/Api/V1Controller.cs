using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using tmatsol_dump.Models;
using tmatsol_dump.Controllers;
using tmatsol_dump.Models.Web_Report;
using tmatsol_dump.Models.Web_Report.DAL;
using tmatsol_dump.Api.Token;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Web.Helpers;

namespace tmatsol_dump.Api
{
    public class V1Controller : ApiController
    {
        public class required_api_response_format
        {
            public  string responseCode="";
            public  string responseMessage="";
            public JObject responseData;
            public Boolean result = false;
        }
        
        [System.Web.Http.HttpGet]
        public string Basic1()
        {
            return "Hello World";
        }
        public static string[] Filter()
        {
            string[] List = { "Cashier Date", "Settled Date", "Sale Date" };        
            return List;
        }

        public static string[] ReportName()
        {
            string[] List = { 
                "Bill Statement",
                "Bill Statement With Item",
                "Bill Wise Discount Report",
                "Consolidated Summary",
                "Hour Summary",
                "Item Value Summary(ItemName)",
                "Item Value Summary(ItemName+Rate)",
                "Item Wise Discount Report",
                "Location Bill Wise",
                "Location Summary",
                "Location+Date Summary",
                "Pending bill report",
                "Sale Summary",
                "Void / Return item report"
            };
            //ViewData["Filter"] = Filter;

            return List;
        }
        public class ReportParam {
            public string DcsFrTime { get; set; }
            public string DcsToTime { get; set; }
            public string DcsFrDt { get; set; }
            public string DcsToDt { get; set; }
            public string filter { get; set; }
            public string ReportName { get; set; }
            public string Comp_Name { get; set; }
            public string CustID { get; set; }
        }

       
    [System.Web.Http.Route("api/V1/POSRestroReport")]
        [System.Web.Http.HttpPost]
        //00.00,23:59,2020-02-01,2023-02-01,Cashier Date,Bill Statement,POS Restro,xxx123xxx
        //public IHttpActionResult POSRestroReport(string DcsFrTime,string DcsToTime,string DcsFrDt,string DcsToDt,string filter,string ReportName, string Comp_Name,string CustID)
        public IHttpActionResult POSRestroReport(ReportParam rpm)
        {
            var re = Request;
            var headers = re.Headers;

            string token;

            if (headers.Contains("token"))
            {
                token = headers.GetValues("token").First();
            }
            else
            {
                required_api_response_format format = new required_api_response_format();

                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! token Not Defined at Header";
                format.responseData = null;
                format.result = false;
           
                return Json(format);
            }

            Token_Master tm = new Token_Master();
            string x_msg = "";

            if (token == "" || token == null)
            {
                required_api_response_format format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Please pass token";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            if (tm.user_Token_InMst(token, ref x_msg) == false)
            {
                required_api_response_format format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            string[] A = Filter();
            string[] B = ReportName();
            if (A.Contains(rpm.filter)==false || B.Contains(rpm.ReportName)==false)
            {
                required_api_response_format format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Invalid Filter or Report Name !!!";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            string XError = "";
            try
            {
                var From_Time = rpm.DcsFrTime;
                var To_Time = rpm.DcsToTime;

                var From_date = (Convert.ToDateTime(rpm.DcsFrDt)).ToString("dd-MMM-yyyy");
                var To_date = (Convert.ToDateTime(rpm.DcsToDt)).ToString("dd-MMM-yyyy");//Convert.ToDateTime(frm["To"].ToString("dd-MMM-yyyy")); 
                //To_date = To_date + " 23:59";
                From_date = From_date + " " + From_Time;
                To_date = To_date + " " + To_Time;

                string XSName = "";
                string XDbNane = "";

                string XSQL_USER_NAME = "";
                string XSQL_PWD = "";

                DataSet Ds = new DataSet();

                string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
                ServerDetails ser = new ServerDetails();
                ser.setServerDetails_EF_dcs(rpm.CustID, rpm.Comp_Name, ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
                DAL Xdal = new DAL();
                XDbNane = DBName;
                XSName = DBServer;

                XSQL_USER_NAME = DBuser;
                XSQL_PWD = DBPassword;

                XError = "";

                Ds = Report_DS(rpm.CustID, rpm.Comp_Name, rpm.ReportName, From_date, To_date, rpm.filter, XSName, XDbNane, XSQL_USER_NAME, XSQL_PWD, ref XError);

                required_api_response_format format = new required_api_response_format();
                format.responseCode = "200";
                format.responseMessage = "Success !!!";
                var output = JsonConvert.SerializeObject(Ds);
                JObject resultjson = JObject.Parse(output);
                format.responseData = resultjson;
                format.result = true;
                return Json(format);

            }
            catch (Exception e)
            {
                required_api_response_format format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = e.Message;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
        }
        public string[] CompanyList(string ID)
        {
            string str = "select Comp_Name from cust_rec where Cust_ID = '" + ID + "'";
            SqlConnection con = new SqlConnection("Data Source=54.36.166.189;Initial Catalog=tmatsol_Report_DB_Entry;User ID=tmatsreport;Password=iLu4j22#;");
            con.Open();
            SqlCommand cmd = new SqlCommand(str, con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            string[] List = null;
            if (dt.Rows.Count > 0)
            {
                List = dt.Rows[0].ItemArray.Select(x => x.ToString().Trim()).ToArray();
            }
            return List;
        }
        public DataSet Report_DS(string CustID, string Comp_Name, string XReportName, string From_date, string To_date, string filter, string XServerName, string XDbName, string XSQL_USER, string XSQL_PWD, ref string X_Error)
        {
            string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
            ServerDetails ser = new ServerDetails();

            ser.setServerDetails_EF_dcs(CustID, Comp_Name, ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
            DBName = XDbName;
            DBServer = XServerName;
            if (XSQL_USER == null || XSQL_USER == "")
            {
                XSQL_USER = "sa";

            }

            if (XSQL_PWD == null || XSQL_PWD == "")
            {
                XSQL_PWD = "kajal3792kajal";

            }
            DataSet xRetundDS = new DataSet();

            try
            {
                var a = new ThinkHPTLM.Reports(true, DBuser, DBPassword, DBtype, DBName, DBServer, XSQL_USER, XSQL_PWD);

                if (XReportName == "Sale Summary")
                {
                    xRetundDS = a.Sale_Summary2(From_date, To_date, 0, "", false, filter, ref X_Error, true, false);

                }

                if (XReportName == "Bill Statement")
                {
                    Boolean Xemp = false;
                    //xRetundDS = a.Sale_Summary2_Bill_Statement(From_date, To_date, filter, ref Xemp, ref X_Error,);
                    xRetundDS = a.Sale_Summary2_Bill_Statement(From_date, To_date, filter, ref Xemp, "", ref X_Error);
                };

                if (XReportName == "Bill Statement With Item")
                {
                    xRetundDS = a.Sale_Summary2_Bill_Statement_Item_Wise(From_date, To_date, filter, Loc_code, ref X_Error);
                };

                if (XReportName == "Hour Summary")
                {
                    xRetundDS = a.Sale_Hour_Summary(From_date, To_date, 0, "", false, ref X_Error, 0);
                };

                if (XReportName == "Consolidated Summary")
                {
                    xRetundDS = a.Consolidated_Summary(From_date, To_date, 0, "", false, filter, ref X_Error, true, false);
                    //X_Ds = X_Sale_Conso_Summary.Consolidated_Summary(X_From, X_To, 0, "", False, "Cashier Date", X_Error, True, False)
                }

                if (XReportName == "Item Value Summary(ItemName)")
                {
                    xRetundDS = a.Item_Summary(From_date, To_date, false, filter, "", ref X_Error,"");
                    //Item_Summary(Format(Dt_From.Value, "dd-MMM-yyyy 00:00").Trim, Format(Dt_To.Value, "dd-MMM-yyyy 23:59:59").Trim, True, X_Date_Filter, "", X_Error)

                };

                if (XReportName == "Item Value Summary(ItemName+Rate)")
                {
                    xRetundDS = a.Item_Summary_Rate(From_date, To_date, true, 0, filter, ref X_Error,"");
                    //Item_Summary_Rate(Format(Dt_From.Value, "dd-MMM-yyyy 00:00").Trim, Format(Dt_To.Value, "dd-MMM-yyyy 23:59:59").Trim, True, 0, X_Date_Filter, X_Error)
                };

                if (XReportName == "Location Bill Wise")
                {
                    //Location+Date Summary
                    //Location Summary
                    //Bill Wise'
                    xRetundDS = a.Location_Date_Summary_OR_Detail(From_date, To_date, 0, "", false, filter, ref X_Error, true, false, "Bill Wise");
                    //Item_Summary_Rate(Format(Dt_From.Value, "dd-MMM-yyyy 00:00").Trim, Format(Dt_To.Value, "dd-MMM-yyyy 23:59:59").Trim, True, 0, X_Date_Filter, X_Error)
                };

                if (XReportName == "Location Summary")
                {
                    //Location+Date Summary
                    //Location Summary
                    //Bill Wise'
                    xRetundDS = a.Location_Date_Summary_OR_Detail(From_date, To_date, 0, "", false, filter, ref X_Error, true, false, "Location Summary");
                    //Item_Summary_Rate(Format(Dt_From.Value, "dd-MMM-yyyy 00:00").Trim, Format(Dt_To.Value, "dd-MMM-yyyy 23:59:59").Trim, True, 0, X_Date_Filter, X_Error)
                };

                if (XReportName == "Location+Date Summary")
                {
                    //Location+Date Summary
                    //Location Summary
                    //Bill Wise'
                    xRetundDS = a.Location_Date_Summary_OR_Detail(From_date, To_date, 0, "", false, filter, ref X_Error, true, false, "Location+Date Summary");
                    //Item_Summary_Rate(Format(Dt_From.Value, "dd-MMM-yyyy 00:00").Trim, Format(Dt_To.Value, "dd-MMM-yyyy 23:59:59").Trim, True, 0, X_Date_Filter, X_Error)
                };

                if (XReportName == "Bill Wise Discount Report")
                {
                    //Bill Wise Discount Report','Sale_Bill_Statement_Discounted_Bill'
                    xRetundDS = a.Sale_Bill_Statement_Discounted_Bill(From_date, To_date, ref X_Error, "Without Items", filter,"");
                };

                if (XReportName == "Item Wise Discount Report")
                {

                    xRetundDS = a.Sale_Bill_Statement_Discounted_Bill(From_date, To_date, ref X_Error, "With Items", filter,"");
                };

                if (XReportName == "Void / Return item report")
                {
                    xRetundDS = a.Void_Cancel_Items(From_date, To_date, 0, 0, ref X_Error,"");
                };

                if (XReportName == "Pending bill report")
                {
                    xRetundDS = a.M_Proc_Report_Pending_Bill(true, From_date, To_date, ref X_Error, "","");
                };
            }
            catch (Exception ex)
            {
                X_Error = ex.Message.ToString() + " while at " + XReportName + DBuser;
            }
            return xRetundDS;
        }
        
        [System.Web.Http.Route("api/V1/Report_Logout")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult Report_Logout()
        {
            var re = Request;
            var headers = re.Headers;

            string username;
            string tokenid;

            if (headers.Contains("username") && headers.Contains("token"))
            {                
                username = headers.GetValues("username").First();
                tokenid = headers.GetValues("token").First();
                
                if (tokenid == "" || tokenid == null)
                {
                    required_api_response_format format = new required_api_response_format();
                    format.responseCode = "400";
                    format.responseMessage = "Please pass token";
                    format.responseData = null;
                    format.result = false;
                    return Json(format);
                }

                if (username == "" || username == null)
                {
                    required_api_response_format format = new required_api_response_format();
                    format.responseCode = "400";
                    format.responseMessage = "Please pass username";
                    format.responseData = null;
                    format.result = false;
                    return Json(format);
                }
            }
            else
            {
                required_api_response_format format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! username OR token Not Defined at Header";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            Token_Master tm = new Token_Master();
            string Msg="";
            
            tm.user_Token_InMst(tokenid, ref Msg);
            
            if (Msg.Trim() !="")
            {
                required_api_response_format format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = Msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            tm.Token_Delete(username, tokenid, ref Msg);
            
            if (Msg.Trim()!="")
            {
                required_api_response_format format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = Msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            else
            {
                required_api_response_format format = new required_api_response_format();
                format.responseCode = "200";
                format.responseMessage = "Logout Successfully !!!";
                format.responseData = null;
                format.result = true;
                return Json(format);
            }
        }
        
        [System.Web.Http.Route("api/V1/Report_Login")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult Report_Login()
        {
            required_api_response_format format = new required_api_response_format();

            var re = Request;
            var headers = re.Headers;

            string username;
            string password;

            if (headers.Contains("username") && headers.Contains("password"))
            {
                username = headers.GetValues("username").First();
                password = headers.GetValues("password").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! username OR password Not Defined at Header";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            string ID = "";
            string Vaildity = "";
            DateTime VaildityDt = DateTime.Now;

            string str = "select * from cust_rec where Login_ID = '" + username + "' and login_PWD = '" + password + "'";
            SqlConnection con = new SqlConnection("Data Source=54.36.166.189;Initial Catalog=tmatsol_Report_DB_Entry;User ID=tmatsreport;Password=iLu4j22#;");
            con.Open();
            SqlCommand cmd = new SqlCommand(str, con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    ID = row["Cust_ID"].ToString();
                    VaildityDt = (DateTime)row["To_dt"];
                    Vaildity = VaildityDt.ToString("dd-MMM-yyyy");
                }
                if (DateTime.Now >= VaildityDt)
                {
                    cmd.Dispose();
                    con.Close();

                    format = new required_api_response_format();
                    format.responseCode = "400";
                    format.responseMessage = "Validity Expired at " + Vaildity;
                    format.responseData = null;
                    format.result = false;
                    return Json(format);
                }
                cmd.Dispose();
                con.Close();

                string tokenID = "";
                string msg="";
                Token_Master tm = new Token_Master();
                tm.token_inserted(username,ref tokenID,ref msg);

                if (msg.Trim() !="")
                {
                    format = new required_api_response_format();
                    format.responseCode = "400";
                    format.responseMessage = msg;
                    format.responseData = null;
                    format.result = false;
                    return Json(format);
                }

                //var responseData=Json(new
                //{
                //    result = true,
                //    Msg = "Login Sucessful !!!",
                //    ID = ID,
                //    company = CompanyList(ID),
                //    Vaildity = Vaildity,
                //    token= tokenID
                //});
                var X_responseData = (new responseData
                {
                    ID = ID,
                    company = CompanyList(ID),
                    Vaildity = Vaildity,
                    token = tokenID
                });


                format = new required_api_response_format();
                format.responseCode = "200";
                format.responseMessage = "Login Sucessful !!!";

                var output = JsonConvert.SerializeObject(X_responseData);
                JObject resultjson = JObject.Parse(output);
                format.responseData = resultjson;

                format.result = true;
                return Json(format);
                //--------------------24/10/2022--------
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Invalid User Name or Password!!!";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
        }
        class responseData
        {
            public string ID;
            public string[] company;
            public string Vaildity;
            public string token;
        }
       //Also Use for Customer/Company and Id Validation..........
        public DataTable ListRebound(string CUSTID, string dbname,ref string x_msg,ref bool x_result)
        {
            try
            {
                DataTable dtt = new DataTable();

                //con.ConnectionString = con.ConnectionString.Replace("XpasswordX", "kajal3792kajal");
                SqlConnection con = new SqlConnection("Data Source=54.36.166.189;Initial Catalog=tmatsol_Report_DB_Entry;User ID=tmatsreport;Password=iLu4j22#;");

                con.Open();
                string X_str = "select * from Cust_Rec where Cust_ID = '" + CUSTID.Trim() + "' and Comp_Name='" + dbname.Trim() + "' ";

                SqlCommand cmd = new SqlCommand(X_str, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dtt);
                sda.Dispose();
                con.Close();

                if (dtt.Rows.Count ==0)
                {
                    x_msg = "Invalid Company Or ID !!!!";
                    x_result =true;
                }

                return dtt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [System.Web.Http.Route("api/V1/Report_Login_Next")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult Report_Login_Next()
        {
            required_api_response_format format = new required_api_response_format();

            var re = Request;
            var headers = re.Headers;

            string username;
            string password;
            string token;
            string ID;
            string From_dt = "";
            string To_dt = "";
            string To_dtVal = "";
            string DaysRemaining = "";
            string Comp_Name;

            //if (headers.Contains("company_name") && headers.Contains("username") && headers.Contains("passcode") && headers.Contains("ID"))
            //{
            //    Comp_Name = headers.GetValues("company_name").First();
            //    username = headers.GetValues("username").First();
            //    password = headers.GetValues("passcode").First();
            //    ID = headers.GetValues("ID").First();
            //}
            if (headers.Contains("company_name") && headers.Contains("username") && headers.Contains("passcode") && headers.Contains("token") && headers.Contains("ID"))
            {
                Comp_Name = headers.GetValues("company_name").First();
                username = headers.GetValues("username").First();
                password = headers.GetValues("passcode").First();
                ID = headers.GetValues("ID").First();
                token = headers.GetValues("token").First();

            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! company_name Or username Or passcode Or token Or ID Not Defined at Header";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            
            Token_Master tm = new Token_Master();
            string x_msg = "";
            if (token=="" || token == null)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Please pass token";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            if (tm.user_Token_InMst(token,ref x_msg)==false)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
            string Login_ID = "";
            DataTable dt = new DataTable();
           
            Boolean x_result = false;
            dt = ListRebound(ID, Comp_Name,ref x_msg,ref x_result);
            if (x_result)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            string conn = null;

            foreach (DataRow row in dt.Rows)
            {
                Login_ID = row["Login_ID"].ToString();
                From_dt = ((DateTime)row["From_dt"]).ToString("dd-MMM-yy");
                To_dt = ((DateTime)row["To_dt"]).ToString("dd-MMM-yy");
                To_dtVal = ((DateTime)row["To_dt"]).ToString();
                DateTime today = DateTime.Now;
                DateTime Todate = (DateTime)row["To_dt"];
                DaysRemaining = "Left Day/s" + Math.Round((Todate - today).TotalDays, 0).ToString();
                DBServer = row["DBServer"].ToString();
                DBName = row["DBName"].ToString();
                DBuser = row["DBuser"].ToString();
                DBtype = row["DBtype"].ToString();

                //added at 12/07/2022
                if (!DBNull.Value.Equals(row["PIP"]))
                {
                    if (row["PIP"].ToString() != "")
                    {
                        DBServer = row["PIP"].ToString();
                    }
                }
                //12/07/2022

                DBPassword = "kajal3792kajal";
                if (row["pwd_policy"].ToString() == "POLICY001")
                {
                    DBPassword = "Kajal@123";
                }
                if (row["pwd_policy"].ToString() == "POLICY002")
                {
                    DBPassword = "Trushachampak@123";
                }



                Comp_Name = row["Comp_Name"].ToString().Trim();

                conn = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;
                dt.Dispose();
            }
            try
            {
                SqlConnection con = new SqlConnection(conn);
                con.Open();
                SqlCommand cmd = new SqlCommand("select Count(*) from fi_usermast where First_name='" + username + "' and Authorize_Code='" + password + "' and Web_Report_Require='Yes';", con);


                int i = Convert.ToInt16(cmd.ExecuteScalar());
                if (i > 0)
                {
                    DateTime Dbt = Convert.ToDateTime(To_dtVal);

                    DateTime Today = DateTime.Now;
                    // '10no   7 '
                    //7   10
                    if (Dbt < Today)
                    {
                        format = new required_api_response_format();
                        format.responseCode = "400";
                        format.responseMessage = "Your Report Utility Validity is Expired!!";
                        format.responseData = null;
                        format.result = false;
                        return Json(format);
                    }
                    else
                    {
                        ////------------------------Log Record---------------------------
                        //SqlConnection connn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);

                        //connn.Open();
                        //string strinst = "insert into Report_Login_History(UniqueID,InSessionLog,SessionLogId) values('" + Session["CUSTID"] + "','" + DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss") + "','" + Login_ID + "/" + obj.First_name + "')";
                        //SqlCommand cmdd = new SqlCommand(strinst, connn);
                        //cmdd.ExecuteNonQuery();
                        //cmdd.Dispose();
                        //connn.Close();
                        ////-------------------------End---------------------------------
                        con.Close();
                        //return Json(new
                        //{
                        //    result = true,
                        //    Msg = "Login Sucessful !!!",
                        //    username = username,
                        //    From_dt = From_dt,
                        //    To_dt = To_dt,
                        //    To_dtVal = To_dtVal,
                        //    DaysRemaining = DaysRemaining,
                        //    Comp_Name = Comp_Name
                        //});
                        var XReport_Login_Next=new Report_Login_Next_
                        {
                            username = username,
                            From_dt = From_dt,
                            To_dt = To_dt,
                            To_dtVal = To_dtVal,
                            DaysRemaining = DaysRemaining,
                            Comp_Name = Comp_Name
                        };
                        format = new required_api_response_format();
                        format.responseCode = "200";
                        format.responseMessage = "Login Sucessful !!!";

                        var output = JsonConvert.SerializeObject(XReport_Login_Next);
                        JObject resultjson = JObject.Parse(output);
                        format.responseData = resultjson;

                        format.result = true;
                        return Json(format);
                    }
                }
                else
                {
                    con.Close();
                    format = new required_api_response_format();
                    format.responseCode = "400";
                    format.responseMessage = "Please Insert Valid User Name And Password!!";
                    format.responseData = null;
                    format.result = false;
                    return Json(format);
                }
            }
            catch (Exception e)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "DBServer :" + DBServer + " ,Error : " + e.Message.ToString();
                format.responseData = null;
                format.result = false;
                return Json(format);               
                //throw e;
            }
        }
        public class Report_Login_Next_
        {
            public string username;
            public string From_dt;
            public string To_dt;
            public string To_dtVal;
            public string DaysRemaining;
            public string Comp_Name;
        }           
                        
        [System.Web.Http.Route("api/V1/Dashboard")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult DashBoard()
        {
            required_api_response_format format = new required_api_response_format();
            string json;
            string Error_str = "";

            var re = Request;
            var headers = re.Headers;
            string Comp_Name;
            string ID;
            string token;

            if (headers.Contains("company_name") && headers.Contains("ID") && headers.Contains("token"))
            {
                Comp_Name = headers.GetValues("company_name").First();
                ID = headers.GetValues("ID").First();
                token = headers.GetValues("token").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! company_name OR ID OR token Not Defined at Header ";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            Token_Master tm = new Token_Master();
            string x_msg = "";
            if (token == "" || token == null)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Please pass token";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            if (tm.user_Token_InMst(token, ref x_msg) == false)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            //--------------------Customer/Company and ID Validation------------------------------------
            Boolean x_result = false;
            var dt = ListRebound(ID, Comp_Name, ref x_msg, ref x_result);
            if (x_result)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //--------------------Customer/Company and ID Validation------------------------------------

            DashBoardDataConfig XFetchData = new DashBoardDataConfig(ID, Comp_Name);
            json = XFetchData.FetchData(ref Error_str);
            //JObject resultjson = JObject.Parse(json);
            //return Json(resultjson);
            format = new required_api_response_format();
            format.responseCode = "200";
            format.responseMessage = "success !!!";

            JObject resultjson = JObject.Parse(json);
            format.responseData = resultjson;
                        
            format.result = true;
            return Json(format);

        }
        [System.Web.Http.Route("api/V1/Chart")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult Chart(short numofDays)
        {

            required_api_response_format format = new required_api_response_format();
            var re = Request;
            var headers = re.Headers;
            string Comp_Name;
            string ID;

            if (headers.Contains("company_name") && headers.Contains("ID"))
            {
                Comp_Name = headers.GetValues("company_name").First();
                ID = headers.GetValues("ID").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! company_name OR ID Not Defined at Header";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //token
            string token;

            if (headers.Contains("token"))
            {
                token = headers.GetValues("token").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! token Not Defined at Header ";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            Token_Master tm = new Token_Master();
            string x_msg = "";
            if (token == "" || token == null)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Please pass token";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            if (tm.user_Token_InMst(token, ref x_msg) == false)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //token
            //--------------------Customer/Company and ID Validation------------------------------------
            Boolean x_result = false;
            var dt = ListRebound(ID, Comp_Name, ref x_msg, ref x_result);
            if (x_result)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //--------------------Customer/Company and ID Validation------------------------------------

            string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";

            var ds = new DataSet();
            ServerDetails ser = new ServerDetails();
            //ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
            ser.setServerDetails_EF_dcs(ID, Comp_Name, ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

            var x_error = "";
            ds = new ThinkHPTLM.Reports(true, null, null, null, null, null, null, null).WeekDayTotalOrders(true, DBuser, DBPassword, DBtype, DBName, DBServer, DBuser, DBPassword, ref numofDays, ref x_error);

            List<WeekDay> result=new List<WeekDay>();

            if (numofDays == 24)
            {
                result = ds.Tables[0].AsEnumerable()
                            .Select(dataRow => new WeekDay
                            {
                                SaleDay = dataRow.Field<string>("SaleDate"),
                                Amount = dataRow.Field<double>("LastYear")
                            }).ToList();
            }
            else
            {
                result = ds.Tables[0].AsEnumerable()
                            .Select(dataRow => new WeekDay
                            {
                                SaleDay = dataRow.Field<string>("SaleDate"),
                                Amount = dataRow.Field<double>("Amount")
                            }).ToList();
            }
                        
            format = new required_api_response_format();
            format.responseCode = "200";
            format.responseMessage = "success !!!";

            string output = "{\"ResponseData\":";
            output += JsonConvert.SerializeObject(result);
            output +="}";

            JObject resultjson = JObject.Parse(output);
            format.responseData = resultjson;
            
            format.result = true;
            return Json(format);
            //return Json(result);
        }
        [System.Web.Http.Route("api/V1/GetTables")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetTables(short TABLE_GRP_CD)//USE TO FILL Table List
        {
            required_api_response_format format = new required_api_response_format();
            var re = Request;
            var headers = re.Headers;
            string Comp_Name;
            string ID;

            if (headers.Contains("company_name") && headers.Contains("ID"))
            {
                Comp_Name = headers.GetValues("company_name").First();
                ID = headers.GetValues("ID").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!!company_name OR ID Not Defined at Header ";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //token
            string token;

            if (headers.Contains("token"))
            {
                token = headers.GetValues("token").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! token Not Defined at Header ";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            Token_Master tm = new Token_Master();
            string x_msg = "";
            if (token == "" || token == null)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Please pass token";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            if (tm.user_Token_InMst(token, ref x_msg) == false)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //token
            //--------------------Customer/Company and ID Validation------------------------------------
            Boolean x_result = false;
            var xdt = ListRebound(ID, Comp_Name, ref x_msg, ref x_result);
            if (x_result)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //--------------------Customer/Company and ID Validation------------------------------------

            string conn = "";
            try
            {
                string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
                ServerDetails ser = new ServerDetails();
                //ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
                ser.setServerDetails_EF_dcs(ID, Comp_Name, ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

                conn = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;

                DbProviderFactory f = DbProviderFactories.GetFactory("System.Data.SqlClient");
                DataTable dt = new DataTable();

                using (DbConnection connection = f.CreateConnection())
                {
                    connection.ConnectionString = conn;
                    //Main(null);
                    //connection.ConnectionString = conn.ConnectionString;

                    connection.Open();

                    DbCommand command = f.CreateCommand();
                    // command.CommandText = "select * from FI_Table_Master";
                    //string str = "SELECT Table_no,Nos_Of_Persons,FI_Table_Template_Master_sr_no ,isnull((select top 1  D_C_P_REF_NO from FI_giishead where D_C_P_REF_NO = a.Table_no and OrderType = 1),'NA') as [Table_Occupuied],IOT,(select MIN(Print_Seq_Number_Bill) from FI_giishead where D_C_P_REF_NO=a.Table_no ) as [Bill_Printed] FROM FI_Table_Master A, FI_Table_Template_Master B WHERE A.FI_Table_Template_Master_sr_no = B.SR_NO  AND TABLE_GRP_CD = '" + Session["TABLE_GRP_CD"].ToString() + "' order by FI_Table_Template_Master_sr_no";
                    string str = "SELECT Table_no,Nos_Of_Persons,FI_Table_Template_Master_sr_no ,isnull((select top 1  D_C_P_REF_NO from FI_giishead where D_C_P_REF_NO = a.Table_no and OrderType = 1),'NA') as [Table_Occupuied],IOT,(select MIN(Print_Seq_Number_Bill) from FI_giishead where D_C_P_REF_NO=a.Table_no ) as [Bill_Printed],(select pos_x from FI_Table_Template_Master where sr_no =a.FI_Table_Template_Master_sr_no) as [X],(select pos_y from FI_Table_Template_Master where sr_no = a.FI_Table_Template_Master_sr_no) as [Y] FROM FI_Table_Master A, FI_Table_Template_Master B WHERE A.FI_Table_Template_Master_sr_no = B.SR_NO  AND TABLE_GRP_CD = '" + TABLE_GRP_CD + "' order by FI_Table_Template_Master_sr_no";
                    command.CommandText = str;
                    command.Connection = connection;

                    IDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    connection.Close();

                }

                //List<FI_Table_Master> data = new List<FI_Table_Master>();
                //data = (from DataRow dr in dt.Rows
                //        select new FI_Table_Master()
                //        {
                //            loc_code = dr["loc_code"].ToString(),
                //            Table_Grp_cd = Convert.ToInt32(dr["Table_Grp_cd"]),
                //            Table_no = dr["Table_no"].ToString(),
                //            Nos_Of_Persons = Convert.ToInt32(dr["Nos_Of_Persons"]),
                //            Fi_Table_Template_Master_sr_no= Convert.ToInt32(dr["Fi_Table_Template_Master_sr_no"]),
                //            Image_File_Name= dr["Image_File_Name"].ToString(),
                //            IOT = Convert.ToInt32(dr["IOT"])
                //        }).ToList();

                List<FI_Table_My_Mst> data = new List<FI_Table_My_Mst>();

                data = (from DataRow dr in dt.Rows
                        select new FI_Table_My_Mst()
                        {
                            Table_no = dr["Table_no"].ToString(),
                            Nos_Of_Persons = Convert.ToInt32(dr["Nos_Of_Persons"]),
                            Fi_Table_Template_Master_sr_no = Convert.ToInt32(dr["Fi_Table_Template_Master_sr_no"]),
                            //Image_File_Name = dr["Image_File_Name"].ToString(),
                            Pos_X = Convert.ToInt16(Convert.ToInt16(dr["X"]) * 1.25),
                            Pos_Y = Convert.ToInt32(dr["Y"]),
                            Table_Occupuied = dr["Table_Occupuied"].ToString(),

                            //Bill_Printed=Convert.ToInt32(dr["Bill_Printed"]),
                            Bill_Printed = dr["Bill_Printed"].ToString(),
                            IOT = Convert.ToInt32(dr["IOT"])

                            // color = IsTableActive(dr["Table_no"].ToString())

                        }).ToList();

                //return Json(data);//PASS DATA TO AJAX
                format = new required_api_response_format();
                format.responseCode = "200";
                format.responseMessage = "success !!!";
                //var output = JsonConvert.SerializeObject(data);
                
                string output = "{\"ResponseData\":";
                output += JsonConvert.SerializeObject(data);
                output += "}";

                JObject resultjson = JObject.Parse(output);
                format.responseData = resultjson;
                format.result = true;
                return Json(format);
            }
            catch (Exception ex)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = ex.Message;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

        }
        [System.Web.Http.Route("api/V1/GetDineINmode")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetDineINmode()
        {
            required_api_response_format format = new required_api_response_format();
            var re = Request;
            var headers = re.Headers;
            string Comp_Name;
            string ID;

            if (headers.Contains("company_name") && headers.Contains("ID"))
            {
                Comp_Name = headers.GetValues("company_name").First();
                ID = headers.GetValues("ID").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! company_name OR ID Not Defined at Header ";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            //token
            string token;

            if (headers.Contains("token"))
            {
                token = headers.GetValues("token").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! token Not Defined at Header ";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            Token_Master tm = new Token_Master();
            string x_msg = "";
            if (token == "" || token == null)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Please pass token";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            if (tm.user_Token_InMst(token, ref x_msg) == false)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //token
            //--------------------Customer/Company and ID Validation------------------------------------
            Boolean x_result = false;
            var xdt = ListRebound(ID, Comp_Name, ref x_msg, ref x_result);
            if (x_result)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //--------------------Customer/Company and ID Validation------------------------------------

            string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
            ServerDetails ser = new ServerDetails();
            //ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
            ser.setServerDetails_EF_dcs(ID, Comp_Name, ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

            string con = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;

            SqlConnection conn = new SqlConnection(con);

            try
            {
                // var data = _db.fI_Caller_Masters.Where(x => x.Caller_Type == 0).ToList();
                DataTable dt = new DataTable();
                //String str = "select ref_no,OrderType,(select MODE_NAME from MODE_MASTER where MODE_CODE=a.OrderType) as [ModeTypeStr],Print_Seq_Number_Bill  from FI_giishead a  where OrderType =1 and D_C_P_REF_NO = N'" + Session["TableId"] + "'";
                String str = "select TABLE_GRP_CD, TABLE_GRP_NAME from FI_TABLE_GROUP_MASTER";

                conn.Open();
                SqlCommand cmd = new SqlCommand(str, conn);

                IDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();


                List<Old_Bill_Rec> data = new List<Old_Bill_Rec>();
                data = (from DataRow dr in dt.Rows
                        select new Old_Bill_Rec()
                        {
                            ref_no = Convert.ToInt32(dr["TABLE_GRP_CD"]),
                            ModeTypeStr = dr["TABLE_GRP_NAME"].ToString(),
                        }).ToList();


                conn.Close();

                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "success !!!";
                //var output = JsonConvert.SerializeObject(data);
                string output = "{\"ResponseData\":";
                output += JsonConvert.SerializeObject(data);
                output += "}";

                JObject resultjson = JObject.Parse(output);
                format.responseData = resultjson;
                format.result = true;
                return Json(format);

                //return Json(data);//PASS DATA TO AJAX
            }
            catch (Exception ex)
            {
                conn.Close();
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = ex.Message;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
        }
        [System.Web.Http.Route("api/V1/GetOldBillList")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetOldBillList(string TableId)
        {
            required_api_response_format format = new required_api_response_format();

            var re = Request;
            var headers = re.Headers;
            string Comp_Name;
            string ID;

            if (headers.Contains("company_name") && headers.Contains("ID"))
            {
                Comp_Name = headers.GetValues("company_name").First();
                ID = headers.GetValues("ID").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! company_name OR ID Not Defined at Header ";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            //token
            string token;

            if (headers.Contains("token"))
            {
                token = headers.GetValues("token").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! token Not Defined at Header ";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

            Token_Master tm = new Token_Master();
            string x_msg = "";
            if (token == "" || token == null)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Please pass token";
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            if (tm.user_Token_InMst(token, ref x_msg) == false)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //token
            //--------------------Customer/Company and ID Validation------------------------------------
            Boolean x_result = false;
            var xdt = ListRebound(ID, Comp_Name, ref x_msg, ref x_result);
            if (x_result)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //--------------------Customer/Company and ID Validation------------------------------------

            string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
            ServerDetails ser = new ServerDetails();
            //ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
            ser.setServerDetails_EF_dcs(ID, Comp_Name, ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

            string con = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;

            SqlConnection conn = new SqlConnection(con);

            try
            {
                // var data = _db.fI_Caller_Masters.Where(x => x.Caller_Type == 0).ToList();
                //SqlConnection conn = new SqlConnection(con);
                DataTable dt = new DataTable();
                String str = "select ref_no,OrderType,(select MODE_NAME from MODE_MASTER where MODE_CODE=a.OrderType) as [ModeTypeStr],Print_Seq_Number_Bill,Bill_Note  from FI_giishead a  where OrderType =1 and D_C_P_REF_NO = N'" + TableId + "'";

                conn.Open();
                SqlCommand cmd = new SqlCommand(str, conn);

                IDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();


                List<Old_Bill_Rec> data = new List<Old_Bill_Rec>();
                data = (from DataRow dr in dt.Rows
                        select new Old_Bill_Rec()
                        {
                            ref_no = Convert.ToInt32(dr["ref_no"]),
                            OrderType = Convert.ToInt32(dr["OrderType"]),
                            ModeTypeStr = dr["ModeTypeStr"].ToString(),
                            Print_Seq_Number_Bill = Convert.ToInt32(dr["Print_Seq_Number_Bill"]),
                            Bill_Note = dr["Bill_Note"].ToString()

                        }).ToList();

                conn.Close();

                conn.Close();
                format = new required_api_response_format();
                format.responseCode = "200";
                format.responseMessage = "success !!!";
                //var output = JsonConvert.SerializeObject(data);
                string output = "{\"ResponseData\":";
                output += JsonConvert.SerializeObject(data);
                output += "}";

                JObject resultjson = JObject.Parse(output);
                format.responseData = resultjson;
                format.result = true;
                return Json(format);
                //return Json(data);//PASS DATA TO AJAX
            }
            catch (Exception ex)
            {
                conn.Close();
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = ex.Message;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }

        }
        [System.Web.Http.Route("api/V1/ViewCurrentBill")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult ViewCurrentBill(int id)
        {

            required_api_response_format format = new required_api_response_format();
            var re = Request;
            var headers = re.Headers;
            string Comp_Name;
            string ID;

            if (headers.Contains("company_name") && headers.Contains("ID"))
            {
                Comp_Name = headers.GetValues("company_name").First();
                ID = headers.GetValues("ID").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! company_name OR ID Not Defined at Header ";
                format.responseData = null;
                format.result = false;
                return Json(format);
                //return Json(new { Msg = "Bad Request !!! company_name OR ID Not Defined at Header ", success = false });
            }

            //token
            string token;

            if (headers.Contains("token"))
            {
                token = headers.GetValues("token").First();
            }
            else
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Bad Request !!! token Not Defined at Header ";
                format.responseData = null;
                format.result = false;
                return Json(format);
             //   return Json(new { Msg = "Bad Request !!! token Not Defined at Header ", success = false });
            }

            Token_Master tm = new Token_Master();
            string x_msg = "";
            if (token == "" || token == null)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = "Please pass token";
                format.responseData = null;
                format.result = false;
                return Json(format);
               // return Json(new { Msg = "Please pass token", success = false });
            }
            if (tm.user_Token_InMst(token, ref x_msg) == false)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);

                //return Json(new { Msg = x_msg, success = false });
            }
            //token
            //--------------------Customer/Company and ID Validation------------------------------------
            Boolean x_result = false;
            var xdt = ListRebound(ID, Comp_Name, ref x_msg, ref x_result);
            if (x_result)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = x_msg;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
            //--------------------Customer/Company and ID Validation------------------------------------

            try
            {
                string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
                ServerDetails ser = new ServerDetails();
                //ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
                ser.setServerDetails_EF_dcs(ID, Comp_Name, ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

                string conn = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;


                DbProviderFactory f = DbProviderFactories.GetFactory("System.Data.SqlClient");
                DataTable dt = new DataTable();

                using (DbConnection connection = f.CreateConnection())
                {
                    connection.ConnectionString = conn;
                    //connection.ConnectionString = ConfigurationManager.ConnectionStrings["conn"].ToString();
                    //connection.ConnectionString = conn.ConnectionString;
                    connection.Open();

                    DbCommand command = f.CreateCommand();
                    // command.CommandText = "select * from FI_Table_Master";
                    //string str = "select * from fi_giisftr where ref_no ='" + id + "'";
                    //string str = "select *,b.Gross_Amount,b.Discount_Amount,b.Tax_Amount,b.Net_Amount from fi_giisftr a,fi_giishead b where  a.ref_no ='" + id + "' and b.REF_NO='" + id + "' ";
                    string str = "select *,b.Gross_Amount,b.Discount_Amount,b.Tax_Amount,b.Net_Amount,(select fname from FI_Caller_Master x where x.CallerIDNumber = b.CustomerID )  as [CustName],(select CallerID from FI_Caller_Master x where x.CallerIDNumber = b.CustomerID )  as [CustMobile] from fi_giisftr a,fi_giishead b where a.ref_no ='" + id + "' and b.REF_NO='" + id + "'";
                    command.CommandText = str;
                    command.Connection = connection;

                    IDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    connection.Close();

                }
                List<View_Bill> data = new List<View_Bill>();

                data = (from DataRow dr in dt.Rows
                        select new View_Bill()
                        {
                            Entered_Name = dr["Name_Display"].ToString(),
                            Name = dr["item_desc"].ToString(),
                            Qty = Convert.ToDouble(dr["QTY"].ToString()),
                            Rate = Convert.ToDouble(dr["Org_Rate"].ToString()),
                            Amount = Convert.ToDouble(dr["Org_Rate"].ToString()),
                            KOTNO = dr["KOTNO"].ToString(),
                            Punc_D_Time = dr["FTR_REF_DATE"].ToString(),
                            Code = dr["ITEM_CD"].ToString(),
                            userID = Convert.ToInt32(dr["userID"].ToString()),
                            UserName = dr["userName"].ToString(),
                            StationID = Convert.ToInt32(dr["StationID"].ToString()),
                            StationNAME = dr["StationNAME"].ToString(),
                            //BAR_CODE_NON_BAR_CODE = dr["BAR_CODE_NON_BAR_CODE"].ToString(),
                            //SERVICE_PROVIDER_NAME = dr["SERVICE_PROVIDER_NAME"].ToString(),
                            //SERVICE_PROVIDER_ID = Convert.ToInt32(dr["SERVICE_PROVIDER_ID"].ToString()),
                            //Service_Provider_Require = dr["Service_Provider_Require"].ToString(),
                            ItemID = Convert.ToInt32(dr["ItemID"].ToString()),
                            //StepNo = Convert.ToInt32(dr["StepNo"].ToString()),
                            ComboID = Convert.ToInt32(dr["ComboID"].ToString()),
                            //IMEI_Req = dr["IMEI_Req"].ToString(),
                            IMEI1_NO = dr["IMEI1"].ToString(),
                            IMEI2_NO = dr["IMEI2"].ToString(),
                            Item_Return_Remark = dr["Item_Return_Remark"].ToString(),
                            Unique_ID = dr["unique_ID"].ToString(),
                            Rate_WOT = Convert.ToDouble(dr["Rate_WOT"].ToString()),
                            TAX1_RAmt = Convert.ToDouble(dr["TAX1_RAmt"].ToString()),
                            TAX2_RAmt = Convert.ToDouble(dr["TAX2_RAmt"].ToString()),
                            //Rate_WTT = Convert.ToDouble(dr["Rate_WOT"].ToString()),
                            TAX_Price_Mode = dr["TAX_Price_Mode"].ToString(),
                            TAX1_Per = Convert.ToDouble(dr["TAX1"].ToString()),
                            TAX2_Per = Convert.ToDouble(dr["TAX2"].ToString()),
                            MC_Per = Convert.ToDouble(dr["MC_Per"].ToString()),
                            MC_AMT = Convert.ToDouble(dr["MC_AMT"].ToString()),
                            SC_Per = Convert.ToDouble(dr["SC_Per"].ToString()),
                            SC_AMT = Convert.ToDouble(dr["SC_AMT"].ToString()),
                            //-------------------------------------------------------------------------------------------------------------
                            Gross_Amount = Convert.ToDouble(dr["Gross_Amount"].ToString()),
                            Discount_Amount = Convert.ToDouble(dr["Discount_Amount"].ToString()),
                            Tax_Amount = Convert.ToDouble(dr["Tax_Amount"].ToString()),
                            Net_Amount = Convert.ToDouble(dr["Net_Amount"].ToString()),
                            //-------------------------------------------------------------------------------------------------------------
                            Table_Name = dr["D_C_P_REF_NO"].ToString(),
                            Customer_Name = dr["CustName"].ToString(),
                            Mobile = dr["CustMobile"].ToString()
                        }).ToList();

                //return Json(data);

                format = new required_api_response_format();
                format.responseCode = "200";
                format.responseMessage = "success !!!";
                
                //var output = JsonConvert.SerializeObject(data);
                string output = "{\"ResponseData\":";
                output += JsonConvert.SerializeObject(data);
                output += "}";
                JObject resultjson = JObject.Parse(output);
                format.responseData = resultjson;

                format.result = true;
                return Json(format);
            }
            catch (Exception ex)
            {
                format = new required_api_response_format();
                format.responseCode = "400";
                format.responseMessage = ex.Message;
                format.responseData = null;
                format.result = false;
                return Json(format);
            }
        }
    }
}