using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace tmatsol_dump.Controllers.Rest
{   
    public class LoginController : Controller
    {
        // GET: Login
        //[Route("Report/Rest/Login/Index")]
        public ActionResult Index()
        {
            return View();
        }
        public void ReAssing()
        {
            HttpCookie chk = Request.Cookies["User"];
            HttpCookie chk_id =Request.Cookies["id"];
            if (chk == null && chk_id == null)
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                /*Session.RemoveAll();
                Session.Abandon();*/ // Jay Shah 22-5-24
                foreach (string cookieName in Request.Cookies.AllKeys)
                {
                    Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-1);
                }

                return;
            }
            else if (chk_id != null)
            {
                string id = Request.Cookies["id"].Value;
               // Session["CUSTID"] = id; Jay Shah 22-5-24                 
                Response.Cookies["CUSTID"].Value = id;
                if (chk != null)
                {
                    id = Request.Cookies["id"].Value;
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
            //else if (chk != null)
            //{
            //    string id = Request.Cookies["id"].Value;
            //    string From_dt = Request.Cookies["From_dt"].Value;
            //    string To_dt = Request.Cookies["To_dt"].Value;
            //    string To_dtVal = Request.Cookies["To_dtVal"].Value;
            //    string DaysRemaining = Request.Cookies["DaysRemaining"].Value;
            //    string Comp_Name = Request.Cookies["Comp_Name"].Value;
            //    string User = Request.Cookies["User"].Value;
            //    string TABLE_GRP_CD = Request.Cookies["TABLE_GRP_CD"].Value;
            //    string TableId = Request.Cookies["TableId"].Value;

            //    Session["CUSTID"] = id;
            //    Session["From_dt"] = From_dt;
            //    Session["To_dt"] = To_dt;
            //    Session["To_dtVal"] = To_dtVal;
            //    Session["DaysRemaining"] = DaysRemaining;
            //    Session["Comp_Name"] = Comp_Name;
            //    Session["User"] = User;
            //    Session["TABLE_GRP_CD"] = TABLE_GRP_CD;
            //    if (Session["TableId"] == null)
            //    {
            //        Session["TableId"] = TableId;
            //    }
            //    else
            //    {
            //        Session["TableId"] = Session["TableId"];
            //    }

            //}
        }

        [HttpGet]
        public ActionResult Login()
        {
            ReAssing();
           /* if (Session["CUSTID"] != null)
            {
                if (Session["User"] != null)
                {
                    return RedirectToAction("Dashboard", "Report");
                }
                else
                {
                    return RedirectToAction("Index", "REPORT", new { ID = Session["CUSTID"].ToString() });
                }
            }*/ // Jay Shah 22-5-24
            if (Request.Cookies["CUSTID"] != null)
            {
                if (Request.Cookies["User"] != null)
                {
                    return RedirectToAction("Dashboard", "Report");
                }
                else
                {
                    return RedirectToAction("Index", "REPORT", new { ID = Request.Cookies["CUSTID"].Value });
                }
            }

            return View();
        }
        [HttpPost]
        public ActionResult Login(fi_usermast obj)
        {
            string str= "select TOP 1 Cust_ID from cust_rec where Login_ID = '" + obj.GetCustID + "' and login_PWD = '" + obj.Authorize_Code + "'";
            SqlConnection con = new SqlConnection("Data Source=54.36.166.189;Initial Catalog=tmatsol_Report_DB_Entry;User ID=tmatsreport;Password=iLu4j22#;");
            con.Open();
            SqlCommand cmd = new SqlCommand(str,con);
            var rst=cmd.ExecuteScalar();
            if (rst!=null)
            {
                cmd.Dispose();
                con.Close();
                return RedirectToAction("Index", "REPORT", new { ID=rst.ToString() } );
            }
            else
            {
                cmd.Dispose();
                con.Close();
                ViewBag.Err = "Invalid User Name or Password !!!";
                return View();
            }            
        }
    }
}