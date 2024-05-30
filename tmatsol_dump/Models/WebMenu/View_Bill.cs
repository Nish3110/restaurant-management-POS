using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class View_Bill
    {
        public string Entered_Name { get; set; }
        public string Name { get; set; } 
       public Double  Qty { get; set; }
       public Double  Rate { get; set; }
       public Double Amount { get; set; }
       public string  KOTNO { get; set; }
       public string  Punc_D_Time { get; set; }//DateTime
        public string Code { get; set; }        
        public int userID { get; set; }
        public string UserName { get; set; }
        public int StationID { get; set; }
        public string StationNAME { get; set; }
        public string BAR_CODE_NON_BAR_CODE { get; set; }
        public string SERVICE_PROVIDER_NAME { get; set; }
        public int SERVICE_PROVIDER_ID { get; set; }
        public string Service_Provider_Require { get; set; }
        public int ItemID { get; set; }
        public int StepNo { get; set; }
        public int ComboID { get; set; }
        public string IMEI_Req { get; set; }
        public string IMEI1_NO { get; set; }
        public string IMEI2_NO { get; set; }
        public string Item_Return_Remark { get; set; }
        public string Unique_ID { get; set; }
        public Double Rate_WOT { get; set; }
        public Double TAX1_RAmt { get; set; }
        public Double TAX2_RAmt { get; set; }
        public Double Rate_WTT { get; set; }
        public string TAX_Price_Mode { get; set; }
        public Double TAX1_Per { get; set; }
        public Double TAX2_Per { get; set; }
        public Double MC_Per { get; set; }
        public Double MC_AMT { get; set; }
        public Double SC_Per { get; set; }
        public Double SC_AMT { get; set; }
        public Double Gross_Amount { get; set; }
        public Double Discount_Amount { get; set; }
        public Double Tax_Amount { get; set; }
        public Double Net_Amount { get; set; }
    }
}