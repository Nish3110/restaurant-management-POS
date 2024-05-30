using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;


//data access liberary (DAL)
namespace tmatsol_dump.Models.Web_Report.DAL
{
    public class DAL
    {
        string XXServerName;
        string XXDbname;
        string XXCUSTID;

        public DAL() { }

        public DAL(string XServerName, String XDnname, String XCUSTID)
        {
            this.XXDbname = XDnname;
            this.XXServerName = XServerName;
            this.XXCUSTID = XCUSTID;
        }

        public void GetDbname( ref string XServerName, ref string XDbname,ref string XCUSTID)
        {
            XDbname = this.XXDbname;
            XServerName = this.XXServerName;
            XCUSTID = this.XXCUSTID;
        }


        public DataSet GetDataset(string SqlQeury,ref string XError)
        {
            DataSet Dt = new DataSet();          
            try
            {
                string result_message = "";
                string tGetdb="";
                string tGetServerName="";
                string tcustid = "";
                
                GetDbname( ref tGetServerName, ref tGetdb,ref tcustid);
                if (tGetdb == null)
                {
                    tGetdb = "";
                }     
                
                string names = System.Configuration.ConfigurationManager.ConnectionStrings.ToString();
                //DB_connectivity
                string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "";
                ServerDetails ser = new ServerDetails();

                ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

                //ser.setServerDetails(System.Web.HttpContext.Current.Session["CUSTID"].ToString(), ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
                //var a = new ThinkHPTLM.Reports(true, DBuser, DBPassword, DBtype, DBName, DBServer, "tmats", "kajal3792kajal");

                string constring = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;
                XError = XError + constring;
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    XError = XError + "Opened the connection";  
                    using (SqlCommand cmd = new SqlCommand(SqlQeury, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        XError = XError + "Command type entered";
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(Dt);
                            XError = XError + "Table data filled";
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception e)
            {
                XError = XError + e.Message;
            }
            return Dt;
        }

        public Boolean ExecuteQuery(string X_Sql, ref string result_message)
        {
            try
            {
                //ThinkSoftXt tTest = new ThinkSoftXt.ThinkSofRef();        
                string DBServer = "", DBName = "", DBuser = "", DBPassword = "", AccessPath = "", Loc_code = "", DBtype = "";
                
                ServerDetails ser = new ServerDetails();
                //ser.setServerDetails(System.Web.HttpContext.Current.Session["CUSTID"].ToString(), ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);
                ser.setServerDetails(ref DBServer, ref DBName, ref DBuser, ref DBPassword, ref AccessPath, ref Loc_code, ref DBtype, ref result_message);

                string constring = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;
                //string constring = @"Data Source=" + DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBuser + ";Password=" + DBPassword;
                SqlConnection con = new SqlConnection(constring);

                con.Open();
                    //using (SqlCommand cmd = new SqlCommand("insert into fi_Rlogin_records values('" + this.username + "','" + c + "','" + d + "','" + e + "','" + f + "','" + a + "','" + b + "','" + this.password + "','" + this.login_flag + "','" + public_ip + "')", con))
                    using (SqlCommand cmd = new SqlCommand(X_Sql, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        int result = cmd.ExecuteNonQuery();
                        con.Close();
                        if (result < 0)
                        {
                            result_message = "Not Submitted Data";
                            return false;
                        }
                        else
                        {
                            result_message = "Submitted Data";
                        }
                    }               
            }
            catch (Exception er)
            {
                //Transaction.Current.Rollback();
                result_message = "Error: " + er.Message;
                return false;
            }
            return true;
        }
    }
}