using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
	public class FI_Payment_SETTLED
	{
		public string LOC_CODE { get; set; }
		[Key]
	    public double Payment_ID { get; set; }
		public DateTime Payment_Date { get; set; }
		public int Cashier_log_ID { get; set; }
		public int Invoice_ID { get; set; }
		public int Payment_Code { get; set; }
		public double FC_Amount_Tendered { get; set; }
		public double Currency_Conversion { get; set; }
		public double BC_Amount_Tendered { get; set; }
		public double BC_AmountPaid { get; set; }
		public int Record_ID_FI_Cashier_IN_OUT_Log { get; set; }
		public DateTime Entry_Date { get; set; }
	}
}