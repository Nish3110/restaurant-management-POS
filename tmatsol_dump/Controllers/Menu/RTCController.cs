using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web.Mvc;
using Table_Cart_V2k20.Models;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.IO;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Diagnostics;
using Newtonsoft.Json;
using tmatsol_dump.ViewModel;
using System.Web.Hosting;

namespace Table_Cart_V2k20.Controllers
{
    public class RTCController : Controller
    {
        //--------------------------LOGIN--------------------------------//
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
        [HttpPost]
        //  public JsonResult GetMembers(string cname)//USE TO FILL DROP DOWN
        public JsonResult GetMembers()
        {
            try
            {
                SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["Report_DB_Entry"].ToString());
                SqlCommand cmd1 = new SqlCommand();
                DataTable dt1 = new DataTable();
                con1.Open();
                cmd1.CommandText = "select Comp_Name from cust_rec";
                cmd1.Connection = con1;
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                sda1.Fill(dt1);
                if (dt1.Rows.Count == 0)
                {
                    con1.Close();
                    return Json(new { msg = "Please Define Company Name in web_billing_entry!!!" }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.AList = ToSelectList(dt1, "Comp_Name", "Comp_Name");
                con1.Close();
                return Json(ViewBag.AList, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX
                //return (ToSelectList(ViewBag.AList, "Comp_Name", "Comp_Name"));//, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Index()
        {
            DataTable dtt = new DataTable();
            ViewBag.AList = ToSelectList(dtt, "", "");

            return View();
        }
        [HttpPost]
        public ActionResult Index(UserIndex obj)
        {
            MyConnection.CompName = obj.Comp_Name;
            //return RedirectToAction("Mode", "RTC");
            return RedirectToAction("Menu", "RTC", new { id = obj.Comp_Name.Trim() });
        }
        //--------------------------LOGIN--------------------------------//

        public ActionResult Stest()
        {
            //Task.Stest            
            ViewBag.MSG = Task.Stest;
            return View();
        }
        [HttpPost]
        public ActionResult Stest(FormCollection frm)
        {
            Task.Stest = frm["txtVal"];
            ViewBag.MSG = Task.Stest;
            return View();
        }
        public ActionResult ErrorXmsg()
        {
            return View();
        }

        public JsonResult GetBillList(int id)
        {
            try
            {
                if (id == 1)
                {
                    var data = MyConnection._db.fI_GIISHEADs.Where(x => x.OrderType == 1).Select(i => new { i.REF_NO, i.REF_DATE }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                if (id == 2)
                {
                    var data = MyConnection._db.fI_GIISHEADs.Where(x => x.OrderType == 2).Select(i => new { i.REF_NO, i.REF_DATE }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                if (id == 3)
                {
                    var data = MyConnection._db.fI_GIISHEADs.Where(x => x.OrderType == 3).Select(i => new { i.REF_NO, i.REF_DATE }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                if (id == 4)
                {
                    var data = MyConnection._db.fI_GIISHEADs.Where(x => x.OrderType == 4).Select(i => new { i.REF_NO, i.REF_DATE }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                return Json(null);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult TEST()
        {
            return View();
        }
        public JsonResult logout()
        {
            try
            {
                //string logout_timestamp =(string)Format(DateTime.Now,"dd-MMM-yyyy HH:mm:ss");

                string logout_timestamp = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");


                //SqlConnection conn = new SqlConnection(con);
                MyConnection.conn.Open();
                //string str = "Update [DeviceLog_Rec] set [OutSessionLog] ='" + logout_timestamp + "' where [SessionLogId] ='" + Session["SessionLogId"] + "'"; Jay Shah 22-5-24
                string str = "UPDATE [DeviceLog_Rec] SET [OutSessionLog] = '" + logout_timestamp + "' WHERE [SessionLogId] = '" + Request.Cookies["SessionLogId"]?.Value + "'";

                SqlCommand cmd = new SqlCommand(str, MyConnection.conn);
                cmd.ExecuteNonQuery();
                MyConnection.conn.Close();

                //Session.Clear();
                //Session.Abandon(); Jay Shah 22-5-24

                foreach (string cookieName in Request.Cookies.AllKeys)
                {
                    Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
                }


                return Json(null);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult Error()
        {
            return View();
        }

        public JsonResult changecaller(string cname)
        {
            try
            {
                if (cname == "CORPORATE")
                {
                    return Json(new { redirectUrl = Url.Action("CorporateIndex", "RTC"), isRedirect = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { redirectUrl = Url.Action("CustomerIndex", "RTC"), isRedirect = true }, JsonRequestBehavior.AllowGet);
                }
                // return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public ActionResult ReturnMenu()
        {

            //if (Session["FingerPrint"] == null)
            //{
            //    return RedirectToAction("DeviceInfo", "Login");
            //}
            //-----Dishplay First menu group name---------------------------
            //var FirstMenu = MyConnection._db.fI_GRP_MASTERs.Where(x => x.GRP_CD != "00001").OrderBy(x => x.Order_By).ThenBy(x => x.GRP_NAME).Select(x => x.GRP_NAME).FirstOrDefault();

            // var FirstMenu = MyConnection._db.fI_GRP_MASTERs.Where(x => x.FOR_GRP_CD == "00001").OrderBy(x => x.Order_By).ThenBy(x => x.GRP_NAME).Select(x => x.GRP_NAME).FirstOrDefault();
            var FirstMenu = MyConnection._db.fI_GRP_MASTERs.Where(x => x.FOR_GRP_CD == "00001" && x.GRP_CD != "00001").OrderBy(x => x.Order_By).ThenBy(x => x.GRP_NAME).Select(x => x.GRP_NAME).FirstOrDefault();

            //Session["FirstMenu"] = FirstMenu;
            //Session["FirstMenu"] = "All";
            //-----Dishplay First menu group name---------------------------
            return View();
        }


        public ActionResult Menu(string id, string table)
        {
            //29-06-2021
            Session["SelectedCompany"] = MyConnection.CompName;
            Session["FingerPrint"] = "FP:" + DateTime.Now.ToString();
            Session["UserType"] = "Guest:" + DateTime.Now.ToString();
            Session["User_ID"] = "0";

            if (table == null || table.Trim() == "")
            {
                Session["Mode"] = "DELIVERY";
            }
            else
            {
                Session["Mode"] = "DINE IN";
                Session["TableId"] = table;
            }
            Session["PAX"] = "0";
            //-----Dishplay First menu group name---------------------------
            MyConnection.Main(null);
            var dataz1 = MyConnection.DBuser;
            var dataz2 = MyConnection.DBName;
            var dataz3 = MyConnection._db;


            MyConnection.DBServer = "thinksoftwares.dyndns.org";
            var FirstMenu = MyConnection._db.fI_GRP_MASTERs.Where(x => x.FOR_GRP_CD == "00001" && x.GRP_CD != "00001")
            .OrderBy(x => x.Order_By)
            .ThenBy(x => x.GRP_NAME)
            .Select(x => x.GRP_NAME)
            .FirstOrDefault();

            //-----Dishplay First menu group name---------------------------
            return View();
        }


        [HttpPost]
        public JsonResult AddOpenqty(string qty, string id, string lang)
        {
            try
            {
                if (qty != "" & id != "")
                {
                    //var aaa = MyConnection._db.FI_Itmmasts.Where(p => p.ITEM_CD == id).Select(x => new { x.SALE_PRICE, x.ITEM_DESC, x.ITEM_DESC_SL, x.ItemID }).FirstOrDefault();
                    var aaa = MyConnection._db.FI_Itmmasts.Where(p => p.ITEM_CD == id).Select(x => new { x.SALE_PRICE, x.Dine_IN_Price, x.Counter_Price, x.Corporate_Price, x.Delivery_Price, x.ITEM_DESC, x.ITEM_DESC_SL, x.ItemID }).FirstOrDefault();

                    Product product = new Product();

                    if (GetCart().lineCollection.Count < 1)
                    {
                        product.Sr = 1;
                        //Session["Srno"] = product.Sr;
                    }
                    else
                    {
                        //int srno = Convert.ToInt32(Session["Srno"]);
                        int srno = GetCart().lineCollection.Count;//30aug2023
                        product.Sr = srno + 1;
                        //Session["Srno"] = product.Sr;
                    }
                    product.ProductID = id;
                    product.Quantity = Convert.ToDecimal(qty);
                    //product.Price = Convert.ToDecimal(aaa.SALE_PRICE);
                    product.Price = Convert.ToDecimal(price(aaa.SALE_PRICE, aaa.Dine_IN_Price, aaa.Counter_Price, aaa.Corporate_Price, aaa.Delivery_Price));

                    if (aaa.ITEM_DESC_SL.Trim() != "")
                    {
                        if (lang == "Arabic")
                        {
                            product.Item_Description_SL = aaa.ITEM_DESC_SL;
                            product.Item_Description = aaa.ITEM_DESC;
                        }
                        else
                        {
                            product.Item_Description = aaa.ITEM_DESC;
                        }
                    }
                    else
                    {
                        product.Item_Description = aaa.ITEM_DESC;
                    }
                    Combo_And_Modifier cam = new Combo_And_Modifier(MyConnection.conn);
                    //isModifier
                    //bool isModifier = cam.IsModifier(id);
                    bool isModifier = cam.IsModifier((aaa.ItemID).ToString());
                    cam.Dispose();
                    if (isModifier)
                    {
                        //product.ComboID = id;
                        product.ComboID = (aaa.ItemID).ToString();
                    }
                    else
                    {
                        product.ComboID = "0";
                    }

                    GetCart().AddItem(product, Convert.ToInt32(qty));
                    //var count = Session["Cart"]; Jay Shah 22-5-24
                    var count = Request.Cookies["Cart"]?.Value;


                    //Combo_And_Modifier cam = new Combo_And_Modifier();
                    //isModifier
                    if (isModifier)
                    {
                        return Json(new { redirectUrl = Url.Action("Modifier", "RTC"), mid = id, isRedirect = true, }, JsonRequestBehavior.AllowGet);
                    }
                    //isCombo

                    return Json(count, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult AddOpenitem(string itemname, string rate)
        {
            try
            {
                if (itemname != "" & rate != "")
                {

                    //var aaa = MyConnection._db.FI_Itmmasts.FirstOrDefault(p => p.ITEM_CD == id);

                    Product product = new Product();

                    if (GetCart().lineCollection.Count < 1)
                    {
                        product.Sr = 1;
                        //Session["Srno"] = product.Sr;
                    }
                    else
                    {
                        //int srno = Convert.ToInt32(Session["Srno"]);
                        int srno = GetCart().lineCollection.Count;//30aug2023
                        product.Sr = srno + 1;
                        //Session["Srno"] = product.Sr;
                    }

                    //product.ProductID = Session["openitemid"].ToString(); jay Shah 22-5-24
                    product.ProductID = Request.Cookies["openitemid"]?.Value;

                    product.Quantity = 1;
                    product.Price = Convert.ToDecimal(rate);
                    product.Item_Description = itemname;
                    product.ComboID = "0";
                    GetCart().AddItem(product, 1);
                    // var count = Session["Cart"]; Jay Shah 22-5-24
                    var count = Request.Cookies["Cart"]?.Value;

                    return Json(count, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult AdditemNote(string index, string itemcd, string itemname, string rate)
        {
            try
            {
                if (itemname != "" & rate != "")
                {
                    Product product = new Product();

                    product.Sr = Convert.ToInt32(index) + 1;
                    product.ProductID = itemcd;
                    product.Quantity = 1;
                    product.Price = Convert.ToDecimal(rate);
                    product.Item_Description = itemname;
                    product.ComboID = "0";

                    GetCart().AddItemNote(product, 1, index);
                    //var count = Session["Cart"]; Jay Shah 22-5-24
                    var count = Request.Cookies["Cart"]?.Value;

                    return Json(count, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult hideCounter()
        {
            try
            {
                Boolean xxIsDemo;
                Boolean xxIsLimitOver = false;
                xxIsDemo = Task.IsDeviceReg(Request.Cookies["FingerPrint"].Value, ref xxIsLimitOver);
                if (xxIsDemo == false)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public JsonResult hide_DirectModule()
        {
            try
            {
                if (ConfigurationManager.AppSettings["Allow_BillandReceipt"].ToString() != "Yes")
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static string RemoveSpecialCharacters(string input)
        {
            //Regex r = new Regex("(?:[^a-z0-9 ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            Regex r = new Regex("-");

            return r.Replace(input, String.Empty);
        }


        [HttpGet]
        public JsonResult GetMstMenu()
        {
            try
            {
                //var menu = MyConnection._db.fI_GRP_MASTERs.Where(x => x.FOR_GRP_CD == "00001" && x.GRP_CD != "00001").OrderBy(x => x.GRP_NAME).ToList();
                // var menu = MyConnection._db.fI_GRP_MASTERs.Where(x => x.GRP_CD != "00001").OrderBy(x => x.Order_By).ThenBy(x => x.GRP_NAME).ToList();                

                // var menu = MyConnection._db.fI_GRP_MASTERs.Where(x => x.FOR_GRP_CD == "00001").OrderBy(x => x.Order_By).ThenBy(x => x.GRP_NAME).ToList();
                //  var menu = MyConnection._db.fI_GRP_MASTERs.Where(x => x.FOR_GRP_CD == "00001" && x.GRP_CD != "00001").OrderBy(x => x.Order_By).ThenBy(x => x.GRP_NAME).ToList();
                var menu = MyConnection._db.fI_GRP_MASTERs.Where(x => x.FOR_GRP_CD == "00001" && x.GRP_CD != "00001" && x.Status == "Active").OrderBy(x => x.Order_By).ThenBy(x => x.GRP_NAME).ToList();
                return Json(menu, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult ViewCurrentBill(int id)
        {
            try
            {

                DbProviderFactory f = DbProviderFactories.GetFactory("System.Data.SqlClient");
                DataTable dt = new DataTable();

                using (DbConnection connection = f.CreateConnection())
                {

                    //connection.ConnectionString = ConfigurationManager.ConnectionStrings["conn"].ToString();
                    MyConnection.Main(null);
                    connection.ConnectionString = MyConnection.conn.ConnectionString;
                    connection.Open();

                    DbCommand command = f.CreateCommand();
                    // command.CommandText = "select * from FI_Table_Master";
                    //string str = "select * from fi_giisftr where ref_no ='" + id + "'";
                    string str = "select *,b.Gross_Amount,b.Discount_Amount,b.Tax_Amount,b.Net_Amount from fi_giisftr a,fi_giishead b where  a.ref_no ='" + id + "' and b.REF_NO='" + id + "' ";
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
                            Net_Amount = Convert.ToDouble(dr["Net_Amount"].ToString())
                        }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX
            }
            catch (Exception ex)
            {
                MyConnection.conn.Close();
                throw ex;
            }
            //return null;
        }
        public JsonResult SetBillprt(int id, FI_Web_BILL_Print_request obj)
        {
            try
            {
                string fp = Request.Cookies["FingerPrint"].Value;
                var printer = MyConnection._db.Device_Reg_Msts.Where(x => x.UniqueID == fp).Select(x => x.Bill_Printer_Name).FirstOrDefault();

                if (printer == null || printer == "" || printer == "NA")
                {
                    return Json(new { msg = "Printer Not Defined." }, JsonRequestBehavior.AllowGet);
                }

                printer = printer.Replace("\\\\", "!");
                printer = printer.Replace("\\", "@");

                String X_Return_String = "";

                string str = "select Print_Seq_Number_Bill from FI_GIISHEAD where ref_no =" + id;
                MyConnection.conn.Open();
                SqlCommand cmd = new SqlCommand(str, MyConnection.conn);
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                cmd.Dispose();
                MyConnection.conn.Close();
                //var i=MyConnection._db.FI_Web_BILL_Print_requests.Where(x=>x.bill_no ==id).Select(x=>x.bill_no).FirstOrDefault();
                if (i == 0)
                {
                    //-----------------------------------------Bill PRINT---------------------------------------------------                    

                    //obj.bill_no = id;
                    //obj.Req_DtTime = DateTime.Now;
                    //obj.Req_Status = 0;
                    //obj.Printed_DTtime = DateTime.Now;

                    //MyConnection._db.FI_Web_BILL_Print_requests.Add(obj);
                    //MyConnection._db.SaveChanges();

                    if (true)
                    {
                        if (true)
                        {
                            ThinkHPTLM.ThinkHPTLMTrans X_Proc_Save = new ThinkHPTLM.ThinkHPTLMTrans(Convert.ToBoolean(MyConnection.X_Local_Station), MyConnection.X_TMATS_User_Name, MyConnection.X_TMATS_User_Password, MyConnection.X_Db_Type, MyConnection.X_Db_Name, MyConnection.X_Db_Servername, MyConnection.X_Db_UserName);

                            Boolean X_RR;

                            X_RR = X_Proc_Save.M_Proc_Print_Invoice_DLL(id, printer, "web", "Web User", Request.Cookies["UserType"].Value, 999, "web", ref X_Return_String);

                            if (X_RR == false)
                            {
                                if (X_Return_String != "")
                                {
                                    //Response.Write("<script>alert('" + X_Return_String + "');</script>");
                                    return Json(new { msg = X_Return_String }, JsonRequestBehavior.AllowGet);

                                }
                            }
                            else
                            {
                                X_Return_String = "Bill Printing DONE!!!";
                            }
                            // return Json(new { redirectUrl = Url.Action("Menu", "RTC"), isRedirect = true }, JsonRequestBehavior.AllowGet);
                        }
                    }

                    //-----------------------------------------Bill PRINT---------------------------------------------------
                    return Json(new { redirectUrl = Url.Action("Menu", "RTC"), isRedirect = true, X_Return = X_Return_String }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    str = "select Count(*) from FI_User_Rights_Master where User_ID=" + Request.Cookies["User_ID"].Value + " and Right_ID=1061";
                    MyConnection.conn.Open();
                    cmd = new SqlCommand(str, MyConnection.conn);
                    int j = Convert.ToInt32(cmd.ExecuteScalar());
                    if (j > 0)
                    {
                        cmd.Dispose();
                        MyConnection.conn.Close();

                        if (true)
                        {
                            if (true)
                            {
                                //ThinkHPTLM.ThinkHPTLMTrans X_Proc_Save = new ThinkHPTLM.ThinkHPTLMTrans(Convert.ToBoolean(ConfigurationManager.AppSettings["X_Local_Station"]), ConfigurationManager.AppSettings["X_TMATS_User_Name"].ToString(), ConfigurationManager.AppSettings["X_TMATS_User_Password"].ToString(), ConfigurationManager.AppSettings["XMyConnection._db_Type"].ToString(), ConfigurationManager.AppSettings["XMyConnection._db_Name"].ToString(), ConfigurationManager.AppSettings["XMyConnection._db_Servername"].ToString(), ConfigurationManager.AppSettings["XMyConnection._db_UserName"].ToString());
                                ThinkHPTLM.ThinkHPTLMTrans X_Proc_Save = new ThinkHPTLM.ThinkHPTLMTrans(Convert.ToBoolean(MyConnection.X_Local_Station), MyConnection.X_TMATS_User_Name, MyConnection.X_TMATS_User_Password, MyConnection.X_Db_Type, MyConnection.X_Db_Name, MyConnection.X_Db_Servername, MyConnection.X_Db_UserName);

                                Boolean X_RR;

                                X_RR = X_Proc_Save.M_Proc_Print_Invoice_DLL(id, printer, "web", "Web User", Request.Cookies["UserType"].Value, 999, "web", ref X_Return_String);

                                if (X_RR == false)
                                {
                                    if (X_Return_String != "")
                                    {
                                        return Json(new { msg = X_Return_String }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    X_Return_String = "Bill Printing DONE!!!";
                                }
                                // return Json(new { redirectUrl = Url.Action("Menu", "RTC"), isRedirect = true }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        //-----------------------------------------Bill PRINT---------------------------------------------------
                        return Json(new { redirectUrl = Url.Action("Menu", "RTC"), isRedirect = true, X_Return = X_Return_String }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        cmd.Dispose();
                        MyConnection.conn.Close();
                        return Json(new { msg = "Please Contact to Administrator" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                string err = ex.Message + "  at  " + line;
                return Json(new { msg = err }, JsonRequestBehavior.AllowGet);
            }
        }
        public Double price(double SALE_PRICE, double Dine_IN_Price, double Counter_Price, double Corporate_Price, double Delivery_Price)
        {
            Double price = 0;
            price = SALE_PRICE;
            if (Request.Cookies["Mode"].Value == "DINE IN" && Dine_IN_Price != 0)
            {
                price = Dine_IN_Price;
            }
            if (Request.Cookies["Mode"].Value == "TAKE AWAY" && Counter_Price != 0)
            {
                price = Counter_Price;
            }
            if (Request.Cookies["Mode"].Value == "CORPORATE" && Corporate_Price != 0)
            {
                price = Corporate_Price;
            }
            if (Request.Cookies["Mode"].Value == "DELIVERY" && Delivery_Price != 0)
            {
                price = Delivery_Price;
            }
            return price;
        }
        public JsonResult GetMenuItem(string grp_cd)
        {
            try
            {
                // Call SaveProductImages method to save images to the file system
                SaveProductImages(MyConnection.conn.ConnectionString);
                if (grp_cd == "99999")
                {
                    var First = MyConnection._db.fI_GRP_MASTERs.Where(x => x.GROUP_YN == "N" && x.GRP_CD != "00001").OrderBy(x => x.GRP_SNAME).Select(x => x.GRP_CD).FirstOrDefault();
                    grp_cd = First;
                }
                if (grp_cd == "-1")
                {

                    var getall = MyConnection._db.FI_Itmmasts.
                        Join(MyConnection._db.fI_GRP_MASTERs, i => i.GRP_CD, g => g.GRP_CD, (i, g) => new { i, g }).
                        Where(x => x.g.Status == "Active" && x.i.BAR_CODE_NON_BAR_CODE == "B").
                        OrderBy(x => x.i.ITEM_DESC).
                        Select(x => new { x.i.ITEM_CD, x.i.ITEM_DESC, x.i.ITEM_DESC_SL, x.i.SALE_PRICE, x.i.Dine_IN_Price, x.i.Counter_Price, x.i.Corporate_Price, x.i.Delivery_Price, x.i.Image_Path, x.i.Inventory_Item_Code, x.i.GRP_CD }).
                        ToList();


                    List<FI_Itmmast> menu = new List<FI_Itmmast>();
                    Combo_And_Modifier cam = new Combo_And_Modifier(MyConnection.conn);
                    string loc_code = cam.LOC_CODE();

                    menu = (from item in getall
                                //join  img in MyConnection._db.fi_itmmast_Images
                                //on item.ITEM_CD equals img.itemID
                            select new FI_Itmmast()
                            {
                                ITEM_CD = item.ITEM_CD,
                                ITEM_DESC = item.ITEM_DESC,
                                ITEM_DESC_SL = item.ITEM_DESC_SL,
                                //SALE_PRICE = item.SALE_PRICE,
                                SALE_PRICE = price(item.SALE_PRICE, item.Dine_IN_Price, item.Counter_Price, item.Corporate_Price, item.Delivery_Price),
                                Image_Path = $"/images/Product_Images/{item.ITEM_CD}_{item.ITEM_CD}.jpg",
                                Inventory_Item_Code = item.Inventory_Item_Code,
                                stockqty = cam.GetStock(item.Inventory_Item_Code, loc_code)
                            }
                            ).ToList();

                    List<Products> mProductList = new List<Products>();
                    foreach (var item in menu)
                    {
                        Products mProducts = new Products();

                        
                    }

                    cam.Dispose();

                    //return Json(menu, JsonRequestBehavior.AllowGet);
                    var jsonResult = Json(menu, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;

                }
                if (grp_cd == "-2")
                {
                    var menu = MyConnection._db.FI_Itmmasts.Where(x => x.ACTIVE_INACTIVE == "A" && x.BAR_CODE_NON_BAR_CODE == "N").OrderBy(x => x.ITEM_DESC).Select(x => new { x.ITEM_CD, x.ITEM_DESC, x.ITEM_DESC_SL, x.SALE_PRICE, x.Image_Path }).ToList();
                    var jsonResult = Json(menu, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                else
                {
                    //var menu = MyConnection._db.FI_Itmmasts.Where(x => x.BAR_CODE_NON_BAR_CODE == "B" && x.GRP_CD == grp_cd).OrderBy(x => x.ITEM_DESC).ToList();
                    string str = "select ITEM_CD, ITEM_DESC,'ITEM' AS[TYPE],SALE_PRICE ,Dine_IN_Price,Drive_Through_Price,Counter_Price,Delivery_Price,Corporate_Price,Service_Provider_Require,ItemID,Product_Mode,ITEM_DESC_SL as [ITEM_DESC_SL],Order_By,Item_Button_Base_Color_ARGB AS[IBC], Item_Button_Fore_Color_ARGB AS[IFC] ,0 as [Stock]  ,Inventory_Item_Code,Item_Button_Font_Name,Item_Button_Font_Size,Item_Button_Font_Bold,Make_Time,Image_Path" +
                                 " from   FI_ITMMAST a   where GRP_CD = '" + grp_cd + "'  and BAR_CODE_NON_BAR_CODE = 'B' and(ACTIVE_INACTIVE = 'A' or ACTIVE_INACTIVE = 'Active') and Product_Mode<> 'Modifier' " +
                                 "union all " +
                                 "select grp_Cd AS[ITEM_CD],GRP_SNAME AS[ITEM_DESC],'GROUP' AS[TYPE] ,0 ,0,0,0,0,0,'No',0,'Single' as [Product_Mode],GRP_SNAME_SL as [ITEM_DESC_SL] ,Order_By,Group_Button_Base_Color_ARGB AS[IBC], Group_Button_Fore_Color_ARGB AS[IFC] ,0 as [Stock] ,' ' as Inventory_Item_Code,'Arial',Font_Size,1,0 as  [Make_Time],'' as [Image_Path] FROM FI_GRP_MASTER  WHERE FOR_GRP_CD = '" + grp_cd + "' order by Order_By,ITEM_DESC";

                    DbProviderFactory f = DbProviderFactories.GetFactory("System.Data.SqlClient");
                    DataTable dt = new DataTable();

                    using (DbConnection connection = f.CreateConnection())
                    {

                        //connection.ConnectionString = ConfigurationManager.ConnectionStrings["conn"].ToString();
                        MyConnection.Main(null);
                        connection.ConnectionString = MyConnection.conn.ConnectionString;

                        connection.Open();

                        DbCommand command = f.CreateCommand();
                        // command.CommandText = "select * from FI_Table_Master";
                        command.CommandText = str;
                        command.Connection = connection;

                        IDataReader reader = command.ExecuteReader();
                        dt.Load(reader);
                        reader.Close();
                        connection.Close();

                    }
                    List<FI_Itmmast> menu = new List<FI_Itmmast>();


                    Combo_And_Modifier cam = new Combo_And_Modifier(MyConnection.conn);
                    string loc_code = cam.LOC_CODE();

                    menu = (from DataRow dr in dt.Rows
                            select new FI_Itmmast()
                            {
                                ITEM_CD = dr["ITEM_CD"].ToString(),
                                ITEM_DESC = dr["ITEM_DESC"].ToString(),
                                ITEM_DESC_SL = dr["ITEM_DESC_SL"].ToString(),
                                TYPE_OF_ITEM = dr["TYPE"].ToString(),
                                SALE_PRICE = Convert.ToDouble(dr["SALE_PRICE"]),
                                Image_Path = dr["Image_Path"].ToString(),

                                Inventory_Item_Code = dr["Inventory_Item_Code"].ToString(),
                                stockqty = cam.GetStock(dr["Inventory_Item_Code"].ToString(), loc_code)


                            }).ToList();
                    cam.Dispose();
                    var jsonResult = Json(menu, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveProductImages(string connectionString)
        {
            // Define folder path for images
            string imagesFolderPath = HostingEnvironment.MapPath("~/images/Product_Images");



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL query to join tables and retrieve image data
                string sql = @"
            SELECT i.ITEM_CD, img.Image_Data, img.itemID
            FROM FI_Itmmast i
            INNER JOIN FI_itmmast_Images img ON i.ItemID = img.itemID";

                SqlCommand cmd = new SqlCommand(sql, connection);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string itemCd = reader.GetString(0); // Get ITEM_CD
                        byte[] imageData = (byte[])reader.GetValue(1); // Get Image_Data
                        int imageId = reader.GetInt32(2); // Get itemID

                        // Generate a unique filename using itemID and item code
                        string fileName = $"{imageId}_{itemCd}.jpg";

                        // Combine file path with unique filename
                        string filePath = Path.Combine(imagesFolderPath, fileName);

                        // Save the image data to the file
                        System.IO.File.WriteAllBytes(filePath, imageData);
                    }
                }
            }
        }

        /* public JsonResult GetMenuItem(string grp_cd)
         {
             try
             {
                 string path = @"D:\Image";
                 //string Path = HttpContext.CurrentNotification.Server.MapPath("~/images/Games"); //Path

                 if (grp_cd == "99999")
                 {
                     var First = MyConnection._db.fI_GRP_MASTERs
                         .Where(x => x.GROUP_YN == "N" && x.GRP_CD != "00001")
                         .OrderBy(x => x.GRP_SNAME)
                         .Select(x => x.GRP_CD)
                         .FirstOrDefault();
                     grp_cd = First;
                 }

                 if (grp_cd == "-1")
                 {
                     var getall = MyConnection._db.FI_Itmmasts
                         .Join(MyConnection._db.fI_GRP_MASTERs, i => i.GRP_CD, g => g.GRP_CD, (i, g) => new { i, g })
                         .Where(x => x.g.Status == "Active" && x.i.BAR_CODE_NON_BAR_CODE == "B")
                         .OrderBy(x => x.i.ITEM_DESC)
                         .Select(x => new {
                             x.i.ITEM_CD,
                             x.i.ITEM_DESC,
                             x.i.ITEM_DESC_SL,
                             x.i.SALE_PRICE,
                             x.i.Dine_IN_Price,
                             x.i.Counter_Price,
                             x.i.Corporate_Price,
                             x.i.Delivery_Price,
                             x.i.Image_Path,
                             x.i.Inventory_Item_Code,
                             x.i.GRP_CD,
                             x.i.ItemID
                         })
                         .ToList();

                     List<FI_Itmmast> menu = new List<FI_Itmmast>();
                     Combo_And_Modifier cam = new Combo_And_Modifier(MyConnection.conn);
                     string loc_code = cam.LOC_CODE();

                     menu = (from item in getall
                             select new FI_Itmmast()
                             {
                                 ITEM_CD = item.ITEM_CD,
                                 ItemID = item.ItemID,
                                 ITEM_DESC = item.ITEM_DESC,
                                 ITEM_DESC_SL = item.ITEM_DESC_SL,
                                 SALE_PRICE = price(item.SALE_PRICE, item.Dine_IN_Price, item.Counter_Price, item.Corporate_Price, item.Delivery_Price),
                                 Inventory_Item_Code = item.Inventory_Item_Code,
                                 stockqty = cam.GetStock(item.Inventory_Item_Code, loc_code)
                             }).ToList();

                     List<Products> mProductList = new List<Products>();
                     foreach (var item in menu)
                     {

                         Products mProducts = new Products
                         {
                             ITEM_CD = item.ITEM_CD,
                             ITEM_DESC = item.ITEM_DESC,
                             ITEM_DESC_SL = item.ITEM_DESC_SL,
                             SALE_PRICE = item.SALE_PRICE,
                             Inventory_Item_Code = item.Inventory_Item_Code,
                             stockqty = item.stockqty
                         };

                         var IMAGEDATA = MyConnection._db.FI_itmmast_Images.ToList();
                         var images = MyConnection._db.FI_itmmast_Images
                             .Where(img => img.itemID == item.ItemID)
                             // .Select(img => Convert.ToBase64String(img.Image_Data)) // Convert binary data to base64 string
                             .ToList();
                         /*foreach (var image in images)
                         {
                             string Binaryimage = Convert.ToBase64String(image.Image_Data);
                             System.IO.File.WriteAllBytes(System.IO.MapPath(path), image.Image_Data);
                             mProducts.ProductImages.Add(Binaryimage);
                         }

                         mProductList.Add(mProducts);
                     }
                     cam.Dispose();

                     var jsonResult = Json(mProductList, JsonRequestBehavior.AllowGet);
                     jsonResult.MaxJsonLength = int.MaxValue;
                     return jsonResult;
                 }

                 if (grp_cd == "-2")
                 {
                     var menu = MyConnection._db.FI_Itmmasts
                         .Where(x => x.ACTIVE_INACTIVE == "A" && x.BAR_CODE_NON_BAR_CODE == "N")
                         .OrderBy(x => x.ITEM_DESC)
                         .Select(x => new Products
                         {
                             ITEM_CD = x.ITEM_CD,
                             ITEM_DESC = x.ITEM_DESC,
                             ITEM_DESC_SL = x.ITEM_DESC_SL,
                             SALE_PRICE = x.SALE_PRICE,
                             Inventory_Item_Code = x.Inventory_Item_Code,
                             stockqty = 0, // You can change this to the actual stock quantity logic if needed
                             ProductImages = MyConnection._db.FI_itmmast_Images
                                 .Where(img => img.itemID == x.ItemID)
                                 .Select(img => Convert.ToBase64String(img.Image_Data)) // Convert binary data to base64 string
                                 .ToList()
                         })
                         .ToList();

                     var jsonResult = Json(menu, JsonRequestBehavior.AllowGet);
                     jsonResult.MaxJsonLength = int.MaxValue;
                     return jsonResult;
                 }
                 else
                 {
                     string str = "select ITEM_CD, ITEM_DESC,'ITEM' AS[TYPE],SALE_PRICE ,Dine_IN_Price,Drive_Through_Price,Counter_Price,Delivery_Price,Corporate_Price,Service_Provider_Require,ItemID,Product_Mode,ITEM_DESC_SL as [ITEM_DESC_SL],Order_By,Item_Button_Base_Color_ARGB AS[IBC], Item_Button_Fore_Color_ARGB AS[IFC] ,0 as [Stock]  ,Inventory_Item_Code,Item_Button_Font_Name,Item_Button_Font_Size,Item_Button_Font_Bold,Make_Time,Image_Path" +
                                  " from   FI_ITMMAST a   where GRP_CD = '" + grp_cd + "'  and BAR_CODE_NON_BAR_CODE = 'B' and(ACTIVE_INACTIVE = 'A' or ACTIVE_INACTIVE = 'Active') and Product_Mode<> 'Modifier' " +
                                  "union all " +
                                  "select grp_Cd AS[ITEM_CD],GRP_SNAME AS[ITEM_DESC],'GROUP' AS[TYPE] ,0 ,0,0,0,0,0,'No',0,'Single' as [Product_Mode],GRP_SNAME_SL as [ITEM_DESC_SL] ,Order_By,Group_Button_Base_Color_ARGB AS[IBC], Group_Button_Fore_Color_ARGB AS[IFC] ,0 as [Stock] ,' ' as Inventory_Item_Code,'Arial',Font_Size,1,0 as  [Make_Time],'' as [Image_Path] FROM FI_GRP_MASTER  WHERE FOR_GRP_CD = '" + grp_cd + "' order by Order_By,ITEM_DESC";

                     DbProviderFactory f = DbProviderFactories.GetFactory("System.Data.SqlClient");
                     DataTable dt = new DataTable();

                     using (DbConnection connection = f.CreateConnection())
                     {
                         MyConnection.Main(null);
                         connection.ConnectionString = MyConnection.conn.ConnectionString;
                         connection.Open();

                         DbCommand command = f.CreateCommand();
                         command.CommandText = str;
                         command.Connection = connection;

                         IDataReader reader = command.ExecuteReader();
                         dt.Load(reader);
                         reader.Close();
                         connection.Close();
                     }

                     List<FI_Itmmast> menu = new List<FI_Itmmast>();
                     Combo_And_Modifier cam = new Combo_And_Modifier(MyConnection.conn);
                     string loc_code = cam.LOC_CODE();

                     menu = (from DataRow dr in dt.Rows
                             select new FI_Itmmast()
                             {
                                 ITEM_CD = dr["ITEM_CD"].ToString(),
                                 ITEM_DESC = dr["ITEM_DESC"].ToString(),
                                 ITEM_DESC_SL = dr["ITEM_DESC_SL"].ToString(),
                                 SALE_PRICE = Convert.ToDouble(dr["SALE_PRICE"]),
                                 Inventory_Item_Code = dr["Inventory_Item_Code"].ToString(),
                                 stockqty = cam.GetStock(dr["Inventory_Item_Code"].ToString(), loc_code)
                             }).ToList();

                     List<Products> mProductList = new List<Products>();
                     foreach (var item in menu)
                     {
                         Products mProducts = new Products
                         {
                             ITEM_CD = item.ITEM_CD,
                             ITEM_DESC = item.ITEM_DESC,
                             ITEM_DESC_SL = item.ITEM_DESC_SL,
                             SALE_PRICE = item.SALE_PRICE,
                             Inventory_Item_Code = item.Inventory_Item_Code,
                             stockqty = item.stockqty
                         };

                         var images = MyConnection._db.FI_itmmast_Images
                         .Where(img => img.itemID == item.ItemID)
                         .Select(img => Convert.ToBase64String(img.Image_Data.ToArray())) // Convert binary data to base64 string
                         .ToList();


                         mProducts.ProductImages = images;
                         mProductList.Add(mProducts);
                     }
                     cam.Dispose();

                     var jsonResult = Json(mProductList, JsonRequestBehavior.AllowGet);
                     jsonResult.MaxJsonLength = int.MaxValue;
                     return jsonResult;
                 }
             }
             catch (Exception ex)
             {
                 var dd = ex.Message;
                 var dd1 = ex.StackTrace;
                 var dd2 = ex.InnerException;

                 // Log exception (ex)
                 return Json(new { error = "An error occurred while fetching menu items." }, JsonRequestBehavior.AllowGet);
             }
         }*/




        public JsonResult CART_COUNT()
        {
            try
            {
                //var count = Session["Cart"]; Jay Shah 22-5-24
                var count = Request.Cookies["Cart"]?.Value;
                return Json(count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public bool IsOpen(string id)
        {
            //select ALLOW_CHANGE_RATE,Change_Name_Allow from FI_Itmmast where ( Change_Name_Allow ='Yes' or ALLOW_CHANGE_RATE ='Y' ) and ITEM_CD='213'
            //var result =MyConnection._db.FI_Itmmasts.Where(x=>x.ITEM_CD == id && x.BAR_CODE_NON_BAR_CODE!="B" && x.Change_Name_Allow == "Yes" | x.ALLOW_CHANGE_RATE =="Y").ToList();
            var result = MyConnection._db.FI_Itmmasts.Where(x => x.ITEM_CD == id && x.ALLOW_CHANGE_RATE == "Y").Select(x => new { x.ITEM_CD, x.ITEM_DESC, x.ITEM_DESC_SL }).ToList();
            if (result.Count > 0)
            {
                //Session["openitemid"] = result.Where(x => x.ITEM_CD == id).Select(x => x.ITEM_CD).FirstOrDefault(); Jay Shah 22-5-24
                Response.Cookies["openitemid"].Value = result.FirstOrDefault(x => x.ITEM_CD == id)?.ITEM_CD ?? "";

                return true;
            }
            return false;
        }
        public bool IsOpenItemRemark()
        {
            //select ALLOW_CHANGE_RATE,Change_Name_Allow from FI_Itmmast where ( Change_Name_Allow ='Yes' or ALLOW_CHANGE_RATE ='Y' ) and ITEM_CD='213'
            var result = MyConnection._db.FI_Itmmasts.Where(x => x.ITEM_CD == "NA" && x.ACTIVE_INACTIVE == "A").Select(x => new { x.ITEM_CD, x.ItemID, x.BAR_CODE_NON_BAR_CODE }).FirstOrDefault();
            if (result == null)
            {
                return false;
            }
            if (result.BAR_CODE_NON_BAR_CODE == "N")
            {
                //Session["openitemid"] = "NA"; Jay Shah 22-5-24
                Response.Cookies["openitemid"].Value = "NA";

                return true;
            }
            return false;
        }
        public JsonResult OpenItemRemark()
        {
            if (IsOpenItemRemark())
            {
                return Json(new { IsOpen = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { IsOpen = false }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReturnCART(string id, string remark, string lang)
        {
            try
            {
                var aaa = MyConnection._db.FI_Itmmasts.Where(p => p.ITEM_CD == id).Select(x => new { x.SALE_PRICE, x.ITEM_DESC, x.ITEM_DESC_SL }).FirstOrDefault();

                Product product = new Product();

                if (GetCart().lineCollection.Count < 1)
                {
                    product.Sr = 1;
                    //Session["Srno"] = product.Sr;
                }
                else
                {
                    //int srno = Convert.ToInt32(Session["Srno"]);
                    int srno = GetCart().lineCollection.Count;//30aug2023
                    product.Sr = srno + 1;
                    //Session["Srno"] = product.Sr;
                }
                product.ProductID = id;
                product.Quantity = 1;
                product.Price = Convert.ToDecimal(aaa.SALE_PRICE);
                if (aaa.ITEM_DESC_SL.Trim() != "")
                {
                    if (lang == "Arabic")
                    {
                        product.Item_Description_SL = aaa.ITEM_DESC_SL;
                        product.Item_Description = aaa.ITEM_DESC;
                    }
                    else
                    {
                        product.Item_Description = aaa.ITEM_DESC;
                    }
                }
                else
                {
                    product.Item_Description = aaa.ITEM_DESC;

                }
                product.Item_Return_Remark = remark;

                GetCart().AddItem(product, -1);
                //var count = Session["Cart"]; Jay Shah 22-5-24
                var count = Request.Cookies["Cart"]?.Value;

                return Json(count, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult ReturnMinus(int id)
        {
            try
            {
                var aaa = GetCart().lineCollection.FirstOrDefault(p => p.Product.Sr == id);
                //var aaa = MyConnection._db.FI_Itmmasts.FirstOrDefault(p => p.ITEM_CD == id);

                Product product = new Product();
                product.Sr = id;
                product.ProductID = aaa.Product.ProductID;
                product.Quantity = 1;
                product.Price = aaa.Product.Price;
                product.Item_Description = aaa.Product.Item_Description;

                GetCart().ReturnItem(product, -1);
                //var count = Session["Cart"]; Jay Shah 22-5-24
                var count = Request.Cookies["Cart"]?.Value;

                return Json(count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //-------------------------Modifier and combo------------------------------------
        public ActionResult Modifier(string id, int qty)
        {
            var prod_nm = MyConnection._db.FI_Itmmasts.Where(x => x.ITEM_CD == id).Select(x => x.ITEM_DESC).FirstOrDefault();
            //Session["prod_nm"] = prod_nm; Jay Shah 22-5-24
            Response.Cookies["prod_nm"].Value = prod_nm;

            string finalop = "";
            DataTable dt;
            //----------------MAX STEPS--------------------------
            Combo_And_Modifier cam = new Combo_And_Modifier(MyConnection.conn);
            //int steps = cam.CountSteps(id);
            var itemcomboid = MyConnection._db.FI_Itmmasts.Where(x => x.ITEM_CD == id).Select(x => x.ItemID).FirstOrDefault();
            int steps = cam.CountSteps(itemcomboid.ToString());
            cam.Dispose();

            //-----------------html------------------------------
            for (int i = 1; i < steps + 1; i++)
            {
                TagBuilder tabal = new TagBuilder("table");
                tabal.Attributes["id"] = "tblCMitems";
                TagBuilder thead = new TagBuilder("thead");
                TagBuilder tbody = new TagBuilder("tbody");

                cam = new Combo_And_Modifier(MyConnection.conn);
                //int Maxqty = cam.Maxqty(id, i);
                int Maxqty = cam.Maxqty(itemcomboid.ToString(), i, qty);

                TagBuilder h5 = new TagBuilder("h5");
                h5.InnerHtml = "Step No :" + i + " Maximum selection limmit is:" + Maxqty;

                finalop += h5.ToString();

                TagBuilder hidden = new TagBuilder("hidden");
                hidden.InnerHtml = "<input type='hidden' id='hidden" + i + "' value='" + Maxqty + "'/>";
                finalop += hidden.ToString();

                //ViewBag.step = new HtmlString(finalop);
                //-------------------step wise item--------------
                //DataTable dt= cam.StepBaseItems(id, conn,i);
                //dt = cam.StepBaseItems(id, i);
                dt = cam.StepBaseItems(itemcomboid.ToString(), i);
                //["ComboID"].ToString(),
                TagBuilder tr = new TagBuilder("tr");

                //TagBuilder th1 = new TagBuilder("th");

                tr.InnerHtml = "<th> </th>" +
                                "<th> Qty </th>" +
                                "<th> </th>" +
                                "<th> ITEM </th>" +
                                "<th> Price </th>" +
                                "<th style='display:none'> itemid </th>" +
                                "<th style='display:none'> comboid </th>";
                //"<th> ITEM_CD </th>" +
                //"<th> ComboID</th>" +
                //"<th> ItemID </th>" +
                //"<th> Step_No </th>" +
                //"<th> combo_type </th>";

                //StringBuilder html = new StringBuilder();
                //html.Append(tr.ToString());

                thead.InnerHtml = tr.ToString();
                string FinalDataStr = "";
                string maxqty = "";

                foreach (DataRow row in dt.Rows)
                {
                    maxqty = row["Qty"].ToString();
                    TagBuilder rowtr = new TagBuilder("tr");

                    TagBuilder rowtd = new TagBuilder("td");
                    rowtd.InnerHtml = "<input id='minus' type='button' value='-' class='btn btn-default' onclick='minusqty(" + row["ItemID"].ToString().Trim() + "," + row["Step_No"].ToString() + ");'/>";
                    rowtr.InnerHtml = rowtd.ToString();

                    /*value='" + 0 + "'*/
                    rowtd = new TagBuilder("td");
                    rowtd.Attributes["id"] = row["ItemID"].ToString();
                    rowtd.InnerHtml = "<input id='qty" + row["ItemID"].ToString().Trim() + "' value='0' class='qtyz' type='text' min='0' readonly/>";
                    rowtr.InnerHtml += rowtd.ToString();

                    rowtd = new TagBuilder("td");
                    rowtd.InnerHtml = "<input id='plus' type='button' value='+' class='btn btn-default' onclick='plusqty(" + row["ItemID"].ToString().Trim() + "," + row["Step_No"].ToString() + ")'/>";
                    rowtr.InnerHtml += rowtd.ToString();

                    rowtd = new TagBuilder("td");
                    rowtd.InnerHtml = row["ITEM_DESC"].ToString();
                    rowtr.InnerHtml += rowtd.ToString();

                    rowtd = new TagBuilder("td");
                    rowtd.InnerHtml = row["Sale_Price"].ToString();
                    rowtr.InnerHtml += rowtd.ToString();

                    rowtd = new TagBuilder("td");
                    rowtd.Attributes["style"] = "display:none";
                    rowtd.InnerHtml = row["ITEM_CD"].ToString();
                    rowtr.InnerHtml += rowtd.ToString();

                    rowtd = new TagBuilder("td");
                    rowtd.Attributes["style"] = "display:none";
                    rowtd.Attributes["id"] = "cmbid";
                    rowtd.InnerHtml = row["ComboID"].ToString();
                    rowtr.InnerHtml += rowtd.ToString();

                    //string file7 = row["ITEM_CD"].ToString();
                    //string file  = row["ComboID"].ToString();
                    //string file1 = row["ItemID"].ToString();
                    //string file2 = row["Step_No"].ToString();
                    //string file4 = row["combo_type"].ToString();


                    //TagBuilder hidden = new TagBuilder("input");
                    //hidden.Attributes["type"] = "hidden";
                    //hidden.Attributes["value"] = maxqty;
                    //hidden.Attributes["id"] ="hidden"+ i;

                    //rowtd = new TagBuilder("td");
                    //rowtd.InnerHtml = hidden.ToString();
                    //rowtr.InnerHtml += rowtd.ToString();


                    FinalDataStr += rowtr.ToString();

                }
                cam.Dispose();

                tbody.InnerHtml = FinalDataStr;
                tabal.InnerHtml = thead.ToString() + tbody.ToString();

                //h5 = new TagBuilder("h5");
                //h5.InnerHtml = " Maximum selection limmit is:" + maxqty;

                //finalop += h5.ToString();
                finalop += tabal.ToString();
                ViewBag.step = new HtmlString(finalop);
            }

            //-----------------html------------------------------

            return View();
        }
        public class products
        {
            public string itemid { get; set; }
            public string comboid { get; set; }
            public int qty { get; set; }
            public string item { get; set; }
            public int price { get; set; }
        }
        [HttpPost]
        public ActionResult Modifier(List<products> product)
        {
            string lang = "";
            foreach (products p in product)
            {
                var aaa = MyConnection._db.FI_Itmmasts.Where(x => x.ITEM_CD == p.itemid).Select(x => new { x.SALE_PRICE, x.ITEM_DESC, x.ITEM_DESC_SL }).FirstOrDefault();

                Product pr = new Product();

                if (GetCart().lineCollection.Count < 1)
                {
                    pr.Sr = 1;
                    //Session["Srno"] = pr.Sr;
                }
                else
                {
                    //int srno = Convert.ToInt32(Session["Srno"]);
                    int srno = GetCart().lineCollection.Count;//30aug2023
                    pr.Sr = srno + 1;
                    //Session["Srno"] = pr.Sr;
                }
                pr.ProductID = p.itemid;
                pr.Quantity = p.qty;
                //pr.Price = Convert.ToDecimal(aaa.SALE_PRICE);
                pr.Price = p.price;
                pr.ComboID = p.comboid;

                if (aaa.ITEM_DESC_SL.Trim() != "")
                {
                    if (lang == "Arabic")
                    {
                        pr.Item_Description_SL = aaa.ITEM_DESC_SL;
                        pr.Item_Description = aaa.ITEM_DESC;
                    }
                    else
                    {
                        pr.Item_Description = aaa.ITEM_DESC;
                    }
                }
                else
                {
                    pr.Item_Description = aaa.ITEM_DESC;
                }

                GetCart().AddItem(pr, (int)pr.Quantity);

            }
            //var count = Session["Cart"]; Jay Shah 22-5-24
            var count = Request.Cookies["Cart"]?.Value;


            //return View();
            return Json(new { redirectUrl = Url.Action("Menu", "RTC"), isRedirect = true, }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Modifier_Cancle_Order(string id)
        {
            GetCart().RemoveLineForCombo(id);
            //Session["Srno"] = GetCart().lines.Count();
            return Json(new { redirectUrl = Url.Action("Menu", "RTC"), isRedirect = true, }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddCART(string id, string lang)
        {
            try
            {
                if (ConfigurationManager.AppSettings["Open_Qty_Popup"].ToString() == "Yes")
                {
                    return Json(new { IsOpenQty = true, itm_id = id }, JsonRequestBehavior.AllowGet);
                }

                if (IsOpen(id))
                {
                    return Json(new { IsOpen = true }, JsonRequestBehavior.AllowGet);
                }
                //else
                //{
                //var aaa = MyConnection._db.FI_Itmmasts.Where(p => p.ITEM_CD == id).Select(x=>new { x.SALE_PRICE ,x.ITEM_DESC,x.ITEM_DESC_SL }).FirstOrDefault();
                var aaa = MyConnection._db.FI_Itmmasts.Where(p => p.ITEM_CD == id).Select(x => new { x.SALE_PRICE, x.Dine_IN_Price, x.Counter_Price, x.Corporate_Price, x.Delivery_Price, x.ITEM_DESC, x.ITEM_DESC_SL, x.ItemID }).FirstOrDefault();

                Product product = new Product();

                if (GetCart().lineCollection.Count < 1)
                {
                    product.Sr = 1;
                    //Session["Srno"] = product.Sr;
                }
                else
                {
                    //int srno = Convert.ToInt32(Session["Srno"]);
                    int srno = GetCart().lineCollection.Count;//30aug2023
                    product.Sr = srno + 1;
                    //Session["Srno"] = product.Sr;
                }
                product.ProductID = id;
                product.Quantity = 1;
                //product.Price = Convert.ToDecimal(aaa.SALE_PRICE);
                product.Price = Convert.ToDecimal(price(aaa.SALE_PRICE, aaa.Dine_IN_Price, aaa.Counter_Price, aaa.Corporate_Price, aaa.Delivery_Price));

                if (aaa.ITEM_DESC_SL.Trim() != "")
                {
                    if (lang == "Arabic")
                    {
                        product.Item_Description_SL = aaa.ITEM_DESC_SL;
                        product.Item_Description = aaa.ITEM_DESC;
                    }
                    else
                    {
                        product.Item_Description = aaa.ITEM_DESC;
                    }
                }
                else
                {
                    product.Item_Description = aaa.ITEM_DESC;
                }
                Combo_And_Modifier cam = new Combo_And_Modifier(MyConnection.conn);
                //isModifier
                //bool isModifier = cam.IsModifier(id);
                bool isModifier = cam.IsModifier((aaa.ItemID).ToString());
                cam.Dispose();
                if (isModifier)
                {
                    //product.ComboID = id;
                    product.ComboID = (aaa.ItemID).ToString();
                }
                else
                {
                    product.ComboID = "0";
                }

                GetCart().AddItem(product, 1);
                //var count = Session["Cart"]; Jay Shah 22-5-24
                var count = Request.Cookies["Cart"]?.Value;


                //Combo_And_Modifier cam = new Combo_And_Modifier();
                //isModifier
                if (isModifier)
                {
                    return Json(new { redirectUrl = Url.Action("Modifier", "RTC"), mid = id, isRedirect = true, }, JsonRequestBehavior.AllowGet);
                }
                //isCombo

                return Json(count, JsonRequestBehavior.AllowGet);
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult AddCARTqty(int id)
        {
            try
            {
                var aaa = GetCart().lineCollection.FirstOrDefault(p => p.Product.Sr == id);

                //if (aaa.Product.ComboID == "" || aaa.Product.ComboID == null)
                if (aaa.Product.ComboID == "0")
                {
                    Product product = new Product();

                    product.Sr = id;
                    product.ProductID = aaa.Product.ProductID;
                    product.Quantity = 1;
                    product.Price = aaa.Product.Price;
                    product.Item_Description = aaa.Product.Item_Description;

                    GetCart().AddItem(product, 1);
                    //var count = Session["Cart"]; Jay Shah 22-5-24
                    var count = Request.Cookies["Cart"]?.Value;

                    return Json(count, JsonRequestBehavior.AllowGet);
                    //}
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        //public JsonResult MinusqtyCART(string id)
        public JsonResult MinusqtyCART(int id)
        {
            var aaa = GetCart().lineCollection.FirstOrDefault(p => p.Product.Sr == id);
            //var aaa = MyConnection._db.FI_Itmmasts.FirstOrDefault(p => p.ITEM_CD == id);
            //if (aaa.Product.ComboID=="" || aaa.Product.ComboID == null)
            if (aaa.Product.ComboID == "0")
            {
                Product product = new Product();
                product.Sr = id;
                product.ProductID = aaa.Product.ProductID;
                product.Quantity = 1;
                product.Price = aaa.Product.Price;
                product.Item_Description = aaa.Product.Item_Description;

                //product.ProductID = id;
                //product.Qnautity = 1;
                //product.Price = Convert.ToDecimal(aaa.SALE_PRICE);
                //product.Item_Description = aaa.ITEM_DESC;

                GetCart().AddItem(product, -1);

                //var count = Session["Cart"]; Jay Shah 22-5-24
                var count = Request.Cookies["Cart"]?.Value;

                return Json(count, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //  public JsonResult MinusItemCART(string id)
        public JsonResult MinusItemCART(int id)
        {
            try
            {
                //var count = Session["Cart"]; Jay Shah 22-5-24
                var count = Request.Cookies["Cart"]?.Value;

                var aaa = GetCart().lineCollection.FirstOrDefault(p => p.Product.Sr == id);
                //var aaa = MyConnection._db.FI_Itmmasts.FirstOrDefault(p => p.ITEM_CD == id);
                //if (aaa.Product.ComboID == "" || aaa.Product.ComboID == null)
                if (aaa.Product.ComboID == "0")
                {
                    GetCart().RemoveLine(id);
                    //count = Session["Cart"]; Jay Shah 22-5-24
                    count = Request.Cookies["Cart"]?.Value;

                    return Json(count, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    GetCart().RemoveLineForCombo(aaa.Product.ComboID);
                    //count = Session["Cart"]; Jay Shah 22-5-24
                    count = Request.Cookies["Cart"]?.Value;
                    return Json(count, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public JsonResult ShowCARTitem()
        {
            try
            {
                //var JsonData = Session["Cart"]; Jay Shah 22-5-24
                var JsonData = Request.Cookies["Cart"]?.Value;
                return Json(JsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpPost]
        public JsonResult NullCARTitem()
        {
            try
            {
                //Cart cart = new Cart();
                /*Cart cart = (Cart)Session["Cart"];
                cart.Clear();
                Session["Cart"] = null;*/
                HttpCookie cartCookie = Request.Cookies["Cart"];
                if (cartCookie != null)
                {
                    // Clear the cart by removing the cookie
                    cartCookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cartCookie);
                }

                //var cartdata = Session["Cart"]; 
                //return Json(cartdata, JsonRequestBehavior.AllowGet);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //----------------bill nonte--------------------------
        public JsonResult Storebillnote(string blnote)
        {
            //Session["BillNote"] = blnote; jay Shah 22-5-24
            Response.Cookies["BillNote"].Value = blnote;

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //public JsonResult Insert(string fingerprint)
        public JsonResult Insert(int counter)
        {
            if (Request.Cookies["FingerPrint"] != null)
            {
                try
                {
                    var SessionID = System.Guid.NewGuid();
                    var id = SessionID.ToString();
                    //Cart c = (Cart)Session["Cart"]; Jay Shah 22-5-24

                    /// Retrieve the serialized cart from the cookie
                    string serializedCart = Request.Cookies["Cart"]?.Value;

                    // Deserialize the cart if the serialized cart is not null
                    Cart c;
                    if (serializedCart != null)
                    {
                        c = JsonConvert.DeserializeObject<Cart>(serializedCart);
                    }
                    else
                    {
                        // Handle the case where the cart cookie is not found or is null
                        c = new Cart();
                    }


                    IEnumerable<CartLine> itm = c.lines;

                    FI_Sale_Invoice_Pend_From_Cart_Start s = new FI_Sale_Invoice_Pend_From_Cart_Start();
                    FI_Sale_Invoice_Pend_From_Cart_End e = new FI_Sale_Invoice_Pend_From_Cart_End();

                    var newlist = new List<FI_Sale_Invoice_Pend_From_Cart_Start>();

                    if (c.lines.Count() > 0)
                    {
                        foreach (var item in itm)
                        {
                            if (String.IsNullOrEmpty(item.Product.Item_Description_SL))
                            {
                                newlist.Add(new FI_Sale_Invoice_Pend_From_Cart_Start()
                                {
                                    Session_ID = SessionID.ToString(),

                                    //Sr_No =srno,
                                    item_id = item.Product.ProductID,
                                    item_name = item.Product.Item_Description,
                                    Name_Display = item.Product.Item_Description,
                                    Rate = item.Product.Price,
                                    //Qty = item.Product.Qnautity,
                                    Qty = item.Qnautity,
                                    Uniq_ItemID = Convert.ToInt32(MyConnection._db.FI_Itmmasts.Where(x => x.ITEM_CD == item.Product.ProductID).Select(x => x.ItemID).FirstOrDefault()),
                                    Item_Return_Remark = item.Product.Item_Return_Remark,
                                    ComboID = Convert.ToInt32(item.Product.ComboID),
                                    deviceid = Request.Cookies["FingerPrint"].Value,
                                    Api_Ref_No = ConfigurationManager.AppSettings["Api_Ref_No"].ToString()
                                });
                            }
                            else
                            {
                                newlist.Add(new FI_Sale_Invoice_Pend_From_Cart_Start()
                                {
                                    Session_ID = SessionID.ToString(),
                                    //Sr_No =srno,
                                    item_id = item.Product.ProductID,
                                    Name_Display = item.Product.Item_Description_SL,
                                    item_name = item.Product.Item_Description,
                                    //MyConnection._db.FI_Itmmasts.Where(x => x.ITEM_CD == item.Product.ProductID).Select(x => x.ItemID).FirstOrDefault()
                                    Rate = item.Product.Price,
                                    //Qty = item.Product.Qnautity,
                                    Qty = item.Qnautity,
                                    Uniq_ItemID = Convert.ToInt32(MyConnection._db.FI_Itmmasts.Where(x => x.ITEM_CD == item.Product.ProductID).Select(x => x.ItemID).FirstOrDefault()),
                                    Item_Return_Remark = item.Product.Item_Return_Remark,
                                    ComboID = Convert.ToInt32(item.Product.ComboID),
                                    deviceid = Request.Cookies["FingerPrint"].Value,
                                    Api_Ref_No = ConfigurationManager.AppSettings["Api_Ref_No"].ToString()
                                });
                            }

                        }
                        MyConnection._db.fI_Sale_Invoice_Pend_From_Cart_Starts.AddRange(newlist);
                        MyConnection._db.SaveChanges();


                        e.Session_ID = SessionID.ToString();
                        e.User_ID = Convert.ToInt32(Request.Cookies["User_ID"].Value);
                        if (Request.Cookies["table"] == null || Request.Cookies["table"].Value.Trim() == "")
                        {
                            e.Table_No = "Online";
                        }
                        else
                        {
                            e.Table_No = Request.Cookies["table"].Value;
                        }
                        e.Session_date = System.DateTime.Now;
                        e.deviceid = Request.Cookies["FingerPrint"].Value;

                        if (Session["Old_ref_no"] == null)
                        {
                            e.Old_ref_no = 0;
                        }
                        else if ((Session["Old_ref_no"]).ToString() == "New Bill")
                        {
                            e.Old_ref_no = 0;
                        }
                        else
                        {
                            e.Old_ref_no = Convert.ToInt32(Session["Old_ref_no"]);
                        }


                        if ((Request.Cookies["Mode"]).Value == "DINE IN")
                        {
                            e.Order_Type = 1;
                        }
                        else if ((Request.Cookies["Mode"]).Value == "TAKE AWAY")
                        {
                            e.Order_Type = 2;
                        }
                        else if ((Request.Cookies["Mode"]).Value == "DELIVERY")
                        {
                            e.Order_Type = 3;
                        }
                        else if ((Request.Cookies["Mode"]).Value == "CORPORATE")
                        {
                            e.Order_Type = 4;
                        }

                        if (Session["Customer_ID"] == null)
                        {
                            e.Customer_ID = 0;
                        }
                        else
                        {
                            e.Customer_ID = Convert.ToInt32(Session["Customer_ID"].ToString());
                        }

                        e.User_Name = Request.Cookies["UserType"].Value;
                        e.PAX = Convert.ToInt32(Request.Cookies["PAX"].Value);
                        //e.bill_note = Session["BillNote"].ToString();
                        if (Session["BillNote"] == null)
                        {
                            e.bill_note = "";
                        }
                        else
                        {
                            e.bill_note = Session["BillNote"].ToString();
                        }
                        e.Api_Ref_No = ConfigurationManager.AppSettings["Api_Ref_No"].ToString();

                        MyConnection._db.fI_Sale_Invoice_Pend_From_Cart_Ends.Add(e);
                        MyConnection._db.SaveChanges();

                    }

                    Cart cart = (Cart)Session["Cart"];
                    //cart.Clear();
                    Session["Cart"] = null;

                    var jsonResult = Json(new { redirectUrl = Url.Action("Menu", "RTC"), isRedirect = true, success = true }, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                    // return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    //ViewBag.SaveError = e.Message.ToString();
                    //ViewBag.SaveError = e.InnerException.InnerException;
                    //var Error= e.InnerException.InnerException;
                    //e.InnerException != null
                    System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(e, true);
                    var Error = e.Message.ToString();
                    int line = trace.GetFrame(0).GetFileLineNumber();
                    if (e.InnerException != null)
                    {
                        Error = Error + e.InnerException.InnerException.ToString() + " Line:" + line;
                    }
                    return Json(new { success = false, message = Error + " Line:" + line }, JsonRequestBehavior.AllowGet);

                    //return Json(new { success = false, responseText = e.Message });
                    // return Json(new { success = false, msg = ViewBag.SaveError}, JsonRequestBehavior.AllowGet);
                    // Error=e.InnerException.InnerException; 
                    //throw e;
                }
                finally
                {
                    //nothing        
                }
            }
            return Json(new { redirectUrl = Url.Action("Index", "RTC"), isRedirect = true, success = false, message = "Session Expired, You want to Re-Login !!!" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PageRefresh()
        {
            Boolean R_freshVal = false;
            try
            {

                //added at 02/02/2022
                //string CheckUser = Session["UserType"].ToString();
                string CheckUser = Request.Cookies["id"].Value;
                if (CheckUser == null)
                {
                    R_freshVal = false;
                }
                else
                {
                    R_freshVal = true;
                }

                Session["Last_Refresh_Time"] = DateTime.Now;
                string Last_Refresh_Time = Session["Last_Refresh_Time"].ToString();
                //string Last_Refresh_Time = DateTime.Now.ToString();

                //return Json(new { R_fresh = true, Last_Refresh_Time = Last_Refresh_Time }, JsonRequestBehavior.AllowGet);
                return Json(new { R_fresh = R_freshVal, Last_Refresh_Time = Last_Refresh_Time }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //return Json(new { R_fresh = false }, JsonRequestBehavior.AllowGet);
                return Json(new { R_fresh = R_freshVal }, JsonRequestBehavior.AllowGet);
            }
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;

            }
            return cart;
        }
    }
}