using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_GIISHEAD_SETTLED
    {
        [Key]
        public int REF_NO { get; set; }
        public DateTime? REF_DATE { get; set; }
        public string PARTY_CD { get; set; }
        public Double? Discount_Amount { get; set; }
        public Double? TOTAL_VALUE { get; set; }
        public Double? CLEARED_AMOUNT { get; set; }
        public string Type { get; set; }
        public string Sale_Person { get; set; }
        public string USER_NAME { get; set; }
        public string PAY_TYPE { get; set; }
        public string BuyerPo_Date { get; set; }
        public DateTime? Default_Entered_Date { get; set; }
        public string Item_Enter_Y_N { get; set; }
        public string Shift_no { get; set; }
        public DateTime? Ref_Time { get; set; }
        public Double? Ser_Tax_Per { get; set; }
        public Double? Ser_Tax_Amt { get; set; }
        public string BILL_PAD_NO { get; set; }
        public string Print_Invoice { get; set; }
        public Double? Print_Seq_No { get; set; }
        public string Party_Detail_Party_cd { get; set; }
        public int StationID { get; set; }
        public string StationName { get; set; }
        public int userID {get;set;}
        public string userName { get; set; }
        public int PAX { get; set; }
        public string D_C_P_REF_NO { get; set; }
        public string D_C_P_MODE { get; set; }
        public string LOC_CODE { get; set; }
        public decimal Gross_Amount { get; set; }
        public decimal Tax_Amount { get; set; }
        public decimal Net_Amount { get; set; }
        public int OrderType { get; set; }
        public int CustomerID { get; set; }
        public int Driver_ID { get; set; }
        public string Driver_Name { get; set; }
        public string Bill_Note { get; set; }
        public int Print_Seq_Number_Bill { get; set; }
        public string TMATS_REF_NO { get; set; }
        public string TMATS_REF_NO_SI_NO { get; set; }
        public string Disc_Per { get; set; }
        public decimal INV_Amount_WO_TAX { get; set; }
        public decimal INV_Amount_WT_TAX { get; set; }
        public decimal TAX1_AMOUNT { get; set; }
        public decimal TAX2_AMOUNT { get; set; }
        public string TRN { get; set; }
        public string ref_no_with_ps { get; set; }
        public string Disc_Remark_H { get; set; }
        //--------------------------------------
        public DateTime? Settle_DT { get; set; } 
    	public DateTime? Cashier_Settled_Date { get; set; }
        public int Record_ID_FI_Cashier_IN_OUT_Log { get; set; }
        public DateTime? Exported_Date { get; set; }
        public DateTime? Imported_Server_Date { get; set; }
        public int Table_Grp_cd { get; set; }
        public Double? DISCOUNT_FACTOR { get; set; }
        public int Mode_Code_Link { get; set; }
        public int Cashier_ID { get; set; }
        public string Cashier_Name { get; set; }
    }
}