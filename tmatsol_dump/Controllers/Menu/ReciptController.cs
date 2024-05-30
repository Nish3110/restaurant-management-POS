using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Table_Cart_V2k20.Models;
using System.Web.UI.WebControls;

namespace Table_Cart_V2k20.Controllers
{
    public class ReciptController : Controller
    {
        public static RTC_DBContext db = MyConnection._db;
        public static SqlConnection conn = MyConnection.conn;

        public static string DBServer = MyConnection.DBServer;
        public static string DBName = MyConnection.DBName;
        public static string DBuser = MyConnection.DBuser;
        public static string DBpass = MyConnection.DBpass;

        public object TableId { get; private set; }
        public object Mode { get; private set; }

        // GET: Recipt

        public ActionResult Index()
 
        {
            return View();
        }
        //------------------------------------------------------------------------------------

        public ActionResult Bill_Print() 
        {
            return View();
        }
        public Boolean IsAuthoriseForJson()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserType"] == null) // changed from Session["UserType"] == null
            {
                // IsAuth = false;
                //Session.Abandon();
                //Response.Redirect("RTC/DeviceInfo");
                //FormsAuthentication.RedirectFromLoginPage("~/RTC/DeviceInfo", false);
               /* Session.Clear();
                Session.Abandon();*/ // Jay Shah 22-5-24
                foreach (string cookieName in Request.Cookies.AllKeys)
                {
                    Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
                }

                return false;
                //return Json(null);
            }
            else
            {
                return true;
            }

        }
        public DataTable Comp_Info()
        {
            conn = MyConnection.conn;
            string X_Str = "select Decimal_Point,COMP_NAME from COMPANY_MASTER";

            conn.Open();
            SqlCommand cmd = new SqlCommand(X_Str, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();
            conn.Close();
            return dt;
        }
        public JsonResult GetHF()
        {
            if (IsAuthoriseForJson() == false)
            {
                return Json(null);
            }

            String Comp_Name = "";
            int Decimal_Point = 0;
            DataTable DT_compinfo = Comp_Info();

            foreach (DataRow item in DT_compinfo.Rows)
            {
                Comp_Name = item["COMP_NAME"].ToString();
                Decimal_Point = Convert.ToInt32(item["Decimal_Point"].ToString());
            }

            conn = MyConnection.conn;
            //string bill_no= Session["bprntID"].ToString(); Jay Shah 22-5-24
            string bill_no = Request.Cookies["bprntID"].Value;


            string X_Str = "select a.REF_NO,a.REF_DATE,a.PARTY_CD,Discount_Amount,Disc_Per,Gross_Amount,CLEARED_AMOUNT,a.Party_Detail_Party_cd,a.StationID"+
                           ",a.StationName,a.userID,a.userName,a.PAX,a.D_C_P_REF_NO,a.D_C_P_MODE,b.ITEM_CD,b.item_desc,b.QTY,a.OrderType,a.CustomerID,a.Bill_Note" +
                           ",(select First_name from FI_Service_Provider where Service_Provider_ID=b.Service_Provider_ID) as [ServiceProviderName]"+
                           ",(select item_desc_sl from fi_itmmast where item_cd=b.item_cd) as [Desc_other_Language]"+
                           ",(select Product_mode from fi_itmmast where item_cd=b.item_cd) as [Product_mode]"+
                           ",a.Driver_Name,b.TAX_Price_Mode,b.TAX1_RAmt,b.TAX2_RAmt,a.INV_Amount_WT_TAX,a.Tax_Amount,b.IMEI1,b.IMEI2,a.Net_Amount"+
                           ",(SELECT Product_Mode from fi_itmmast where item_cd = b.item_cd) as [ProductMode]"+
                           ",(SELECT Mode_Name from MODE_MASTER where MODE_CODE = a.OrderType) as [ModeType]"+
                           ",b.TAX1,b.TAX2,a.TRN" +
                           ",(select CallerID from FI_Caller_Master where CallerIDNumber=a.CustomerID) as [CustomerNo]"+
                           ",(select FName from FI_Caller_Master where CallerIDNumber=a.CustomerID) as [FiratName]"+
                           ",(select MName from FI_Caller_Master where CallerIDNumber=a.CustomerID) as [MiddleName]"+
                           ",(select Addl1 from FI_Caller_Master where CallerIDNumber=a.CustomerID) as [Address1]"+
                           ",(select Addl1 from FI_Caller_Master where CallerIDNumber=a.CustomerID) as [Address2]"+
                           ",(select Addl1 from FI_Caller_Master where CallerIDNumber=a.CustomerID) as [Address3]"+
                           ",(select Addl1 from FI_Caller_Master where CallerIDNumber=a.CustomerID) as [Address4]"+
                           ",(select Addl1 from FI_Caller_Master where CallerIDNumber=a.CustomerID) as [Address5]"+
                           ",TAX1_AMOUNT as [Total_TAXAmount_1]"+
                           ",TAX2_AMOUNT as [Total_TAXAmount_2]"+
                           ",b.Ftr_Ref_Date as [FtrRefDate]"+
                           ",(SELECT TYPE_OF_ITEM from fi_itmmast where item_cd = b.item_cd) as [item_Category]"+
                           ",a.ref_no_with_ps,Org_Rate,INV_Amount_WO_TAX,INV_Amount_WO_TAX,b.Record_no as [RecordNo]"+
                           " from FI_GIISHEAD a, FI_GIISFTR b "+
                           "where a.ref_no = b.ref_no and a.ref_no ='"+bill_no+"' order by b.RecordNo";

            conn.Open();
            SqlCommand cmd = new SqlCommand(X_Str, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();
            conn.Close();

            List<Bill_Header_Footer> bill_data = new List<Bill_Header_Footer>();
            bill_data = (from DataRow sdr in dt.Rows
                        select new Bill_Header_Footer()
                        {
                            Decimal_Point= Decimal_Point,
                            Company_Names = Comp_Name,
                            Headline1 = "2033, Burjuman Business Tower, 20th floor",
                            Headline2 = "Dubai, UAE",
                            //----------------------------------------------------------
                            Bill_No= sdr["Ref_No"].ToString(),
                            BillDate= sdr["Ref_Date"].ToString(),
                            Mode= sdr["D_C_P_REF_NO"].ToString()+"("+ sdr["ModeType"].ToString() + ")"+" PAX:"+ sdr["PAX"].ToString(),
                            //----------------------------------------------------------
                            ITEM= sdr["item_desc"].ToString(),
                            QTY= sdr["QTY"].ToString(),
                            RATE= sdr["Org_Rate"].ToString(),
                            VALUE= sdr["Org_Rate"].ToString(),
                            //----------------------------------------------------------
                            GrossAmt = sdr["Gross_Amount"].ToString(),
                            TAXAmt= sdr["Tax_Amount"].ToString(),
                            NetAmt= sdr["Net_Amount"].ToString(),
                            //----------------------------------------------------------
                            TAX_per= sdr["TAX1"].ToString(),
                            Taxable_Amt= sdr["INV_Amount_WO_TAX"].ToString(),
                            TAX_amt= sdr["Total_TAXAmount_1"].ToString(),
                            Total= sdr["Net_Amount"].ToString()
                        }).ToList();

            return Json(bill_data, JsonRequestBehavior.AllowGet);

        }
        //------------------------------------------------------------------------------------
        //---------------------img prnt-------------------------------------------------------
        public void copy_paste()
        {
            //var fn = Session["billImage_path"].ToString(); Jay Shah 22-5-24
            var fn = Request.Cookies["billImage_path"]?.Value;

            var currentPath = Path.GetFullPath(fn);
            currentPath = Directory.GetParent(currentPath).FullName;

            string imgdes= Server.MapPath("~/TempCreatedImages");
            //string sourceDir = @"c:\current";
            string sourceDir = @currentPath;
            string backupDir = @imgdes;

           // Session["pathdetails"] = "path:  " + sourceDir + "    ,backuppath" + backupDir; Jay Shah 22-5-24
            string pathdetails = "path: " + sourceDir + ", backuppath: " + backupDir;
            Response.Cookies["pathdetails"].Value = pathdetails;

            try
            {
                string[] picList = Directory.GetFiles(sourceDir, "*.jpg");
                //string[] txtList = Directory.GetFiles(sourceDir, "*.txt");

                // Copy picture files.
                foreach (string f in picList)
                {
                    // Remove path from the file name.
                    string fName = f.Substring(sourceDir.Length + 1);
                    //Session["Image_file"] = fName; Jay Shah 22-5-24
                    Response.Cookies["Image_file"].Value = fName;

                    // Use the Path.Combine method to safely append the file name to the path.
                    // Will overwrite if the destination file already exists.
                    System.IO.File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, fName), true);
                }

                // Copy text files.
                //foreach (string f in txtList)
                //{

                //    // Remove path from the file name.
                //    string fName = f.Substring(sourceDir.Length + 1);

                //    try
                //    {
                //        // Will not overwrite if the destination file already exists.
                //        File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, fName));
                //    }

                //    // Catch exception if the file was already copied.
                //    catch (IOException copyError)
                //    {
                //        Console.WriteLine(copyError.Message);
                //    }
                //}

                // Delete source files that were copied.
                //foreach (string f in txtList)
                //{
                //    File.Delete(f);
                //}
                foreach (string f in picList)
                {
                    System.IO.File.Delete(f);
                }
            }

            catch (DirectoryNotFoundException dirNotFound)
            {
                //Session["pathdetails"] = "path:  "+sourceDir +"    ,backuppath"+  backupDir +"    ,message:"+ dirNotFound.Message; Jay Shah 22-5-24
                pathdetails = "path: " + sourceDir + ", backuppath: " + backupDir + ", message: " + dirNotFound.Message;
                Response.Cookies["pathdetails"].Value = pathdetails;


            }
        }
        //string file;
        //public static Image ConvertToImage(Binary iBinary)
        //{
        //    var arrayBinary = iBinary.ToArray();
        //    Image rImage = null;

        //    using (MemoryStream ms = new MemoryStream(arrayBinary))
        //    {
        //        rImage = Image.FromStream(ms);
        //    }
        //    return rImage;
        //}

        public ActionResult imgRecipt()
 
        {
            //Session["Image_file"] = file;
            //copy_paste();
            //Binary b;
            //ConvertToImage();

            TagBuilder div = new TagBuilder("div");
            div.Attributes["id"] = "Acv";

            TagBuilder img = new TagBuilder("img");
            img.Attributes["alt"] = "Site Logo";
            img.Attributes["id"] = "mainimg";
            //img.Attributes["style"] = "max-width: 270%; max - height: 100 %; display: block; float:left;margin:0px;";
            img.Attributes["style"] = "max-width: 100%; max - height: 100 %; display: block; float:left;margin:0px;";
            //img.Attributes["style"] = "width:100%;";
            //img.Attributes["src"]= "~/TempCreatedImages/"+Session["Image_file"].ToString();
            //string url = Url.Content("~/TempCreatedImages/" + Session["Image_file"].ToString());
            //string url = Url.Content("~/TempCreatedImages/INV_00000034.jpg");
            //string url = Session["billImage_path"].ToString();
            //string url = Session["billImage_path"].ToString(); Jay Shah 22-5-24
            string url = Request.Cookies["billImage_path"]?.Value;

            byte[] photo = System.IO.File.ReadAllBytes(url);
            string base64 = Convert.ToBase64String(photo);
            string imgSrc = String.Format("data:image/jpg;base64,{0}", base64);
            img.MergeAttribute("src", imgSrc);
            div.InnerHtml = img.ToString();
            string FinalDataStr = div.ToString();
            //Session["htmlstring"] = new HtmlString(FinalDataStr); Jay Shah 22-5-24
            string htmlString = FinalDataStr; 
            Response.Cookies["htmlstring"].Value = htmlString;


            ViewBag.Return = Url.Action("Menu", "RTC");
            return View();
        }      

        public JsonResult SetimgReceipt(string id)
        {
            if (IsAuthoriseForJson() == false)
            {
                return Json(null);
            }
            MyConnection.Main(null);
            //Session["bprntID"] = id; Jay Shah 22-5-24
            Response.Cookies["bprntID"].Value = id;

            try
            {
                return Json(new { redirectUrl = Url.Action("Bill_Print", "Recipt"), isRedirect = true }, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX

                //return Json(null);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //MyConnection.Main(null);
            //try
            //{
            //    DBServer = MyConnection.DBServer;
            //    DBName = MyConnection.DBName;
            //    DBuser = MyConnection.DBuser;
            //    ThinkHPTLM.ThinkHPTLMTrans X_Proc_Save = new ThinkHPTLM.ThinkHPTLMTrans(true, "Administrator", "", "MSSQL", DBName, DBServer, DBuser);
            //    bool X_RR;

            //    string X_Return_Message = "";
            //    //string XXX_Invoice_Receipt_Image_File = "";
            //    db = MyConnection._db;
            //    string fp = Session["FingerPrint"].ToString();
            //    short StationID = Convert.ToInt16(db.fI_StationMasters.Where(x => x.StationName == fp).Select(x => x.StationID).FirstOrDefault());
            //    string XXX_Invoice_Receipt_Image_File = "";
            //    string myCurr = Server.MapPath("~/TempCreatedImages");

            //    X_RR = X_Proc_Save.M_Proc_Print_Invoice_IMAGE(System.Convert.ToString(id).ToString(), "Design", ref XXX_Invoice_Receipt_Image_File, StationID, ref X_Return_Message);
            //    Session["billImage_path"] = XXX_Invoice_Receipt_Image_File;

            //    if (X_RR == false)
            //    {
            //        if (X_Return_Message.Trim() != "")
            //        {
            //            return Json(new { Altmsg = X_Return_Message.Trim() }, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX
            //        }
            //        ///MsgBoxThinkSoft xx = new MsgBoxThinkSoft(M_MsgBox_Title, X_Return_Message, "");
            //    }
            //    return Json(new { redirectUrl = Url.Action("imgRecipt", "Recipt"), isRedirect = true }, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX

            //    //return Json(null);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        //---------------------img prnt-------------------------------------------------------

        public ActionResult Recipt() 
        {
            return View();
        }
#pragma warning disable CS0246 // The type or namespace name 'JsonResult' could not be found (are you missing a using directive or an assembly reference?)
        public JsonResult SetReceipt(string id)
#pragma warning restore CS0246 // The type or namespace name 'JsonResult' could not be found (are you missing a using directive or an assembly reference?)
        {
            if (IsAuthoriseForJson() == false)
            {
                return Json(null);
            }
            MyConnection.Main(null);
            try
            {
                if (Request.Cookies["cash_type"].ToString()=="IN") // changed from Session["cash_type"] Jay Shah 22-5-24
                {
                    conn = MyConnection.conn;
                    conn.Open();

                    string str = "select b.Gross_Amount,b.ref_date,b.gross_Amount,b.Discount_Amount,b.Tax_Amount,b.Net_Amount from fi_giisftr a,fi_giishead b where a.ref_no=b.ref_no and a.ref_no =" + id;

                    SqlCommand command = new SqlCommand(str, conn);

                    SqlDataReader reader = command.ExecuteReader();

                   /* Session["RefNo"] = id;

                    Session["TableId"].ToString();
                    Session["Mode"].ToString();*/ // Jay Shah 22-5-24
                    Response.Cookies["RefNo"].Value = id;
                    Response.Cookies["TableId"].Value = TableId.ToString();
                    Response.Cookies["Mode"].Value = Mode.ToString();


                    while (reader.Read())
                    {
                        /*Session["Date"] = reader["REF_DATE"].ToString();
                        Session["Gross"] = reader["Gross_Amount"].ToString();
                        Session["Discount"] = reader["Discount_Amount"].ToString();
                        Session["VAT"] = reader["Tax_Amount"].ToString();
                        Session["Net"] = reader["Net_Amount"].ToString();*/ // Jay Shah 22-5-24
                        Response.Cookies["Date"].Value = reader["REF_DATE"].ToString();
                        Response.Cookies["Gross"].Value = reader["Gross_Amount"].ToString();
                        Response.Cookies["Discount"].Value = reader["Discount_Amount"].ToString();
                        Response.Cookies["VAT"].Value = reader["Tax_Amount"].ToString();
                        Response.Cookies["Net"].Value = reader["Net_Amount"].ToString();

                        //Session["Paid"] = 0.00;
                        //Session["Balance"] = reader["Net_Amount"].ToString();
                    }
                    reader.Close();
                    command.Dispose();
                    conn.Close();

                    return Json(new { redirectUrl = Url.Action("Recipt", "Recipt"), isRedirect = true }, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX

                    //return Json(null);

                }
                else
                {
                    return Json(new { Altmsg = "Please Cashier IN First....." }, JsonRequestBehavior.AllowGet);//PASS DATA TO AJAX

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public class FI_Payment {
            public string LOC_CODE { get; set; }
            public int Payment_ID { get; set; }
            public string PaymentName { get; set; }
            public DateTime Payment_Date { get; set; }
            public int Cashier_log_ID { get; set; }
            public int Invoice_ID { get; set; }
            public int Payment_Code { get; set; }
            public double FC_Amount_Tendered { get; set; }
            public float Currency_Conversion { get; set; }
            public float BC_Amount_Tendered { get; set; }
            public float BC_AmountPaid { get; set; }
            public int Record_ID_FI_Cashier_IN_OUT_Log { get; set; }
            public DateTime Entry_Date { get; set; }
        }
        public class FI_Payment_Method_Master
        {
            public int Code { get; set; }
            public string Name { get; set; }
            public int Conversion { get; set; }
        }
#pragma warning disable CS0246 // The type or namespace name 'JsonResult' could not be found (are you missing a using directive or an assembly reference?)
        public JsonResult fillPaymentmethod()
#pragma warning restore CS0246 // The type or namespace name 'JsonResult' could not be found (are you missing a using directive or an assembly reference?)
        {
            if (IsAuthoriseForJson() == false)
            {
                return Json(null);
            }
            string X_Str = "select Code,Name,Conversion from FI_Payment_Method_Master";

            conn.Open();
            SqlCommand cmd = new SqlCommand(X_Str, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();
            conn.Close();

            List<FI_Payment_Method_Master> pay_data = new List<FI_Payment_Method_Master>();
            pay_data = (from DataRow sdr in dt.Rows
                        select new FI_Payment_Method_Master()
                        {
                            Code = Convert.ToInt16(sdr["Code"].ToString()),
                            Name = sdr["Name"].ToString(),
                            Conversion = Convert.ToInt32(sdr["Conversion"].ToString()),
                        }).ToList();

            return Json(pay_data, JsonRequestBehavior.AllowGet);
        }
#pragma warning disable CS0246 // The type or namespace name 'JsonResult' could not be found (are you missing a using directive or an assembly reference?)
        public JsonResult Paid_Amount(string amt,string code)
#pragma warning restore CS0246 // The type or namespace name 'JsonResult' could not be found (are you missing a using directive or an assembly reference?)
        {
            if (IsAuthoriseForJson() == false)
            {
                return Json(null);
            }
            //string LOC_CODE= Session["LOC_CODE"].ToString(); Jay Shah 22-5-24
            string LOC_CODE = Request.Cookies["LOC_CODE"]?.Value;

            //int M_User_ID =Convert.ToInt32(Session["User_ID"].ToString()); Jay Shah 22-5-24
            int M_User_ID = 0;
            if (int.TryParse(Request.Cookies["User_ID"]?.Value, out int userId))
            {
                M_User_ID = userId;
            }
            int M_Record_ID_FI_Cashier_IN_OUT_Log=0;
            conn.Open();
            string str = "select Record_ID from FI_Cashier_IN_OUT_Log where Cashier_OUT_Amount is null and User_ID = " + M_User_ID;
            SqlCommand cmd = new SqlCommand(str, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    M_Record_ID_FI_Cashier_IN_OUT_Log = Convert.ToInt16(dr["Record_ID"].ToString());
                    //LOC_CODE = dr["loc_code"].ToString();
                }
            }
            dr.Close();
            cmd.Dispose();
            conn.Close();

            DataSet X_ds=new DataSet();
            string pXML;

            DataTable X_Table=new DataTable();
            X_Table.Columns.Add("LOC_CODE");
            X_Table.Columns.Add("Payment_ID");
            X_Table.Columns.Add("Payment_Date");
            X_Table.Columns.Add("Cashier_log_ID");
            X_Table.Columns.Add("Record_ID_FI_Cashier_IN_OUT_Log");
            X_Table.Columns.Add("Invoice_ID");
            X_Table.Columns.Add("Payment_Code");
            X_Table.Columns.Add("FC_Amount_Tendered");
            X_Table.Columns.Add("Currency_Conversion");
            X_Table.Columns.Add("BC_Amount_Tendered");
            X_Table.Columns.Add("BC_AmountPaid");
        
            DataRow X_Row = X_Table.NewRow();
            X_Row["LOC_CODE"] = LOC_CODE;
            X_Row["Payment_ID"] = 0;
            
            //now[to be taken cashier loing in date
            //X_R[w("Payment_Date") = X_Server_date
           // X_Row["Payment_Date"] = Session["Date"]; Jay Shah 22-4-25
            X_Row["Payment_Date"] = Request.Cookies["Date"]?.Value;

            X_Row["Cashier_log_ID"] = M_User_ID;
            X_Row["Record_ID_FI_Cashier_IN_OUT_Log"] = M_Record_ID_FI_Cashier_IN_OUT_Log;
           // X_Row["Invoice_ID"] = Session["RefNo"]; Jay Shah 22-5-24
            X_Row["Invoice_ID"] = Request.Cookies["RefNo"]?.Value;

            X_Row["Payment_Code"] = code;
            X_Row["FC_Amount_Tendered"] = amt;
            X_Row["Currency_Conversion"] = 1;
            X_Row["BC_Amount_Tendered"] = amt;
            X_Row["BC_AmountPaid"] = amt; //to be decied after calculation
            X_Table.Rows.Add(X_Row);
            X_ds.Tables.Add(X_Table);
            pXML = X_ds.GetXml();

            //ThinkHPTLM.ThinkHPTLMTrans X_Proc_Save = new ThinkHPTLM.ThinkHPTLMTrans(true, M_User_First_Name, M_User_PassWord, M_DB_TYPE, M_TMATS_DB_Path_For_access, M_TMATS_SQL_SERVER_NAME, M_SQL_User);
            DBServer = MyConnection.DBServer;
            DBName = MyConnection.DBName;
            DBuser = MyConnection.DBuser;
            ThinkHPTLM.ThinkHPTLMTrans X_Proc_Save = new ThinkHPTLM.ThinkHPTLMTrans(true, "", "", "SQL", DBName, DBServer, DBuser);

            string M_ComputerName = "";
            string X_Computer_IP = "";
            string X_MAC_ADDRESS = "";
            string X_Computer_User_Name = "";
            string X_Return_String = "";
            int X_Return_Payment_ID=0;
            string Action="";

            bool result = X_Proc_Save.M_Proc_Save_FI_Payment(ref Action, LOC_CODE, pXML, M_ComputerName, X_Computer_IP, X_MAC_ADDRESS, X_Computer_User_Name,ref X_Return_String,ref X_Return_Payment_ID);
            if (result)
            {
                if (X_Return_Payment_ID == 1)
                {
                    return Json(new { redirectUrl = Url.Action("Table", "RTC"), isRedirect = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string X_Str = "select LOC_CODE,Payment_ID";
                    X_Str = X_Str + ",Payment_Date,Cashier_log_ID";
                    X_Str = X_Str + ",Invoice_ID";
                    X_Str = X_Str + ",Payment_Code";
                    X_Str = X_Str + ",(select name from FI_Payment_Method_Master";
                    X_Str = X_Str + " where Code=a.Payment_Code) as [PaymentName] ";
                    X_Str = X_Str + ",FC_Amount_Tendered,Currency_Conversion";
                    X_Str = X_Str + ",BC_Amount_Tendered,BC_AmountPaid";
                    X_Str = X_Str + " from FI_Payment a where Invoice_ID =" + Request.Cookies["RefNo"]?.Value;
                    X_Str = X_Str + " order by Payment_ID";

                    conn.Open();
                    cmd = new SqlCommand(X_Str, conn);
                    dr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    dr.Close();
                    cmd.Dispose();
                    conn.Close();

                    List<FI_Payment> pay_data = new List<FI_Payment>();
                    pay_data = (from DataRow sdr in dt.Rows
                                select new FI_Payment()
                                {
                                    FC_Amount_Tendered = Convert.ToDouble(sdr["FC_Amount_Tendered"].ToString()),
                                    PaymentName=sdr["PaymentName"].ToString(),
                                    Payment_ID = Convert.ToInt32(sdr["Payment_ID"].ToString())
                                }).ToList();
                    //Session["Paid"] = pay_data.Sum(x => x.FC_Amount_Tendered).ToString(); Jay Shah 22-5-24
                    double sum = pay_data.Sum(x => x.FC_Amount_Tendered);
                    Response.Cookies["Paid"].Value = sum.ToString();


                    /*double paid = Convert.ToDouble(Session["Paid"].ToString());
                    double net = Convert.ToDouble(Session["Net"].ToString());

                    double pending_amt = net-paid;
                    Session["Balance"] = pending_amt;*/
                    double paid = Convert.ToDouble(Request.Cookies["Paid"]?.Value ?? "0");
                    double net = Convert.ToDouble(Request.Cookies["Net"]?.Value ?? "0");

                    double pending_amt = net - paid;
                    Response.Cookies["Balance"].Value = pending_amt.ToString();


                    return Json(new { Pay_data=pay_data, Paid= paid,Net=net, Pending_amt= pending_amt }, JsonRequestBehavior.AllowGet);
                }
            }
            
            return Json(null);
        }
#pragma warning disable CS0246 // The type or namespace name 'JsonResult' could not be found (are you missing a using directive or an assembly reference?)
        public JsonResult remove_payment(string pid)
#pragma warning restore CS0246 // The type or namespace name 'JsonResult' could not be found (are you missing a using directive or an assembly reference?)
        {
            if (IsAuthoriseForJson() == false)
            {
                return Json(null);
            }
            conn.Open();
            string str = "delete from FI_Payment where Payment_ID="+pid;
            SqlCommand cmd = new SqlCommand(str,conn);
            var i = Convert.ToInt32(cmd.ExecuteNonQuery());
            cmd.Dispose();
            if (i < 1)
            {
                return Json(new { ismsg=true,msg = "Unsucess Full !!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string X_Str = "select LOC_CODE,Payment_ID";
                X_Str = X_Str + ",Payment_Date,Cashier_log_ID";
                X_Str = X_Str + ",Invoice_ID";
                X_Str = X_Str + ",Payment_Code";
                X_Str = X_Str + ",(select name from FI_Payment_Method_Master";
                X_Str = X_Str + " where Code=a.Payment_Code) as [PaymentName] ";
                X_Str = X_Str + ",FC_Amount_Tendered,Currency_Conversion";
                X_Str = X_Str + ",BC_Amount_Tendered,BC_AmountPaid";
                X_Str = X_Str + " from FI_Payment a where Invoice_ID =" + Request.Cookies["RefNo"]?.Value;
                X_Str = X_Str + " order by Payment_ID";

                cmd = new SqlCommand(X_Str, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();
                conn.Close();

                List<FI_Payment> pay_data = new List<FI_Payment>();
                pay_data = (from DataRow sdr in dt.Rows
                            select new FI_Payment()
                            {
                                FC_Amount_Tendered = Convert.ToDouble(sdr["FC_Amount_Tendered"].ToString()),
                                PaymentName = sdr["PaymentName"].ToString(),
                                Payment_ID = Convert.ToInt32(sdr["Payment_ID"].ToString())
                            }).ToList();

                /*Session["Paid"] = pay_data.Sum(x => x.FC_Amount_Tendered).ToString();

                double paid = Convert.ToDouble(Session["Paid"].ToString());
                double net = Convert.ToDouble(Session["Net"].ToString());

                double pending_amt = net - paid;
                Session["Balance"] = pending_amt;*/ // Jay Shah 22-5-24
                // Storing sum of pay_data in a cookie named "Paid"
                double sumPaid = pay_data.Sum(x => x.FC_Amount_Tendered);
                Response.Cookies["Paid"].Value = sumPaid.ToString();

                // Retrieving values from cookies and calculating pending amount
                double paid = double.TryParse(Request.Cookies["Paid"]?.Value, out double paidValue) ? paidValue : 0;
                double net = double.TryParse(Request.Cookies["Net"]?.Value, out double netValue) ? netValue : 0;
                double pending_amt = net - paid;

                // Storing pending amount in a cookie named "Balance"
                Response.Cookies["Balance"].Value = pending_amt.ToString();

                //return Json(pay_data, JsonRequestBehavior.AllowGet);
                return Json(new { Pay_data = pay_data, Paid = paid, Net = net, Pending_amt = pending_amt }, JsonRequestBehavior.AllowGet);
            }
        }
#pragma warning disable CS0246 // The type or namespace name 'JsonResult' could not be found (are you missing a using directive or an assembly reference?)
        public JsonResult get_pay_data()
#pragma warning restore CS0246 // The type or namespace name 'JsonResult' could not be found (are you missing a using directive or an assembly reference?)
        {
            if (IsAuthoriseForJson() == false)
            {
                return Json(null);
            }
            string X_Str = "select LOC_CODE,Payment_ID";
            X_Str = X_Str + ",Payment_Date,Cashier_log_ID";
            X_Str = X_Str + ",Invoice_ID";
            X_Str = X_Str + ",Payment_Code";
            X_Str = X_Str + ",(select name from FI_Payment_Method_Master";
            X_Str = X_Str + " where Code=a.Payment_Code) as [PaymentName] ";
            X_Str = X_Str + ",FC_Amount_Tendered,Currency_Conversion";
            X_Str = X_Str + ",BC_Amount_Tendered,BC_AmountPaid";
            X_Str = X_Str + " from FI_Payment a where Invoice_ID =" + Request.Cookies["RefNo"]?.Value;
            X_Str = X_Str + " order by Payment_ID";

            conn.Open();
            SqlCommand cmd = new SqlCommand(X_Str, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();
            conn.Close();

            List<FI_Payment> pay_data = new List<FI_Payment>();
            pay_data = (from DataRow sdr in dt.Rows
                        select new FI_Payment()
                        {
                            FC_Amount_Tendered =Convert.ToDouble(sdr["FC_Amount_Tendered"].ToString()),
                            PaymentName = sdr["PaymentName"].ToString(),
                            Payment_ID = Convert.ToInt32(sdr["Payment_ID"].ToString())
                        }).ToList();

            /* Session["Paid"] = pay_data.Sum(x => x.FC_Amount_Tendered).ToString();
              if (Session["Paid"].ToString()=="0")
              {
                  Session["Paid"] = 0.00;
                  Session["Balance"] = Session["Net"].ToString();
              }
              else
              {
                  double paid = Convert.ToDouble(Session["Paid"].ToString());
                  double net = Convert.ToDouble(Session["Net"].ToString());

                  double pending_amt = net - paid;
                  Session["Balance"] = pending_amt;
              }*/ // Jay shah 22-5-24
                  // Storing sum of pay_data in a cookie named "Paid"
            double sumPaid = pay_data.Sum(x => x.FC_Amount_Tendered);
            Response.Cookies["Paid"].Value = sumPaid.ToString();

            // Check if Paid is zero
            if (Request.Cookies["Paid"]?.Value == "0")
            {
                // Set Paid to 0.00
                Response.Cookies["Paid"].Value = "0.00";
                // Set Balance to Net
                Response.Cookies["Balance"].Value = Request.Cookies["Net"]?.Value;
            }
            else
            {
                // Retrieve values from cookies
                double paid = double.TryParse(Request.Cookies["Paid"]?.Value, out double paidValue) ? paidValue : 0;
                double net = double.TryParse(Request.Cookies["Net"]?.Value, out double netValue) ? netValue : 0;
                // Calculate pending amount
                double pendingAmt = net - paid;
                // Store pending amount in a cookie named "Balance"
                Response.Cookies["Balance"].Value = pendingAmt.ToString();
            }


            return Json(pay_data, JsonRequestBehavior.AllowGet);

        }
    }
}