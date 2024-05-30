//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace tmatsol_dump.Controllers.BO
//{
//    public class BO_LoginController : Controller
//    {
//        // GET: BO_Login
//        public ActionResult Index()
//        {
//            return View();
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace tmatsol_dump.Controllers.bo
{
    public class BO_LoginController : Controller
    {
        // GET: BO_Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(fi_usermast obj)
        {
            string str = "select TOP 1 Cust_ID from cust_rec where Login_ID = '" + obj.GetCustID + "' and login_PWD = '" + obj.Authorize_Code + "'";
            SqlConnection con = new SqlConnection("Data Source=54.36.166.189;Initial Catalog=tmatsol_Report_DB_Entry;User ID=tmatsreport;Password=iLu4j22#;");
            con.Open();
            SqlCommand cmd = new SqlCommand(str, con);
            var rst = cmd.ExecuteScalar();
            if (rst != null)
            {                
                    cmd.Dispose();
                    con.Close();
                if (Request.IsLocal)
                {
                    return Redirect("https://localhost:44354/RT_Login/IndexLogin?id='" + rst + "'");
                }
                else
                {
                    return Redirect("http://bo.tmatsol.com/RT_Login/IndexLogin?id='" + rst + "'");
                }
                //return RedirectToAction("Index", "BO");
                //return RedirectToAction("Index", "BO", new { ID = rst.ToString() });
                //return RedirectToAction("Dashboard", "AMC");
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