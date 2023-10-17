using EventManagementWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EventManagementWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
                return View();
            else return RedirectToAction("Login", "User");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact vm, string message = "")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string strFrom = "sanjeevk@itpathsolutions.com";
                    string pass = "Sanjeev@123456";
                    string host = "smtp.gmail.com";
                    string strSubject = "Event Invitation";
                    MailMessage mailMessage = new MailMessage();
                    MailAddress fromAddress = new MailAddress(strFrom);

                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.Host = host;
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    NetworkCredential basicCredential = new NetworkCredential(strFrom, pass);
                    _ = new SmtpClient();
                    smtpClient.Credentials = basicCredential;
                    mailMessage.From = fromAddress;
                    mailMessage.Subject = strSubject;
                    mailMessage.IsBodyHtml = true;
                    string link = "http://c-sharpcorner.com/";
                    string color_Blue = "color:blue";
                    string list_Bold = "font-weight: bold;";
                    mailMessage.Body = "<htm><body> <h1 style=\"" + color_Blue + "\">Thank you for contacting us</h1> <p>Dear Friend,</p> <p><strong>We are pleased to inform you about all the events which we are going to arrange.</strong></p><ol><li style=\"" + list_Bold + "\"></li><li style=\"" + list_Bold + "\">Event Date: 2023 </li><li style=\"" + list_Bold + "\"></li><li style=\"" + list_Bold + "\">Conatct Us : 123487972 </li> <ol><p>Thanks</p><br/></body></html>";
                    mailMessage.To.Add(vm.Email);

                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Sorry we are facing Problem here {ex.Message}";
                }
            }
            return View();
        }


        public ActionResult Error()
        {
            return View();
        }
    }
}