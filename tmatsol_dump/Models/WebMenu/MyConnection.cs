using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    internal class MyConnection
    {
        public static RTC_DBContext _db;
        public static SqlConnection conn;
        public static string CompName;

        public static string Comp_Name;
        public static string DBServer;
        public static string DBName;
        public static string DBuser;
        public static string DBpass = "kajal3792kajal";


        //--------------------DLL-------------------------------
        public static string X_Local_Station = "true";
        public static string X_TMATS_User_Name = "web";
        public static string X_TMATS_User_Password = "";
        public static string X_Db_Type = "SQL";
        //public static string X_Db_Name = "TMATS00012016";
        //public static string X_Db_Servername=".";
        //public static string X_Db_UserName = "TMATS";
        public static string X_Db_Name;
        public static string X_Db_Servername;
        public static string X_Db_UserName;

        //--------------------DLL-------------------------------
        // public RTC_DBContext() : base(conn1.ConnectionString)
        public static void Main(string[] args)
        {
            try
            {
                if(HttpContext.Current.Request.Cookies["id"] != null)
                {
                    CompName = HttpContext.Current.Request.Cookies["id"].Value;
                }
                //comp = CompName;
                if (CompName == null)
                {
                    conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Report_DB_Entry"].ToString());
                    //conn = new SqlConnection("Data Source=.;Initial Catalog=TMATS00012016;User ID=tmats;Password=kajal3792kajal;Integrated Security=True");
                    _db = new RTC_DBContext(conn.ConnectionString);
                }
                else
                {

                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Report_DB_Entry"].ToString());
                    string str = "select * from Cust_Rec where Comp_Name='" + CompName + "'";
                    con.Open();
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Comp_Name = rdr["Comp_Name"].ToString();
                        DBServer = rdr["DBServer"].ToString();
                        DBName = rdr["DBName"].ToString();
                        DBuser = rdr["DBuser"].ToString();
                    }

                    X_Db_Name = DBName;
                    X_Db_Servername = DBServer;
                    X_Db_UserName = DBuser;



                    rdr.Close();    
                    con.Close();
                    conn = new SqlConnection("Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBpass + "");
                    _db = new RTC_DBContext(conn.ConnectionString);
                }
            }
            catch (Exception ex)
            {
                //var Error = ex.Message.ToString();
                //Session["ErrorMsg"] = Error;
                throw ex;
            }
        }
    }
}