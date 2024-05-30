using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Table_Cart_V2k20.Models;
using ThinkSoftXt;
using Table_Cart_V2k20.Controllers;
using System.Data.SqlClient;

namespace Table_Cart_V2k20.Models
{
    public static class Task
    {


        //change at 28/11/2022
        //public static Boolean isDemo =true;
        //public static Boolean isLimitOver = true;
        //public static Boolean isLimitOver = true;
        //public static Boolean isDemo = false;
        //public static Boolean isLimitOver = false;

        public static string Stest = "";

        //public static Boolean isDemo = false;
        //public static Boolean isLimitOver = false;

        public static int MaXCount = 100;
        //public static int Counter = Convert.ToInt32(HttpContext.Current.Session["Session_Counter"]);
        //public static int Counter = MaXCount ;
        //--------------------------14 AUG 2021-----------------------------
        public static void Is_Avail_FI_StationMaster(string FP)
        {
            RTC_DBContext _db = MyConnection._db;
            try
            {               
                var Exists = _db.fI_StationMasters.Where(x => x.StationName == FP).Select(x => x.StationName).FirstOrDefault();
                if (Exists == null)
                {
                    var MaxID = _db.fI_StationMasters.Where(x => x.StationID > 999).Select(x => x.StationID).DefaultIfEmpty(0).Max();
                    int newid;
                    if (MaxID == 0)
                    {
                        newid = 1000;
                    }
                    else
                    {
                        newid = MaxID + 1;
                    }
                    var con = MyConnection.conn;
                    string str = "INSERT INTO FI_StationMaster(StationID,StationName,ComputerName) VALUES('"+newid+"','"+FP+"','ANDROID')";
                    con.Open();
                    SqlCommand cmd = new SqlCommand(str,con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    con.Close();
                    // FI_StationMaster obj = new FI_StationMaster()
                    
                    //obj.StationID = Convert.ToInt32(newid);
                    //obj.StationName = FP;
                    //obj.ComputerName = "Android";
                    ////   //------------------------------
                    //obj.ReceiptPrinterType = "";
                    //obj.ReceiptPrinterName = "";
                    //obj.CashDrawerAttached = "";
                    //obj.ReportPrinterName = "";
                    //obj.BarPrinterName = "";
                    //obj.PoleDisplayCommPort = "";
                    //obj.PoleDisplayWelcomeMessage1 = "";
                    //obj.PoleDisplayWelcomeMessage2 = "";
                    //obj.CallerIDCommPort = "";
                    //obj.Entry_Date_Time = null;
                    //obj.Exit_Date_Time = null;
                    //obj.BillPrinterName = "";
                    //obj.Group_Digit_Format = "";
                    //obj.PoleDisplayCommPort_Setting = "";
                    //obj.Counter_PrintCopy = null;
                    //obj.Delivery_PrintCopy = null;
                    //obj.Counter_PrintCopy_RECEIPT = null;
                    //obj.Delivery_PrintCopy_RECEIPT = null;
                    //obj.Dine_IN_PrintCopy_RECEIPT = null;
                    //obj.Numbers_Of_Counter = null;
                    //obj.POS_Updated_Version_DateTime = null;
                    //obj.Bill_Print_Format = "";
                    //obj.Receipt_Print_Format = "";
                    //obj.Port_No = null;
                    //obj.SERVER_IP = "";
                    //obj.SERVER_PORT = null;
                    //obj.StationType = "";
                    //obj.PLU_ITEM_CD_START = null;
                    //obj.PLU_ITEM_CD_LENGTH = null;
                    //obj.PLU_AMOUNT_START = null;
                    //obj.PLU_AMOUNT_LENGTH = null;
                    //obj.Default_PassWord_Screen = "";
                    //obj.DineIN_LogOff = "";
                    //obj.KOT_DISPLAY_REQUIRED = "";
                    //obj.KOT_DISPLAY_REQUIRED_AT = "";
                    //obj.KOT_FOOD_ACCEPTANCE_REQUIRED = "";
                    //obj.MESS_MBR = null;
                    //obj.MESS_RPB = null;
                    //obj.WS_Auto_Com_Port = "";
                    //obj.WS_Auto_Com_String = "";
                    //obj.WS_Auto_Received_String = "";
                    //obj.WS_Brand_Name = "";
                    //obj.StockDisplay_BO = "";
                    //obj.Additional_Billing_Printer_COUNTER = "";
                    //obj.Additional_Billing_Printer_DELIVERY = "";
                    //obj.Code_Req_With_Name = "";
                    //obj.Last_Selected_Language = null;
                    //obj.KOT_Monitor_Print_Req = "";
                    //------------------------------
                    //_db.fI_StationMasters.Add(obj);
                    //_db.SaveChanges();
                }
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //--------------------------14 AUG 2021-----------------------------
        public static void IsExistFi_Other(string FP)
        {
            try
            {
                //RTC_DBContext _db = new RTC_DBContext();
                RTC_DBContext _db = MyConnection._db;
                var Enc_FP = Cryptography.EncData(FP);
                
                var Exist = _db.Fi_Others.Where(x => x.fp == Enc_FP).ToList();
                if (Exist.Count > 0)
                {
                    var getdata = _db.Fi_Others.Where(x => x.fp == Enc_FP).Select(x=>x.ref_no).FirstOrDefault();
                    HttpContext.Current.Session["Session_Counter"] = Cryptography.DecData(getdata);
                }
                else
                {
                    var EncFP= Cryptography.EncData(FP);
                    Fi_others obj = new Fi_others();
                    obj.fp = EncFP;
                    
                    obj.ref_no = Cryptography.EncData(1.ToString());
                    HttpContext.Current.Session["Session_Counter"] = 1;
                    _db.Fi_Others.Add(obj);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }    
        }
       
        public static void DeviceReg(String fp, String im, string os, string osv, string br)
        {
            // RTC_DBContext _db = new RTC_DBContext();
            RTC_DBContext _db = MyConnection._db;
            var Exist = _db.Device_Reg_Msts.Where(x => x.UniqueID == fp).Select(x => x.UniqueID).FirstOrDefault();

            if (Exist == null)
            {
                InsertDevice(fp, im, os, osv, br);
                return;
            }

            if (fp != Exist.ToString())
            {

                InsertDevice(fp, im, os, osv, br);

                //Device_Reg_Mst obj = new Device_Reg_Mst();
                //obj.UniqueID = fp;
                //obj.IsMobile = im;
                //obj.OS = os;
                //obj.OS_Version = osv;
                //obj.Browser = br;
                //obj.Start_date = DateTime.Today;

                //_db.Device_Reg_Msts.Add(obj);
                //_db.SaveChanges();
            }
        }

        private static void InsertDevice(String fp, String im, string os, string osv, string br)
        {
            //RTC_DBContext _db = new RTC_DBContext();
            RTC_DBContext _db = MyConnection._db;
            Device_Reg_Mst obj = new Device_Reg_Mst();
            obj.UniqueID = fp;
            obj.IsMobile = im;
            obj.OS = os;
            obj.OS_Version = osv;
            obj.Browser = br;
            obj.Start_date = DateTime.Today;

            _db.Device_Reg_Msts.Add(obj);
            _db.SaveChanges();

        }

        public static Boolean IsDeviceReg(String fp, ref Boolean xisLimitOver)
     // public static void IsDeviceReg(String fp, int counter)
        {
           // Counter = counter;
  
            String GenLic = GetLicAgainstFP(fp);
            //  string str= " select * from our table where fp=fp and licenceKey=GenLic
            //RTC_DBContext _db = new RTC_DBContext();
            RTC_DBContext _db = MyConnection._db;
            var IsAvai = _db.Device_Reg_Msts.Where(x => x.UniqueID == fp && x.LicenseKey == GenLic).ToList();

           

            if (IsAvai.Count>0 )
            {
                //isDemo = false;
                //isLimitOver = false;
                return false;
            }
            else
            {
                //change at 28/11/2022
                //isDemo = true;
               
                //wait do not do anything

                //  isLimitOver = IsLimitOver(counter);
                int Counter = Convert.ToInt32(HttpContext.Current.Session["Session_Counter"]);
                xisLimitOver = IsLimitOver(Counter);
                return true;
            }
          
        }
        //--28-11-2022---origanal
        //public static void IsDeviceReg(String fp)
        //// public static void IsDeviceReg(String fp, int counter)
        //{
        //    // Counter = counter;

        //    String GenLic = GetLicAgainstFP(fp);
        //    //  string str= " select * from our table where fp=fp and licenceKey=GenLic
        //    //RTC_DBContext _db = new RTC_DBContext();
        //    RTC_DBContext _db = MyConnection._db;
        //    var IsAvai = _db.Device_Reg_Msts.Where(x => x.UniqueID == fp && x.LicenseKey == GenLic).ToList();



        //    if (IsAvai.Count > 0)
        //    {
        //        isDemo = false;
        //        isLimitOver = false;
        //    }
        //    else
        //    {
        //        //change at 28/11/2022
        //        //isDemo = true;
        //        isDemo = false;
        //        //  isLimitOver = IsLimitOver(counter);
        //        Counter = Convert.ToInt32(HttpContext.Current.Session["Session_Counter"]);
        //        isLimitOver = IsLimitOver(Counter);
        //    }

        //}

        public static Boolean IsLimitOver(int counter)
        {
            if (counter > MaXCount)
            {
                return true;
            }
            return false;
        }
        //---28-11-2022 origanal 
        //public static Boolean IsLimitOver(int counter)
        //{           
        //    if (counter > MaXCount)
        //    {
        //        isLimitOver = true;
        //        return true;
        //    }
        //    isLimitOver = false;
        //    return false;
        //}


        public static string GetLicAgainstFP(string fp )
        {
            ThinkSofRef think=new ThinkSofRef();

            var myGetkey = think.EncryptINI(fp, "B57780122", "123456789");

            //string GenLic = "";
            ////'this is just for demo'
            //if(fp == "571504415")
            //{
            //    GenLic = "ABCDE";
            //}

            return myGetkey;
        }
        public static void InLogRec()
        {
            //RTC_DBContext _db = new RTC_DBContext();
            RTC_DBContext _db = MyConnection._db;
            DeviceLog_Rec obj1 = new DeviceLog_Rec();
           // if (HttpContext.Current.Session["FingerPrint"]==null)
           // {
                obj1.UniqueID = HttpContext.Current.Session["FingerPrint"].ToString();
            //}   
            
            obj1.InSessionLog = DateTime.Now;
            obj1.SessionLogId = Guid.NewGuid().ToString();

            //if (HttpContext.Current.Session["SessionLogId"]==null)
           // {
                HttpContext.Current.Session["SessionLogId"] = obj1.SessionLogId;
           // }       

            _db.DeviceLog_Recs.Add(obj1);
            _db.SaveChanges();
        }
        public static string dataSource = "";
        public static string initialcatalog = "";
        public static string userid = "";
       

       
    }
}