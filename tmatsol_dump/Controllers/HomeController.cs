using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tmatsol_dump.Models;

namespace tmatsol_dump.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Download()
        {
            return View();
        }
        public ActionResult Branches()
        {
            return View();
        }
        public ActionResult OurExpertise()
        {
            return View();
        }
        public ActionResult Gallery()
        {
            return View();
        }
        public ActionResult Videos()
        {
            return View();
        }
        public ActionResult services()
        {
            return View();
        }
        public ActionResult _partial()
        {
            return View();
        }
        public ActionResult Coverpage()
        {
            //return View("coming_soon", "Home");
            return View();
        }

        public ActionResult coming_soon()
        {
            return View();
        }
      
        public ActionResult Index()
        {          

            return View();
        }

        public ActionResult About()
        {         
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Updates()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult contactus()
        {
                       
                ContactUs con = new ContactUs();
                var productlist = con.productlist;
                return View(productlist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult contactus(ContactUs con)
        {
            try
            {
                if (!ModelState.IsValid)
                 {
                    string EncodedResponse = Request.Form["g-Recaptcha-Response"];
                    bool IsCaptchaValid = (ReCaptchaClass.Validate(EncodedResponse) == "true" ? true : false);

                    if (IsCaptchaValid)
                    {
                        string resultmessage = "";
                        ContactUs contact = new ContactUs();
                        contact.name = con.name;
                        contact.companyName = con.companyName;
                        contact.contactNo = con.contactNo;
                        contact.emailId = con.emailId;
                        contact.selectedProduct = con.selectedProduct;
                        contact.EmailRequirement(ref resultmessage);

                        if(resultmessage == "success"){
                            TempData["a"] = "success";
                           
                            return View("Index");
                        }
                        else
                        {
                            return View("Error");
                        }
                    }
                    return View("Error");  
                }
                return View("Error");
            }
            catch(Exception e)
            {
                return View("Error",e.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult contactusIndex(ContactUs con)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string EncodedResponse = Request.Form["g-Recaptcha-Response"];
                    bool IsCaptchaValid = (ReCaptchaClass.Validate(EncodedResponse) == "true" ? true : false);

                    if (IsCaptchaValid == true)
                    {
                        string resultmessage = "";
                        ContactUs contact = new ContactUs();
                        contact.name = "--";
                        contact.companyName = "--";
                        contact.contactNo = con.contactNo;
                        contact.emailId = con.emailId;
                        contact.selectedProduct = "--";
                        contact.EmailRequirement(ref resultmessage);

                        if (resultmessage == "success")
                        {
                            TempData["a"] = "Your Demo Request Successfully Submitted.";
                            return PartialView("_partial");
                            
                        }
                        else
                        {
                            TempData["a"] = "Server Timeout, Please Try Again.";
                            return View("Error");
                        }
                    }
                    else
                    {
                        TempData["a"] = "Server Timeout, Please Try Again.";
                        return View("Error");
                    }
                }
                else
                {
                    TempData["a"] = "Server Timeout, Please Try Again.";
                    return View("Error");
                }
            }
            catch (Exception e)
            {
                TempData["a"] = "Server Timeout, Please Try Again. " +e.Message ;
                return View("Error");
            }
        }

        [AllowAnonymous]
        public ActionResult Complaint()
        {
            try
            {
                return View();
            }
            catch(Exception error)
            {
                return View("Error", error.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Complaint(Compliant complaint)
        {
            return View();
        }


        public ActionResult downloads(string filename)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "downloads/";
                byte[] fileBytes = System.IO.File.ReadAllBytes(path + filename);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet,filename);
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
                return View("Error",e.Message);
            }
        }
    }
}