using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
	public class FI_Combo_Items_Relation
	{
		public int ComboID { get; set; }
	    [Key]
		public int ItemID { get; set; }
		public int Step_No { get; set; }
		public double Qty { get; set; }
		public int OrderBy { get; set; }
		public int combo_type { get; set; }
		public decimal Sale_Price { get; set; }
	}
}