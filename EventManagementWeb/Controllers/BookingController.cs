using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace EventManagementWeb.Controllers
{
    public class BookingController : Controller
    {
        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendInvitations(string EmailTo = "")
        {
            try
            {
                //    if (file != null && file.ContentLength > 0)
                //    {
                //        string str = Path.GetFileName(file.FileName);
                //        string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(file.FileName));
                //        file.SaveAs(path);
                sendEmail(EmailTo);
                //}

                //// Set up the SMTP client and specify the SMTP server details
                //SmtpClient smtpClient = new SmtpClient("smtp.example.com");
                //smtpClient.Port = 587; // Use the appropriate port for your SMTP server
                //smtpClient.Credentials = new NetworkCredential("sanjeevkumarr0u7h@gmail.com", "Sanjeev");

                //// Create a new email message
                //MailMessage mailMessage = new MailMessage();
                //mailMessage.From = new MailAddress("your_email@example.com");
                //mailMessage.To.Add("recipient@example.com");
                //mailMessage.Subject = "Subject";
                //mailMessage.Body = "This is the body of the email.";

                //// Send the email
                //smtpClient.Send(mailMessage);

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return View();
        }

        protected void sendEmail(string EmailTo)
        {
            string strFrom = ConfigurationManager.AppSettings["Email"].ToString();
            string pass = ConfigurationManager.AppSettings["Password"].ToString();
            string host = ConfigurationManager.AppSettings["Host"].ToString();
            string strSubject = "FREE Seminar: Getting Started with Azure on X Dec 2015";
            MailMessage mailMessage = new MailMessage();
            MailAddress fromAddress = new MailAddress(strFrom);
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Host = host;
            smtpClient.Port = 25;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            NetworkCredential basicCredential = new NetworkCredential(strFrom, pass);
            smtpClient.Credentials = basicCredential;
            mailMessage.From = fromAddress;
            mailMessage.Subject = strSubject;
            mailMessage.IsBodyHtml = true;
            // added for style sheet & hyper link  
            string link = "http://c-sharpcorner.com/";
            string color_Blue = "color:blue";
            string list_Bold = "font-weight: bold;";
            // added for image attachment in body  
            var contentID = "Image";
            var inlineLogo = new Attachment(@"E:\Images\Screenshot.png");
            inlineLogo.ContentId = contentID;
            inlineLogo.ContentDisposition.Inline = true;
            inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            mailMessage.Attachments.Add(inlineLogo);
            mailMessage.Body = "<htm><body> <h1 style=\"" + color_Blue + "\">Seminar C# Corner Pune Chapter Meet</h1> <p>Dear Rupesh,</p> <p><strong>We are pleased to invite you to attend the Webinar by C# Corner Pune Chapter on Azure to be held on DD Dec, 2015. It's completely free of cost offline event.</strong></p><ol><li style=\"" + list_Bold + "\">Event Venu: Flat No 5, Spicer College Road,Sangavi, Pune - 27</li><li style=\"" + list_Bold + "\">Event Date: x Dec 2015 </li><li style=\"" + list_Bold + "\">Event Start Time: 9 AM sharp </li><li style=\"" + list_Bold + "\">Conatct Us: Rupesh 9545273748 </li> <ol><p>Thanks</p><br/> <a href=\"" + link + "\"><img src=\"cid:" + contentID + "\"></a> </body></html>";
            mailMessage.To.Add(EmailTo);
            try
            {
                smtpClient.Send(mailMessage);
                ViewBag.exception = "Emails Send Successfully !!!";
            }
            catch (Exception ex)
            {
                ViewBag.exception = ex.Message;
            }
        }
    }
}