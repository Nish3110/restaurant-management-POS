using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using tmatsol_dump;
using tmatsol_dump.Models.Web_Report;

public class ServerDetails
{
    //public void setServerDetails(string xCust_ID,ref string DBServer, ref string DBName, ref string DBuser, ref string DBPassword, ref string AccessPath, ref string loc_code, ref string DBtype, ref string result_message)
    public void setServerDetails(ref string DBServer, ref string DBName, ref string DBuser, ref string DBPassword, ref string AccessPath, ref string loc_code, ref string DBtype, ref string result_message)
    {
        try
        {
            UserDATA data = new UserDATA();
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Report_DB_Entry"].ConnectionString);

            // con.ConnectionString = con.ConnectionString.Replace("XpasswordX", "kajal3792kajal");

            con.Open();
            string str = "";
            HttpCookie chk = HttpContext.Current.Request.Cookies["User"];
            if (chk == null)
            {
                str = "select * from Cust_Rec where Cust_ID = '" + System.Web.HttpContext.Current.Session["CUSTID"].ToString() + "' and To_dt >= '" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "' and Comp_Name='" + System.Web.HttpContext.Current.Session["Comp_Name"].ToString().Trim() + "' ";
            }
            else
            {
                str = "select * from Cust_Rec where Cust_ID = '" + HttpContext.Current.Request.Cookies["id"].Value + "' and To_dt >= '" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "' and Comp_Name='" + HttpContext.Current.Request.Cookies["Comp_Name"].Value + "' ";
            }

            //string str = "select * from Cust_Rec where Cust_ID = '" + xCust_ID + "' and To_dt >= '" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "' and Comp_Name='"+ System.Web.HttpContext.Current.Session["Comp_Name"].ToString() + "' ";
            //string str = "select * from Cust_Rec where Cust_ID = '" + +"' and To_dt >= '" + DateTime.Now + "' ";
            SqlCommand cmd = new SqlCommand(str, con);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            foreach (DataRow row in dt.Rows)
            {
                DBServer = row["DBServer"].ToString();
                DBName = row["DBName"].ToString();
                DBuser = row["DBuser"].ToString();
                DBtype = row["DBtype"].ToString();
                DBPassword = data.SQL_PWD;

                //added at 12/07/2022
                if (!DBNull.Value.Equals(row["PIP"]))
                {
                    if (row["PIP"].ToString() != "")
                    {
                        DBServer = row["PIP"].ToString();
                    }
                }
                //12/07/2022

                if (row["pwd_policy"].ToString() == "POLICY001")
                {
                    DBPassword = "Kajal@123";
                }
                if (row["pwd_policy"].ToString() == "POLICY002")
                {
                    DBPassword = "Trushachampak@123";
                }
                sda.Dispose();
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public void setServerDetails_EF_dcs(string Cust_ID, string Comp_Name, ref string DBServer, ref string DBName, ref string DBuser, ref string DBPassword, ref string AccessPath, ref string loc_code, ref string DBtype, ref string result_message)
    {
        try
        {
            UserDATA data = new UserDATA();
            _DBContext db = new _DBContext();

            DateTime date = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
            
            //string Cust_ID = "";
            //string Comp_Name = "";
           
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
    public void setServerDetails_EF(ref string DBServer, ref string DBName, ref string DBuser, ref string DBPassword, ref string AccessPath, ref string loc_code, ref string DBtype, ref string result_message)
    {
        try
        {
            UserDATA data = new UserDATA();
            _DBContext db = new _DBContext();

            DateTime date = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
            HttpCookie chk = HttpContext.Current.Request.Cookies["User"];

            string Cust_ID = "";
            string Comp_Name = "";
            if (chk == null)
            {
                Cust_ID = System.Web.HttpContext.Current.Session["CUSTID"].ToString();
                Comp_Name = System.Web.HttpContext.Current.Session["Comp_Name"].ToString().Trim();
                //str = "select * from Cust_Rec where Cust_ID = '" + System.Web.HttpContext.Current.Session["CUSTID"].ToString() + "' and To_dt >= '" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "' and Comp_Name='" + System.Web.HttpContext.Current.Session["Comp_Name"].ToString().Trim() + "' ";
            }
            else
            {
                Cust_ID = HttpContext.Current.Request.Cookies["id"].Value;
                Comp_Name = HttpContext.Current.Request.Cookies["Comp_Name"].Value;
                //str = "select * from Cust_Rec where Cust_ID = '" + HttpContext.Current.Request.Cookies["id"].Value + "' and To_dt >= '" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "' and Comp_Name='" + HttpContext.Current.Request.Cookies["Comp_Name"].Value + "' ";
            }
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
}