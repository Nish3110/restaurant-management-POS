using tmatsol_dump.Models.Web_Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tmatsol_dump;

using System.Data.SqlClient;
using System.Configuration;
using System.Data;

using ThinkHPTLM;
using tmatsol_dump.Models.Web_Report.Combos;
using tmatsol_dump.Models.Web_Report.DAL;
using System.Data.Common;
using tmatsol_dump.Models;
using System.Xml;
using System.IO;
using System.Threading.Tasks;

namespace tmatsol_dump.Controllers
{
    public class REPORTController : Controller
    {       
        UserDATA data = new UserDATA();
        public SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Report_DB_Entry"].ConnectionString);


        public SqlCommand cmd;
        public ActionResult SetLocal()
        {
            return View();
        }
        public ActionResult DeleteKey()
        {
            return View();
        }
        public ActionResult ClearLocal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ClearLocal(fi_usermast obj)
        {
           // Session["CUSTID"] = obj.GetCustID; Jay shah 22-5-24
            Response.Cookies["CUSTID"].Value = obj.GetCustID;

            if (obj.GetCustID != null)
            {
                if (obj.Authorize_Code == "3792")
                {

                    // return Response.Redirect("<script>function myCookie() { localStorage.removeItem('id');} </ script > ");

                    return RedirectToAction("DeleteKey");
                }
                ViewBag.M1 = "Please Insert Correct Password!!";
                return View();
                //string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
                ////List();
                //DataTable dt = new DataTable();
                //dt = List();
                //string conn = null;

                //foreach (DataRow row in dt.Rows)
                //{
                //    DBServer = row["DBServer"].ToString();
                //    DBName = row["DBName"].ToString();
                //    DBuser = row["DBuser"].ToString();
                //    DBtype = row["DBtype"].ToString();
                //    DBPassword = data.SQL_PWD;

                //    conn = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;
                //    dt.Dispose();
                //}

                //try
                //{

                //    SqlConnection con = new SqlConnection(conn);
                //    con.Open();
                //    SqlCommand cmd = new SqlCommand("select Count(*) from fi_usermast where Authorize_Code='" + obj.Authorize_Code + "' and Web_Report_Require='Yes';", con);

                //    int i = Convert.ToInt16(cmd.ExecuteScalar());
                //    if (i > 0)
                //    {
                //        Response.Write("<script>alert('Done, Value Clear Completed !!!');</script>");
                //        return RedirectToAction("DeleteKey");
                //    }
                //    else
                //    {
                //        ViewBag.M1 = "Please Insert Correct Password!!";
                //    }
                //    con.Close();
                //    return View();
                //}
                //catch (Exception e)
                //{
                //    ViewBag.Error = "Error : " + e.Message.ToString();
                //    return View();
                //}
            }
            else
            {
                return View("Index");
            }
        }
        public ActionResult Index(string ID)
        {
            /*if (ID != null)
            {
                string OLDUrl = Request.Url.AbsoluteUri;
                OLDUrl = OLDUrl.Replace(ID, "");
                //Request.Url.AbsoluteUri.Replace("Employee/List","Report/Login")

                OLDUrl = OLDUrl.ToUpper();
                string NEWUrl = OLDUrl.Replace("REPORT/INDEX/", "Report/Login");

                //Session["CUSTID"] = ID; Jay Shah 22 May 2024
                ViewData["ID"] = ID;
                ViewData["URL"] = NEWUrl;
                return View("DirectSetLocal");
            }
            else
            {
                Session["CUSTID"] = null;
                //return View("SetLocal");
                return View();
            } */ // Jay Shah 22-5-24
            if (ID != null)
            {
                string OLDUrl = Request.Url.AbsoluteUri;
                OLDUrl = OLDUrl.Replace(ID, "");
                OLDUrl = OLDUrl.ToUpper();
                string NEWUrl = OLDUrl.Replace("REPORT/INDEX/", "Report/Login");

                // Set a cookie with the ID value
                Response.Cookies["CUSTID"].Value = ID;

                ViewData["ID"] = ID;
                ViewData["URL"] = NEWUrl;
                return View("DirectSetLocal");
            }
            else
            {
                // Remove the cookie for CUSTID
                Response.Cookies["CUSTID"].Expires = DateTime.Now.AddDays(-1);

                return View();
            }


            // String id = null;
            // SqlDataReader sdr;
            //// con.ConnectionString = coidn.ConnectionString.Replace("XpasswordX", "kajal3792kajal");

            // // cmd = new SqlCommand("select * from Cust_Rec where Cust_ID='" + ID + "' and To_dt >='" + DateTime.Now + "' ", con);
            // cmd = new SqlCommand("select * from Cust_Rec where Cust_ID='" + ID + "' ", con);
            // con.Open();
            // sdr = cmd.ExecuteReader();
            // if (sdr.Read())
            // {
            //     id = sdr["Cust_ID"].ToString();
            //     TempData["CUSTID"] = ID;
            // }
            // else
            // {
            //     sdr.Close();
            // }
            // sdr.Close();
            // con.Close();


            // if (ID == id && ID != null)
            // {
            //     Session["CUSTID"] = TempData["CUSTID"];
            //     return RedirectToAction("Login");
            //    // Session["CUSTID"] = TempData["CUSTID"];
            //    // return RedirectToAction("DirectSetLocal");
            // }
            // else
            // {
            //     return View("Index");
            // }
        }
        public ActionResult DirectSetLocal()
        {
            return View("DirectSetLocal");
        }
        public ActionResult Login()
        {
            DataTable dtt = new DataTable();
            ViewBag.AList = ToSelectList(dtt, "", "");

            return View();
        }

        [HttpPost]
        public ActionResult Login(fi_usermast obj)
        {
            /*if (Session["CUSTID"] == null)
            {
                Session["CUSTID"] = obj.GetCustID;
            }
            else
            {
                if (obj.GetCustID != null && Session["CUSTID"] == null)
                {
                    Session["CUSTID"] = obj.GetCustID;
                }
            }*/ // jay shah 22-5-24
            if (Request.Cookies["CUSTID"] == null)
            {
                // Set a cookie with the value of obj.GetCustID
                Response.Cookies["CUSTID"].Value = obj.GetCustID.ToString();
            }
            else
            {
                // Read the CUSTID value from the cookie
                var custIdCookie = Request.Cookies["CUSTID"];
                var custIdValue = custIdCookie.Value;

                if (obj.GetCustID != null && string.IsNullOrEmpty(custIdValue))
                {
                    // Set a cookie with the value of obj.GetCustID
                    Response.Cookies["CUSTID"].Value = obj.GetCustID.ToString();
                }
            }


            if (Request.Cookies["CUSTID"] != null ) // changed from Session["CUSTID"] != null Jay shah 22-5-24
            {

                string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
                string Login_ID = "";
                DataTable dt = new DataTable();
                dt = ListRebound(obj.Comp_Name);
                string conn = null;

                foreach (DataRow row in dt.Rows)
                {
                    Login_ID = row["Login_ID"].ToString();
                    /*Session["From_dt"] = ((DateTime)row["From_dt"]).ToString("dd-MMM-yy");
                    Session["To_dt"] = ((DateTime)row["To_dt"]).ToString("dd-MMM-yy");
                    Session["To_dtVal"] = ((DateTime)row["To_dt"]).ToString();*/ // Jay shah 22-5-24
                    Response.Cookies["From_dt"].Value = ((DateTime)row["From_dt"]).ToString("dd-MMM-yy");
                    Response.Cookies["To_dt"].Value = ((DateTime)row["To_dt"]).ToString("dd-MMM-yy");
                    Response.Cookies["To_dtVal"].Value = ((DateTime)row["To_dt"]).ToString();

                    DateTime today = DateTime.Now;
                    DateTime Todate = (DateTime)row["To_dt"];
                    string DaysRemaining = "Left Day/s" + Math.Round((Todate - today).TotalDays, 0).ToString();
                    // Session["DaysRemaining"] = DaysRemaining; Jay shah 22-5-24
                    Response.Cookies["DaysRemaining"].Value = DaysRemaining.ToString();
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

                    DBPassword = data.SQL_PWD;
                    if (row["pwd_policy"].ToString() == "POLICY001")
                    {
                        DBPassword = "Kajal@123";
                    }
                    if (row["pwd_policy"].ToString() == "POLICY002")
                    {
                        DBPassword = "Trushachampak@123";
                    }



                   // Session["Comp_Name"] = row["Comp_Name"].ToString(); Jay Shah 22-5-24
                    Response.Cookies["Comp_Name"].Value = row["Comp_Name"].ToString();


                    conn = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;
                    dt.Dispose();
                }
                try
                {


                    DataTable dtt = new DataTable();
                    ViewBag.AList = ToSelectList(dtt, "", "");
                    SqlConnection con = new SqlConnection(conn);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select Count(*) from fi_usermast where First_name='" + obj.First_name + "' and Authorize_Code='" + obj.Authorize_Code + "' and Web_Report_Require='Yes';", con);

                    int i = Convert.ToInt16(cmd.ExecuteScalar());
                    if (i > 0)
                    {
                        //Session["User"] = obj.First_name; Jay Shah 22-5-24
                        Response.Cookies["User"].Value = obj.First_name;


                        //DateTime Dbt = Convert.ToDateTime(Session["To_dtVal"]); Jay Shah 22-5-24
                        DateTime Dbt = Convert.ToDateTime(Request.Cookies["To_dtVal"].Value);


                        DateTime Today = DateTime.Now;
                        // '10no   7 '
                        //7   10
                        if (Dbt < Today)
                        {
                            ViewBag.m2 = "Your Report Utility Validity is Expired!!";
                            return View("login");
                            //reject
                        }
                        else
                        {
                            //------------------------Log Record---------------------------
                            SqlConnection connn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);

                            connn.Open();
                            //string strinst = "insert into Report_Login_History(UniqueID,InSessionLog,SessionLogId) values('" + Session["CUSTID"] + "','" + DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss") + "','" + Login_ID + "/" + obj.First_name + "')"; Jay shah 22-5-24
                            string strinst = "insert into Report_Login_History(UniqueID,InSessionLog,SessionLogId) values('" + Request.Cookies["CUSTID"].Value + "','" + DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss") + "','" + Login_ID + "/" + obj.First_name + "')";

                            SqlCommand cmdd = new SqlCommand(strinst, connn);
                            cmdd.ExecuteNonQuery();
                            cmdd.Dispose();
                            connn.Close();
                            //-------------------------End---------------------------------
                            return RedirectToAction("DashBoard");
                        }




                        //if (Convert.ToInt64(a) > Convert.ToInt64(b))
                        //{
                        //    ViewBag.m2 = "your report utility validity is expired!!";
                        //    return view("login");
                        //}
                        //return RedirectToAction("DashBoard");

                    }
                    else
                    {
                        ViewBag.M1 = "Please Insert Valid User Name And Password!!";
                        //Response.Write("<script>alert('Please Insert Valid User Name And Password!!');</script>");
                    }
                    con.Close();
                    return View();
                }
                catch (Exception e)
                {
                    ViewBag.Error = "DBServer :" + DBServer + " ,Error : " + e.Message.ToString();
                    return View();
                    //throw e;
                }
            }
            else
            {
                return View("Index");
            }


        }
        [NonAction]
        public SelectList ToSelectList(DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }
        public ActionResult Logout()
        {
            try
            {
                if (Request.Cookies["User"] == null) // changed from Session["User"] == null   Jay Shah 22-5-24
                {
                    TempData["a"] = "Please Close The Browser and Re-Open it";
                    // return RedirectToAction("www.tmatsol.com");
                    return RedirectToAction("http://www.tmatsol.com/Report/Login");
                }

                TempData["a"] = "You are successfully Logged out";
                //Session.RemoveAll();
                //Session.Abandon();
                /*Session["User"] = null;
                Session["Comp_Name"] = null;*/ // Jay Shah 22-5-24 
                Response.Cookies["User"].Value = null;
                Response.Cookies["User"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["Comp_Name"].Value = null;
                Response.Cookies["Comp_Name"].Expires = DateTime.Now.AddDays(-1);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                // return RedirectToAction("Index", "Home");
                //------------------------Log out record---------------------------
                SqlConnection connn = new SqlConnection(ConfigurationManager.ConnectionStrings["conn"].ConnectionString);

                connn.Open();
                //string strinst = "insert into Report_Login_History(UniqueID,InSessionLog) values('" + Session["CUSTID"] + "','" + DateTime.Now + "')";
                //string strinst = "update Report_Login_History set OutSessionLog ='" + DateTime.Now + "' where UniqueID='" + Session["CUSTID"] + "' "; Jay Shah 22-5-24
                string strinst = "update Report_Login_History set OutSessionLog ='" + DateTime.Now + "' where UniqueID='" + Request.Cookies["CUSTID"].Value + "' ";

                SqlCommand cmdd = new SqlCommand(strinst, connn);
                cmdd.ExecuteNonQuery();
                cmdd.Dispose();
                connn.Close();
                //-------------------------End---------------------------------
                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                /*Session.RemoveAll();
                Session.Abandon();*/ // Jay Shah 22-5-24
                                     // Clear all cookies
                foreach (string cookieName in Request.Cookies.AllKeys)
                {
                    Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
                }

                TempData["a"] = "You are successfully logged out" + e.Message;
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("Login");
            }
        }

        //public DataTable dt = new DataTable();
        public DataTable List()
        {
            try
            {
                // con.ConnectionString = con.ConnectionString.Replace("XpasswordX", "kajal3792kajal");

                DataTable dtt = new DataTable();
                con.Open();
                //string X_str = "select * from Cust_Rec where Cust_ID = '" + Session["CUSTID"] + "' and To_dt >= '" + DateTime.Now + "' "; Jay Shah 22-5-24
                string X_str = "select * from Cust_Rec where Cust_ID = '" + Request.Cookies["CUSTID"].Value + "' and To_dt >= '" + DateTime.Now + "' ";

                cmd = new SqlCommand(X_str, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dtt);
                sda.Dispose();
                con.Close();
                return dtt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataTable ListRebound(string dbname)
        {
            try
            {
                DataTable dtt = new DataTable();

                //con.ConnectionString = con.ConnectionString.Replace("XpasswordX", "kajal3792kajal");

                con.Open();
                //string X_str = "select * from Cust_Rec where Cust_ID = '" + Session["CUSTID"] + "' and Comp_Name='" + dbname.Trim() + "' "; Jay Shah 22-5-24
                string X_str = "select * from Cust_Rec where Cust_ID = '" + Request.Cookies["CUSTID"].Value + "' and Comp_Name='" + dbname.Trim() + "' ";

                cmd = new SqlCommand(X_str, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dtt);
                sda.Dispose();
                con.Close();
                return dtt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void ReAssing()
        {
            HttpCookie chk = Request.Cookies["User"];
            if (chk == null)
            {
                return;
            }
            else
            {
                string id = Request.Cookies["id"].Value;
                string From_dt = Request.Cookies["From_dt"].Value;
                string To_dt = Request.Cookies["To_dt"].Value;
                string To_dtVal = Request.Cookies["To_dtVal"].Value;
                string DaysRemaining = Request.Cookies["DaysRemaining"].Value;
                string Comp_Name = Request.Cookies["Comp_Name"].Value;
                string User = Request.Cookies["User"].Value;
                string TABLE_GRP_CD = Request.Cookies["TABLE_GRP_CD"].Value;
                string TableId = Request.Cookies["TableId"].Value;

                /*Session["CUSTID"] = id;
                Session["From_dt"] = From_dt;
                Session["To_dt"] = To_dt;
                Session["To_dtVal"] = To_dtVal;
                Session["DaysRemaining"] = DaysRemaining;
                Session["Comp_Name"] = Comp_Name;
                Session["User"] = User;
                Session["TABLE_GRP_CD"] = TABLE_GRP_CD;
                if (Session["TableId"] == null)
                {
                    Session["TableId"] = TableId;
                }
                else
                {
                    Session["TableId"] = Session["TableId"];
                }*/ // Jay Shah 22-5-24
                Response.Cookies["CUSTID"].Value = id;
                Response.Cookies["From_dt"].Value = From_dt.ToString();
                Response.Cookies["To_dt"].Value = To_dt.ToString();
                Response.Cookies["To_dtVal"].Value = To_dtVal.ToString();
                Response.Cookies["DaysRemaining"].Value = DaysRemaining.ToString();
                Response.Cookies["Comp_Name"].Value = Comp_Name;
                Response.Cookies["User"].Value = User;
                Response.Cookies["TABLE_GRP_CD"].Value = TABLE_GRP_CD;

                if (Request.Cookies["TableId"] == null)
                {
                    Response.Cookies["TableId"].Value = TableId;
                }
                else
                {
                    Response.Cookies["TableId"].Value = Request.Cookies["TableId"].Value;
                }

            }
        }

        public JsonResult PageRefresh()
        {
            try
            {
                HttpCookie cookie = new HttpCookie("tmatsol");
                cookie["Last_Refresh_Time"] = DateTime.Now.ToString();
                cookie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(cookie);

                return Json(new { R_fresh = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { R_fresh = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetMembers(string cname)//USE TO FILL DROP DOWN
        {
            SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["Report_DB_Entry"].ConnectionString);

            //con1.ConnectionString= con1.ConnectionString.Replace("XpasswordX", "kajal3792kajal");

            SqlCommand cmd1 = new SqlCommand();
            DataTable dt1 = new DataTable();
            con1.Open();
            cmd1.CommandText = "Select * from Cust_Rec where Cust_ID ='" + cname + "'";
            cmd1.Connection = con1;
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            sda1.Fill(dt1);
            ViewBag.AList = ToSelectList(dt1, "Comp_Name", "Comp_Name");
            con1.Close();
            return Json(ViewBag.AList, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX
                                                                     //  return (ToSelectList(ViewBag.AList, "Comp_Name", "Comp_Name"));//, JsonRequestBehavior.AllowGet);
        }
        //string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";     
        //---------------------Dashboard-----------------------------------------
        public ActionResult DashBoard()
        {
            try
            {
                ReAssing();
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }        
     
        [HttpGet]
        public JsonResult DashBoard_RELOAD(string Custid, string CustName)
        {
                string json;
                string Error_str = "";

                if (Custid.Trim() == "" && CustName.Trim() == "")
                {
                    /*Custid = System.Web.HttpContext.Current.Session["CUSTID"].ToString();
                    CustName = System.Web.HttpContext.Current.Session["Comp_Name"].ToString().Trim();*/ // Jay Shah 22-5-24
                Custid = Request.Cookies["CUSTID"].Value;
                CustName = Request.Cookies["Comp_Name"].Value.Trim();

            }
            DashBoardDataConfig XFetchData = new DashBoardDataConfig(Custid, CustName);
                json = XFetchData.FetchData(ref Error_str);

                //---------------------------------------------
                //SqlConnection con = new SqlConnection("Data Source=thinksoftwares.dyndns.org,2433;Initial Catalog=RedHandiKigali;User ID=RedHandiKigali;Password=Trushachampak@123;");

                //con.Open();
                //using (SqlCommand cmd = new SqlCommand("insert into Temp_test(Temp_test) values('" + CustName + json + DateTime.Now + "') ", con))
                //{
                //    cmd.CommandType = CommandType.Text;
                //    int result = cmd.ExecuteNonQuery();
                //    con.Close();
                //}
                //con.Close();
                //---------------------------------------------            
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult JSON(short numofDays)
        {        
            if (Request.Cookies["User"] == null) // changed from Session["User"] == null Jay Shah 22-5-24
            {
                TempData["a"] = "Please Login";
                return RedirectToAction("Login", "Report");
            }

            string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";

            var ds = new DataSet();
            ServerDetails ser = new ServerDetails();
            ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

            var x_error = "";
            ds = new ThinkHPTLM.Reports(true, null, null, null, null, null, null, null).WeekDayTotalOrders(true, DBuser, DBPassword, DBtype, DBName, DBServer, DBuser, DBPassword, ref numofDays, ref x_error); ;
            
            List<WeekDay> result;

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
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //--------------------Tables Report--------------------------------------

        public ActionResult Table()
        {
            ReAssing();
            if (Request.Cookies["User"] == null) // changed from Session["User"] == null Jay Shah 22-5-24
            {
                TempData["a"] = "Please Login";
                return RedirectToAction("Login", "Report");
            }
            // Session["TABLE_GRP_CD"] = "1"; Jay Shah 22-5-24
            Response.Cookies["TABLE_GRP_CD"].Value = "1";

            return View();
        }
        [HttpPost]
        public ActionResult Table(FI_Table_Master obj)
        {
            try
            {
                //Session["TableId"] = obj.Table_no; Jay Shah 22-5-24
                Response.Cookies["TableId"].Value = obj.Table_no;

                //Session["Old_ref_no"] = "New Bill";

                return RedirectToAction("AvailableBills");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public JsonResult GetTables()//USE TO FILL Table List
        {
            string conn = "";
            try
            {
                string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
                ServerDetails ser = new ServerDetails();
                //ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
                ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

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
                    //string str = "SELECT Table_no,Nos_Of_Persons,FI_Table_Template_Master_sr_no ,isnull((select top 1  D_C_P_REF_NO from FI_giishead where D_C_P_REF_NO = a.Table_no and OrderType = 1),'NA') as [Table_Occupuied],IOT,(select MIN(Print_Seq_Number_Bill) from FI_giishead where D_C_P_REF_NO=a.Table_no ) as [Bill_Printed],(select pos_x from FI_Table_Template_Master where sr_no =a.FI_Table_Template_Master_sr_no) as [X],(select pos_y from FI_Table_Template_Master where sr_no = a.FI_Table_Template_Master_sr_no) as [Y] FROM FI_Table_Master A, FI_Table_Template_Master B WHERE A.FI_Table_Template_Master_sr_no = B.SR_NO  AND TABLE_GRP_CD = '" + Session["TABLE_GRP_CD"].ToString() + "' order by FI_Table_Template_Master_sr_no"; JAy shah 22-5-24
                    string str = "SELECT Table_no,Nos_Of_Persons,FI_Table_Template_Master_sr_no ,isnull((select top 1  D_C_P_REF_NO from FI_giishead where D_C_P_REF_NO = a.Table_no and OrderType = 1),'NA') as [Table_Occupuied],IOT,(select MIN(Print_Seq_Number_Bill) from FI_giishead where D_C_P_REF_NO=a.Table_no ) as [Bill_Printed],(select pos_x from FI_Table_Template_Master where sr_no =a.FI_Table_Template_Master_sr_no) as [X],(select pos_y from FI_Table_Template_Master where sr_no = a.FI_Table_Template_Master_sr_no) as [Y] FROM FI_Table_Master A, FI_Table_Template_Master B WHERE A.FI_Table_Template_Master_sr_no = B.SR_NO  AND TABLE_GRP_CD = '" + Request.Cookies["TABLE_GRP_CD"].Value + "' order by FI_Table_Template_Master_sr_no";

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



                return Json(data, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public JsonResult GetDineINmode()
        {

            string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
            ServerDetails ser = new ServerDetails();
            ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

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
                //if (data.Count == 1)
                //{
                //    conn.Close();
                //    return Json(new { redirectUrl = Url.Action("Table", "RTC"), isRedirect = true }, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX
                //}

                conn.Close();
                return Json(data, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }
        public JsonResult GetmodebaseTable(int Mid)
        {
            string conn = "";
            try
            {
                string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
                ServerDetails ser = new ServerDetails();
                ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

                conn = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;

                DbProviderFactory f = DbProviderFactories.GetFactory("System.Data.SqlClient");
                DataTable dt = new DataTable();

                using (DbConnection connection = f.CreateConnection())
                {
                    connection.ConnectionString = conn;
                    //connection.ConnectionString = ConfigurationManager.ConnectionStrings["conn"].ToString();
                    //Main(null);
                    //connection.ConnectionString = conn.ConnectionString;

                    connection.Open();

                    DbCommand command = f.CreateCommand();
                    // command.CommandText = "select * from FI_Table_Master";
                    //string str = "SELECT Table_no,Nos_Of_Persons,FI_Table_Template_Master_sr_no ,isnull((select top 1  D_C_P_REF_NO from FI_giishead where D_C_P_REF_NO = a.Table_no and OrderType = 1),'NA') as [Table_Occupuied],IOT FROM FI_Table_Master A, FI_Table_Template_Master B WHERE A.FI_Table_Template_Master_sr_no = B.SR_NO  AND TABLE_GRP_CD = '" + Mid + "'";
                    string str = "SELECT Table_no,Nos_Of_Persons,FI_Table_Template_Master_sr_no ,isnull((select top 1  D_C_P_REF_NO from FI_giishead where D_C_P_REF_NO = a.Table_no and OrderType = 1),'NA') as [Table_Occupuied],IOT,(select MIN(Print_Seq_Number_Bill) from FI_giishead where D_C_P_REF_NO=a.Table_no ) as [Bill_Printed],(select pos_x from FI_Table_Template_Master where sr_no =a.FI_Table_Template_Master_sr_no) as [X],(select pos_y from FI_Table_Template_Master where sr_no = a.FI_Table_Template_Master_sr_no) as [Y] FROM FI_Table_Master A, FI_Table_Template_Master B WHERE A.FI_Table_Template_Master_sr_no = B.SR_NO  AND TABLE_GRP_CD = '" + Mid + "' order by FI_Table_Template_Master_sr_no";
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
                            Bill_Printed = dr["Bill_Printed"].ToString(),
                            //Bill_Printed=Convert.ToInt32(dr["Bill_Printed"]),
                            IOT = Convert.ToInt32(dr["IOT"])

                            // color = IsTableActive(dr["Table_no"].ToString())

                        }).ToList();



                return Json(data, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult AvailableBills()
        {
            ReAssing();
            if (Request.Cookies["User"] == null) // changed from  Session["User"] == null Jay Shah 22-5-24
            {
                TempData["a"] = "Please Login";
                return RedirectToAction("Login", "Report");
            }
            return View();
        }
        public JsonResult GetOldBillList()
        {
            string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
            ServerDetails ser = new ServerDetails();
            ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

            string con = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;

            SqlConnection conn = new SqlConnection(con);

            try
            {
                // var data = _db.fI_Caller_Masters.Where(x => x.Caller_Type == 0).ToList();
                //SqlConnection conn = new SqlConnection(con);
                DataTable dt = new DataTable();
                String str = "select ref_no,OrderType,(select MODE_NAME from MODE_MASTER where MODE_CODE=a.OrderType) as [ModeTypeStr],Print_Seq_Number_Bill,Bill_Note  from FI_giishead a  where OrderType =1 and D_C_P_REF_NO = N'" + Session["TableId"] + "'";

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

                if (data.Count == 0)
                {
                    return Json(new { redirectUrl = Url.Action("Table", "Report"), isRedirect = true }, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX
                }

                conn.Close();
                return Json(data, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }

        }
        [HttpPost]
        public JsonResult ViewCurrentBill(int id)
        {
            try
            {
                string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
                ServerDetails ser = new ServerDetails();
                ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

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
                return Json(data, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return null;
        }

        //--------------------Tables Report--------------------------------------

        //--------------------Report Controll------------------------------------
        public ActionResult Sales_Sum()
        {
                ReAssing();
                if (Request.Cookies["User"] == null)// changed from Session["User"] == null Jay Shah 22-5-24
            {
                    TempData["a"] = "Please Login";
                    return RedirectToAction("Login", "Report");
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();

                List<SelectListItem> Filter = new List<SelectListItem>();

                ViewData["ReportType"] = Combos.FI_Web_Report_Details().ToList();

                ViewData["Filter"] = Combos.Cashier_Settle_Date().ToList();

                return View();                       
        }

        [HttpPost]
        [HandleError]
        public ActionResult Sale_summary(FormCollection frm)
        {
            string XError = "test";
            try
            {
                ReAssing();
                if (Request.Cookies["User"] == null)// changed from Session["User"] == null Jay shah 22-5-24
                {
                    TempData["a"] = "Please Login";
                    return RedirectToAction("Login", "Report");
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();

                var From_Time = frm["DcsFrTime"];
                var To_Time = frm["DcsToTime"];

                var From_date = (Convert.ToDateTime(frm["DcsFrDt"])).ToString("dd-MMM-yyyy");
                var To_date = (Convert.ToDateTime(frm["DcsToDt"])).ToString("dd-MMM-yyyy");//Convert.ToDateTime(frm["To"].ToString("dd-MMM-yyyy")); 
                //To_date = To_date + " 23:59";
                From_date = From_date + " " + From_Time;
                To_date = To_date + " " + To_Time;

                var filter = frm["c"].ToString();
                //var filter = "Cashier Date";


                string XSName = "";
                string XDbNane = "";
                string Xstr = "";

                string XSQL_USER_NAME = "";
                string XSQL_PWD = "";
                string xCaption = "";

                DataSet Ds = new DataSet();

                //Xstr = "select DataBase_Name,Server_Name,SQL_USER_NAME,SQL_PWD,Captions from FI_Web_Report_Database where Rec_no=" + frm["FI_Web_Report_Database"].ToString();

                string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
                ServerDetails ser = new ServerDetails();
                ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
                DAL Xdal = new DAL();
                XDbNane = DBName;
                XSName = DBServer;

                // xCaption = Session["Comp_Name"].ToString(); Jay shah 22-5-24
                xCaption = Request.Cookies["Comp_Name"].Value;

                XSQL_USER_NAME = DBuser;
                XSQL_PWD = DBPassword;
               
                //Dt = Report_datasets("Bill Statement", From_date, To_date,"",ref XError);
                //XError = XError + "Invoking REport Dataset from DLL";
                XError = "";
                //Ds = Report_datasets(frm["ReportType"].ToString(), From_date, To_date, filter, XSName, XDbNane, XSQL_USER_NAME, XSQL_PWD, ref XError);
                Ds = Report_datasets(frm["ReportType"].ToString(), From_date, To_date, filter, XSName, XDbNane, XSQL_USER_NAME, XSQL_PWD, ref XError);

                //XError = XError + "Invoking Done Dataset from DLL" + "Errror from Ds" + XError + Ds.Tables[0].Rows.Count;
    
                /*Session["toDate"] = To_date;
                Session["fromDate"] = From_date;*/ // Jay Shah 22-5-24
                Response.Cookies["toDate"].Value = To_date.ToString();
                Response.Cookies["fromDate"].Value = From_date.ToString();

                ViewBag.Company = xCaption;
                ViewBag.Error = XError;
                ViewBag.ReportType = "Company :"+ xCaption + " Report : " + frm["ReportType"].ToString() + " From : " + From_date.ToString() + " To: " + To_date.ToString();
                // return PartialView(Ds.Tables[0]);
           
                return PartialView("Sale_summary", Ds.Tables[0]);                
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message + "Data Error:  " + XError;          
                return View("Error");
                //return Content("Sale_summary");
            }
        }

        public DataSet Report_datasets(string XReportName, string From_date, string To_date, string filter, string XServerName, string XDbName, string XSQL_USER, string XSQL_PWD, ref string X_Error)                    
        {
            string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "", result_message = "";
            ServerDetails ser = new ServerDetails();

            ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
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
            catch(Exception ex)
            {
                X_Error = ex.Message.ToString() + " while at " + XReportName  + DBuser;
            }
            return xRetundDS;
        }
        //--------------------Report Controll------------------------------------

        private void Createtable_Cust_Rec()
        {
            //sample insert query

            //            drop table[Cust_Rec]

            //CREATE TABLE[dbo].[Cust_Rec]
            //        (

            //   [Cust_ID][nvarchar](50) not NULL primary key,

            //  [From_dt] [datetime]
            //        not NULL,

            //  [To_dt] [datetime]
            //        not NULL,

            //  [DBServer] [nvarchar] (50) not NULL,

            //   [DBName] [nvarchar] (50) not NULL,

            //    [DBuser] [nvarchar] (50) not NULL,

            //     [DBtype] [nvarchar] (50) not NULL,

            //      [Comp_Name] [nchar] (100) not NULL
            //) ON[PRIMARY]

            //INSERT INTO[Report_DB_Entry].[dbo].[Cust_Rec]
            //        ([Cust_ID]
            //,[From_dt]
            //,[To_dt]
            //,[DBServer]
            //,[DBName]
            //,[DBuser]
            //,[DBtype]
            //,[Comp_Name])
            //VALUES
            //( '6b8778b5754c4c2a76547b11c3134af4',
            //           '2020-01-01',
            //            '2050-01-01',
            //           'satrangfoods.dyndns.org',
            //            'TMATS00012018',
            //            'tmats',
            //           'SQL',
            //           'pind da dhaba')

        }
    }
}