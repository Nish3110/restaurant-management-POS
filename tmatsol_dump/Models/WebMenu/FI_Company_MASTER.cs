using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
	public class FI_Company_MASTER
	{
		[System.ComponentModel.DataAnnotations.Key]
		public string COMP_CD { get; set; }
		public string COMP_NAME { get; set; }
		public string COMP_NAME1 { get; set; }
		public string COMP_ADDL1 { get; set; }
		public string COMP_ADDL2 { get; set; }
		public string COMP_ADDL3 { get; set; }
		public string COMP_CITY { get; set; }
		public string COMP_STATE { get; set; }
		public string COMP_COUNTRY { get; set; }
		public string COMP_PHONE { get; set; }
		public string COMP_FAX { get; set; }
		public string COMP_EMAIL { get; set; }
		public string COMP_MOBILE { get; set; }
		public string LOC_CODE { get; set; }
		public string PreFix_GenerateFile { get; set; }
		public Nullable<DateTime> FROM_HELP_DATE { get; set; }
		public Nullable<DateTime> TO_HELP_DATE { get; set; }
		public double Form_Colour { get; set; }
		public double Decimal_Point { get; set; }
		public string Shift_Module_Y_N { get; set; }
		public double Fix_Discount_Per { get; set; }
		public string PO_BOX_NO { get; set; }
		public int Qty_Decimal { get; set; }
		public int Rate_Decimal { get; set; }
		public string Main_Button_Color_ARGB { get; set; }
		public string DateFormat { get; set; }
		public string comp_tag { get; set; }
		public int Decimals { get; set; }
		public string Code_Req_IN_Receipt { get; set; }
		public string Stock_Deduction_RealTime { get; set; }
		public string MCB { get; set; }
		public string RSDB_Type { get; set; }
		public string RMAccess_File { get; set; }
		public string RServer_Name { get; set; }
		public string RServer_PWD { get; set; }
		public string RSDB_Name { get; set; }
		public string KOT_REQUIRED { get; set; }
		public string Deno_REQUIRED { get; set; }
		public string Shop_Open_Time { get; set; }
		public string Shop_Close_Time { get; set; }
		public string Sp_Bosnia_Printing { get; set; }
		public string Duplicate_Printing_Allow { get; set; }
		public string CashierName_In_ReceiptP { get; set; }
		public string KOT_With_Code { get; set; }
		public string Bill_Receipt_Print_Style { get; set; }
		public int Item_Desc_Char_Length { get; set; }
		public int Item_Desc_Char_Length_KOT { get; set; }
		public string ZeroValueRequired { get; set; }
		public string Second_Language { get; set; }
		public string RServer_SQL_USER { get; set; }
		public string KOT_Combo_Name_Required { get; set; }
		public string CDSR { get; set; }
		public string Return_Remark_Y_N { get; set; }
		public string loc_name { get; set; }
		public string Type_OF_Business { get; set; }
		public string TAX_Country { get; set; }
		public int item_button_height { get; set; }
		public int item_button_width { get; set; }
		public int item_total_in_Frame { get; set; }
		public int item_per_row { get; set; }
		public int Group_button_height { get; set; }
		public string invoice_pre_fix { get; set; }
		public string invoice_sub_fix { get; set; }
		public int invoice_length { get; set; }
		public string Corporate_Rate_To_Default_Rate { get; set; }
		public string Negative_Stock_Allowed { get; set; }
		public string POP_UP_Required_For_Min_Stk_Lev { get; set; }
		public string Tax_Option_Required { get; set; }
		public string KOT_Print_OnTheSpot { get; set; }
		public string Gmail_ID { get; set; }
		public string Cloud_Server_Name { get; set; }
		public string Cloud_DB_Name { get; set; }
		public string Cloud_DB_User_Name { get; set; }
		public string Cloud_DB_Pwd { get; set; }
		public string API_Ref_No { get; set; }
		public string ExcelFile_Store_Required_AF_CashOut { get; set; }
	}
}