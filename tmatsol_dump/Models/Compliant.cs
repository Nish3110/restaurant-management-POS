using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using tmatsol_dump.Helper_Code.Common;

namespace tmatsol_dump.Models
{
    public class Compliant
    {
        /*-------- Through Software Call Receive if not Available open text box on view ----------*/
        [Display(Name ="Company Name")]
        [Required(ErrorMessage = "Company Name Is Required.")]
        public string companyName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address Is Required"),
            EmailAddress(ErrorMessage = "Valid Email Address Is Required")]
        public string emailAddress { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Product Name Is Required.")]
        public string softwareName { get; set; }

        [Display(Name = "Mobile Number")]
        [MaxLength(10, ErrorMessage = "Please Enter Valid Moile Number ex.: 050####### ."),
            MinLength(10, ErrorMessage = "Please Enter Valid Moile Number ex.: 050####### .")]
        public UInt64 mobileNumber { get; set; }

        [Display(Name = "Landline Number")]
        [MaxLength(9, ErrorMessage = "Please Enter Valid Landline Number ex.: 043798394 ."),
    MinLength(9, ErrorMessage = "Please Enter Valid Landline Number ex.: 043798394 .")]
        public UInt64 landLineNumber { get; set; }

        /*--------- customer will enter manually. --------*/
        [Display(Name = "Contact Person Name")]
        [Required(ErrorMessage ="Contact Person Name Is Required.")]
        public string contactPersonName {get;set;}

        [Display(Name = "Explain Promblem")]
        [Required]
        public string ComplaintText { get; set; }

        [Display(Name = "Attach Image/PDF/Video"),
            FileExtensions(Extensions =".jpg,.png,.jpeg,.pdf,.mp4",ErrorMessage ="Only images, videos (Max. Size is 50MB.)")]
        [AllowFileSize(FileSize = 50*1024*1024, ErrorMessage ="Maxixmum File Size can be 50MB (Large Files Not Allowed).")]
        public HttpPostedFileBase[] File { get; set; }
    }
}