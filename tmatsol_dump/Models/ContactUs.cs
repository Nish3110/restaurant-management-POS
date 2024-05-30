using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace tmatsol_dump.Models
{
    public class ContactUs
    {
        [Required(ErrorMessage ="This Field is required."),MinLength(3,ErrorMessage ="Minimum 3 characters required"),RegularExpression("^[A-z]{3,100}$",ErrorMessage ="Only alphabets allowed")]
        public string name { get; set; }

        [MinLength(3, ErrorMessage = "Minimum 3 characters required"), RegularExpression("^[A-z]{3,100}$", ErrorMessage = "Only alphabets allowed")]
        public string companyName { get; set; }

        [Required(ErrorMessage = "This Field is required."), RegularExpression("^[0-9]{7,10}", ErrorMessage = "Required Format 05########")]
        public string contactNo { get; set; }

        [Required(ErrorMessage = "This Field is required."),EmailAddress(ErrorMessage ="Please Enter Valid Email Address.")]
        public string emailId { get; set; }

        [Required(ErrorMessage = "This Field is required.")]
        public IEnumerable<SelectListItem> productlist { get; set; }

        [Required(ErrorMessage = "This Field is required.")]
        public string selectedProduct{ get; set; }

        
        public static IEnumerable<SelectListItem> getProductList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem {Text="Think POS Restaurant",Value="Think POS Restaurant" },
                new SelectListItem {Text="Think POS Restaurant with Recipe Management", Value="Think POS Restaurant with Recipe Management" },
                new SelectListItem {Text="Think POS Restaurant Management Total",Value="Think POS Restaurant Management Total" },
                new SelectListItem {Text="Think POS Retail", Value="Think POS Retail" },
                new SelectListItem {Text="Think POS Retail With Inventory Management",Value="Think POS Retail With Inventory Management" },
                new SelectListItem {Text="Think POS Retail Management Total",Value="Think POS Retail Management Total" },
                new SelectListItem {Text="Think TMATS Accounting",Value="Think TMATS Accounting" },
                new SelectListItem {Text="Think TMATS Inventory",Value="Think TMATS Inventory" },
                new SelectListItem {Text="Think TMATS Total",Value="Think TMATS Total" },
                new SelectListItem {Text="Think Laundry Billing Software", Value="Think Laundry Billing Software" },
                new SelectListItem {Text="Think Saloon Billing Software",Value="Think Saloon Billing Software" },
                new SelectListItem {Text="Think Game Parlour Software",Value="Think Game Parlour Software" },
                new SelectListItem {Text="Think Cheque printing utility",Value="Think Cheque printing utility" },
            };
            return items;
        }

        public bool EmailRequirement(ref string resultMessage)
        {
            try
            {
                MailMessage mail = new MailMessage("reports@tmatsol.com", "dcshah@eim.ae,posrestro@gmail.com");
                
                mail.From = new MailAddress("reports@tmatsol.com");
                
                SmtpClient smtp = new SmtpClient("mail.tmatsol.com", 587);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential("reports@tmatsol.com","Tdchampak@98765");
                mail.IsBodyHtml = true;

                mail.Subject = "Tmatsol.com Inquiry";

                
                mail.Body = "<table><thead><tr><td>Name</td><td>Company Name</td><td>Contact No</td><td>Email Id</td><td>Product</td></tr></thead><tbody><tr><td>"+name.ToString()+"</td><td>" +companyName.ToString() +"</td><td>"+contactNo.ToString() +"</td><td>" +emailId.ToString() +"</td><td>" +selectedProduct.ToString() +"</td></tr></tbody></table>";
                smtp.Send(mail);
                resultMessage = "success";
                return true;
            }
            catch(Exception e)
            {
                resultMessage = "Error : " + e.Message;
                return false;
            }
        }
    }

  
}