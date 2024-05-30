using System;
using System.Data.Entity;
using tmatsol_dump.Models.WebMenu;


namespace Table_Cart_V2k20.Models
{
    public class RTC_DBContext : DbContext
    {
        //public RTC_DBContext ():base("conn")
        //{
        //    //this.Database.CommandTimeout = 999;
        //}
        //<add name = "conn" providerName="System.Data.SqlClient" connectionString=" Data Source=.;Initial Catalog=TMATS00012016;User ID=tmats;Password=kajal3792kajal;Integrated Security=True" />
        //public static SqlConnection conn1;
        //public static string Company;
        // public static SqlConnection conn1;// = new SqlConnection("Data Source=.;Initial Catalog=TMATS00012016;User ID=tmats;Password=kajal3792kajal;Integrated Security=True");
        // public static string CompName;

        //public static string Comp_Name;
        //public static string DBServer;
        //public static string DBName;
        //public static string DBuser;
        // // public RTC_DBContext() : base(conn1.ConnectionString)
        // public static void Main(string[] args)
        // {
        //     //comp = CompName;
        //     if (CompName == "")
        //     {
        //         conn1 = new SqlConnection("Data Source=.;Initial Catalog=TMATS00012016;User ID=tmats;Password=kajal3792kajal;Integrated Security=True");
        //     }
        //     else
        //     {
        //         SqlConnection con = new SqlConnection("Data Source=satrangfoods.dyndns.org;Initial Catalog=TMATS00012018;User ID=tmats;Password=kajal3792kajal;Integrated Security=True");
        //         string str = "select * from web_billing_entry where Comp_Name='"+CompName+"'";
        //         con.Open();
        //         SqlCommand cmd = new SqlCommand(str,con);
        //         SqlDataReader rdr = cmd.ExecuteReader();
        //         while (rdr.Read())
        //         {
        //             Comp_Name = rdr["Comp_Name"].ToString();
        //             DBServer =  rdr["DBServer"].ToString();
        //             DBName = rdr["DBName"].ToString();
        //             DBuser = rdr["DBuser"].ToString();
        //         }
        //         rdr.Close();
        //         con.Close();
        //     }
        // }
        public RTC_DBContext(String ConnectionString) : base(ConnectionString)
        {
            // base.Database.Connection.ConnectionString = "new connection string here";

            //this.Database.CommandTimeout = 999;
        }
        public DbSet<FI_Table_Master> fI_Table_Masters { get; set; }
        public DbSet<FI_Itmmast> FI_Itmmasts { get; set; }
        //public PagedList.IPagedList<FI_Itmmast> FI_Itmmasts { get; set; }
        public DbSet<FI_GRP_MASTER> fI_GRP_MASTERs { get; set; }
        public DbSet<FI_Sale_Invoice_Pend_From_Cart_Start> fI_Sale_Invoice_Pend_From_Cart_Starts { get; set; }
        public DbSet<FI_Sale_Invoice_Pend_From_Cart_End> fI_Sale_Invoice_Pend_From_Cart_Ends { get; set; }
        public DbSet<FI_Sale_Invoice_Pend_From_Cart_End_DONE> FI_Sale_Invoice_Pend_From_Cart_End_DONEs { get; set; }

        public DbSet<FI_Caller_Master> fI_Caller_Masters { get; set; }
        //public DbSet<FI_itmmast_Images> fi_itmmast_Images { get; set; }

        public DbSet<Device_Reg_Mst> Device_Reg_Msts { get; set; }
        public DbSet<DeviceLog_Rec> DeviceLog_Recs { get; set; }
        public DbSet<FI_GIISHEAD> fI_GIISHEADs { get; set; }
        public DbSet<FI_GIISHEAD_SETTLED> fI_GIISHEAD_SETTLEDs { get; set; }
        public DbSet<FI_GIISFTR_SETTLED> fI_GIISFTR_SETTLEDs { get; set; }
        public DbSet<FI_Payment_SETTLED> fI_Payment_SETTLEDs { get; set; }
        public DbSet<FI_GIISHEAD_IPOS> fI_GIISHEAD_Ipos { get; set; }
        public DbSet<FI_GIISFTR_IPOS> fI_GIISFTR_ipos { get; set; }
        public DbSet<FI_Payment_IPOS> fI_Payment_ipos { get; set; }
        public DbSet<FI_Web_BILL_Print_request> FI_Web_BILL_Print_requests { get; set; }
        public DbSet<Fi_others> Fi_Others { get; set; }
        public DbSet<FI_StationMaster> fI_StationMasters { get; set; }
        public DbSet<Curr_Deno_Mast> curr_Deno_Masts { get; set; }
        public DbSet<FI_USERMAST> fI_USERMASTs { get; set; }
        public DbSet<FI_User_Rights_Master> fI_User_Rights_Masters { get; set; }
        public DbSet<FI_TABLE_GROUP_MASTER> FI_TABLE_GROUP_MASTERs { get; set; }
        public DbSet<MODE_MASTER> MODE_MASTERs { get; set; }
        public DbSet<Mode_Master_Link> Master_Links { get; set; }
        public DbSet<MODE_Code_Link_fi_itmmast> MODE_Code_Link_Fi_Itmmasts { get; set; }
        public DbSet<FI_Driver_Master> fI_Driver_Masters { get; set; }
        public DbSet<FI_Payment_Method_Master> fI_Payment_Method_Masters { get; set; }
        public DbSet<FI_Staff_Master> fI_Staff_Masters { get; set; }
        public DbSet<FI_Service_Provider> fI_Service_Providers { get; set; }
        public DbSet<FI_Card_Type_Master> fI_Card_Type_Masters { get; set; }
        public DbSet<FI_Card_Master> fI_Card_Masters { get; set; }
        public DbSet<FI_Language_Master> fI_Language_Masters { get; set; }
        public DbSet<MODE_MASTER_Oth_lan> MODE_MASTER_Oth_Lans { get; set; }
        public DbSet<fi_rights_master> fi_Rights_Masters { get; set; }
        public DbSet<FI_Rights_Master_Oth_lan> fI_Rights_Master_Oths { get; set; }
        public DbSet<FI_Combo_Items_Relation> FI_Combo_Items_Relations { get; set; }
        public DbSet<FI_Company_MASTER> FI_Company_MASTERS { get; set; }

        //public virtual DbSet<FI_Itmmast> FI_Itmmast { get; set; }
        public virtual DbSet<FI_itmmast_Images> FI_itmmast_Images { get; set; }
    }
}