using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Table_Cart_V2k20.Controllers;

namespace Table_Cart_V2k20.Models
{
    public class Combo_And_Modifier
    {

        private SqlConnection gcnn;
        public Combo_And_Modifier(SqlConnection Cnn)
        {
            gcnn = new SqlConnection(Cnn.ConnectionString+";password=kajal3792kajal");
            gcnn.Open();
        }

        public void Dispose()
        {
            gcnn.Close();
        }



        //-----------------Stock------------------------------
        //public string LOC_CODE(SqlConnection conn)
        public string LOC_CODE()
        {            
                //string str = "select  b.cl_stk,a.MIN_STK_LEVEL,a.MAX_STK_LEVEL  from itmmast a,tbl_stkitems b  where a.item_cd=b.item_cd  and  b.item_cd='"+id+"' and loc_code in (select Tmats_Loc_Code  from FI_POS_LOC_TO_TMATS_LOC_LINK  where Pos_Loc_Code='SHOP')";
                string str = "select LOC_CODE from FI_Company_MASTER";

                //SqlConnection con = new SqlConnection(conn);
                SqlCommand cmd;
            //conn.Open();
            //cmd = new SqlCommand(str, conn);
            cmd = new SqlCommand(str, gcnn);
            var i = (cmd.ExecuteScalar()).ToString();
                if (i != "" || i!=null)
                {
                    cmd.Dispose();
                    //conn.Close();
                    return i;
                }
                cmd.Dispose();
                //conn.Close();                        
            return i;
        }
        

        public int GetStock(string id,string loc_code)
        {
            //string loc_code = LOC_CODE(conn);
            int Res=0;
            if (id != "")
            {
                //string str = "select  b.cl_stk,a.MIN_STK_LEVEL,a.MAX_STK_LEVEL  from itmmast a,tbl_stkitems b  where a.item_cd=b.item_cd  and  b.item_cd='"+id+"' and loc_code in (select Tmats_Loc_Code  from FI_POS_LOC_TO_TMATS_LOC_LINK  where Pos_Loc_Code='SHOP')";
                string str = "select  b.cl_stk from itmmast a,tbl_stkitems b  where a.item_cd=b.item_cd  and  b.item_cd='" + id + "' and loc_code in (select Tmats_Loc_Code  from FI_POS_LOC_TO_TMATS_LOC_LINK  where Pos_Loc_Code='" + loc_code + "')";

                //SqlConnection con = new SqlConnection(conn);
                SqlCommand cmd;
                //conn.Open();
               
                cmd = new SqlCommand(str, gcnn);

                //var i = Convert.ToInt32(cmd.ExecuteScalar());
                var i = cmd.ExecuteScalar();
                cmd.Dispose();
                //conn.Close();
                if (i == null)
                {
                    //i = 0;
                }
                else
                {
                    Res = Convert.ToInt32(i);
                }
            }
            return Res;
            //return (int)i;
        }
        //-----------------Stock------------------------------

        //-----------------Modifier--------------------------
        public bool IsModifier(string id)
        {
            var numberString = id;
            int number;

            bool result=int.TryParse(numberString, out number);
            if (result)
            {
                string str = "SELECT COUNT(*) FROM FI_Combo_Items_Relation  WHERE ComboID = " +  id + "";
                //SqlConnection con = new SqlConnection(conn);
                SqlCommand cmd;
                //conn.Open();
                //cmd = new SqlCommand(str, conn);
                cmd = new SqlCommand(str, gcnn);
                var i = Convert.ToInt32(cmd.ExecuteScalar());
                if (i > 0)
                {
                    cmd.Dispose();
                    //conn.Close();

                    return true;
                }

                cmd.Dispose();
                //conn.Close();
                return false;
            }
            return false;
       
        }
        //-----------------Modifier--------------------------
        //-----------------Combo--------------------------
        public bool IsCombo()
        {
            return false;
        }
        //-----------------Combo--------------------------
        public int Maxqty(string id, int step, int qty)
        {
            string str = "SELECT MAX(Qty) FROM FI_Combo_Items_Relation  WHERE ComboID=" + id + "and Step_No='" + step + "'";
            //SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd;
            //conn.Open();
            //cmd = new SqlCommand(str, conn);
            cmd = new SqlCommand(str, gcnn);
            var i = Convert.ToInt32(cmd.ExecuteScalar());
            if (i > 0)
            {
                cmd.Dispose();
                //conn.Close();

                return i*qty;
            }

            cmd.Dispose();
            //conn.Close();
            return i;
        }
        public int CountSteps(string id)
        {
            string str = "SELECT MAX(STEP_NO) FROM FI_Combo_Items_Relation  WHERE ComboID=" + id + "";
            //SqlConnection con = new SqlConnection(conn);
            SqlCommand cmd;
            //conn.Open();
            cmd = new SqlCommand(str, gcnn);

            var i = Convert.ToInt32(cmd.ExecuteScalar());
            if (i > 0)
            {
                cmd.Dispose();
               //conn.Close();

                return i;
            }

            cmd.Dispose();
            //conn.Close();
            return i;
        }
        public DataTable StepBaseItems(string id,int i)
        {
            DataTable dt=new DataTable();
            string str = "SELECT a.ComboID,a.ItemID,a.Step_No,a.Qty,a.combo_type,a.Sale_Price,b.ITEM_DESC,b.ITEM_CD FROM FI_Combo_Items_Relation a,FI_Itmmast b where a.ItemID=b.ItemID and a.ComboID='"+id+"' and a.Step_No='"+i+"'";
            //SqlConnection con = new SqlConnection(conn);
            //conn.Open();
            SqlDataAdapter sda=new SqlDataAdapter(str,gcnn);
            sda.Fill(dt);
            //conn.Close();
            return dt;
        }
    }
}