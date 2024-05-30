using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_GIISFTR_SETTLED
    {
		[Key]
        public int REF_NO {get;set;}
        public string ITEM_CD {get;set;}
        public Double? Record_No { get; set; }
        public DateTime? FTR_REF_DATE { get; set; }
        public DateTime? FTR_REF_TIME { get; set; }
        public Double? QTY { get; set; }
        public string UNIT { get; set; }
        public Double? Rate { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string item_desc { get; set; }
		public string PARTY_CD { get; set; }
		public string Printed_Y_N { get; set; }
		public DateTime? Default_Entered_Date { get; set; }
		public string Shift_no { get; set; }
		public Double? KOT_Print_Seq_No { get; set; }
		public Double? Tax01_Per { get; set; }
		public Double? Tax02_Per { get; set; }
		public int? Order_Priority { get; set; }
        public string Gender { get; set; }
		public int? Chair { get; set; }
		public Double? Print_Seq_Number { get; set; }
		public int KOTNO { get; set; }
		public int StationID { get; set; }
		public string StationName { get; set; }
		public int userID { get; set; }
		public string userName { get; set; }
	    public int Service_Provider_ID { get; set; }
		public int ItemID { get; set; }
		public int ComboID { get; set; }
		public int? OrderType { get; set; }
	    public int Record_ID_FI_Cashier_IN_OUT_Log { get; set; }
		public string LOC_CODE { get; set; }
     	public Double? Org_Rate { get; set; }
	    public string IMEI1 { get; set; }
		public string IMEI2 { get; set; }
		public string TMATS_REF_NO { get; set; }
	 	public decimal? DISCOUNT_FACTOR { get; set; }
	    public Double? Comm_Per { get; set; }
	    public string Item_Return_Remark { get; set; }
		public string unique_ID { get; set; }
		public decimal TAX1 { get; set; }
		public decimal TAX2{ get; set; }
	    public string TAX_Price_Mode { get; set; }
		public decimal Rate_WOT { get; set; }
	    public decimal TAX1_RAmt { get; set; }
	    public decimal TAX2_RAmt { get; set; }
		public decimal Rate_WTT { get; set; }
		public decimal MC_PER { get; set; }
		public decimal SC_PER { get; set; }
		public decimal MC_AMT { get; set; }
		public decimal SC_AMT { get; set; }
		public int? IS_ReOpen { get; set; }
		public string Disc_Remark_D { get; set; }
	    public string Name_Display { get; set; }
	    public int Package_ID { get; set; }
	}
}