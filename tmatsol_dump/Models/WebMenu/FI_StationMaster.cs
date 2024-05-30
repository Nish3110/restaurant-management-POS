using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Table_Cart_V2k20.Models
{
    public class FI_StationMaster
    {
        [Key]
        public int StationID { get; set; }
        public string StationName { get; set; }
        public string ComputerName { get; set; }
        public string ReceiptPrinterType { get; set; }
        public string ReceiptPrinterName { get; set; }
        public string CashDrawerAttached { get; set; }
        public string ReportPrinterName { get; set; }
        public string BarPrinterName { get; set; }
        public string PoleDisplayCommPort { get; set; }
        public string PoleDisplayWelcomeMessage1 { get; set; }
        public string PoleDisplayWelcomeMessage2 { get; set; }
        public string CallerIDCommPort { get; set; }
        public Nullable<DateTime> Entry_Date_Time { get; set; }
        public Nullable<DateTime> Exit_Date_Time { get; set; }
        public string BillPrinterName { get; set; }
        public string Group_Digit_Format { get; set; }
        public string PoleDisplayCommPort_Setting { get; set; }
        public int? Counter_PrintCopy { get; set; }
        public int? Delivery_PrintCopy { get; set; }
        public int? Counter_PrintCopy_RECEIPT { get; set; }
        public int? Delivery_PrintCopy_RECEIPT { get; set; }
        public int? Dine_IN_PrintCopy_RECEIPT { get; set; }
        public int? Numbers_Of_Counter { get; set; }
        public Nullable<DateTime> POS_Updated_Version_DateTime { get; set; }
        public string Bill_Print_Format { get; set; }
        public string Receipt_Print_Format { get; set; }
        public int? Port_No { get; set; }
        public string SERVER_IP { get; set; }
        public int? SERVER_PORT { get; set; }
        public string StationType { get; set; }
        public int? PLU_ITEM_CD_START { get; set; }
        public int? PLU_ITEM_CD_LENGTH { get; set; }
        public int? PLU_AMOUNT_START { get; set; }
        public int? PLU_AMOUNT_LENGTH { get; set; }
        public string Default_PassWord_Screen { get; set; }
        public string DineIN_LogOff { get; set; }
        public string KOT_DISPLAY_REQUIRED { get; set; }
        public string KOT_DISPLAY_REQUIRED_AT { get; set; }
        public string KOT_FOOD_ACCEPTANCE_REQUIRED { get; set; }
        public int? MESS_MBR { get; set; }
        public int? MESS_RPB { get; set; }
        public string WS_Auto_Com_Port { get; set; }
        public string WS_Auto_Com_String { get; set; }
        public string WS_Auto_Received_String { get; set; }
        public string WS_Brand_Name { get; set; }
        public string StockDisplay_BO { get; set; }
        public string Additional_Billing_Printer_COUNTER { get; set; }
        public string Additional_Billing_Printer_DELIVERY { get; set; }
        public string Code_Req_With_Name { get; set; }
        public int? Last_Selected_Language { get; set; }
        public string KOT_Monitor_Print_Req { get; set; }

    }
}