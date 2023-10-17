using EventManagementWeb.ViewModels;
using EventPlannerData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Mvc;

namespace EventManagementWeb.Controllers
{
    public class EventsController : Controller
    {
        dbEventPlannerEntities objEntity = new dbEventPlannerEntities();
        private int currentYear;
        private int currentBookingNumber;

        // GET: Booking
        public ActionResult EventsList()
        {
            EventBookinkgListView model = new EventBookinkgListView();
            List<EventBookingsList> list = new List<EventBookingsList>();

            list = (from tb in objEntity.tbBookingVenues
                    join tb2 in objEntity.tbEventTypes on tb.EventTypeID equals tb2.EventID
                    join tb3 in objEntity.tbVenues on tb.VenueID equals tb3.VenueID
                    select new EventBookingsList
                    {
                        EventBookingId = tb.BookingVenueID,
                        BookingNo = tb.BookingID,
                        BookingDate = tb.CreatedDate,
                        EventType = tb2.EventType,
                        Venue = tb3.VenueName
                    }).ToList();
            if (list != null)
            {
                model.EventBookingsLists = list;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateEvent(int EventBookingId = 0)
        {
            EventBooking model = new EventBooking();

            if (EventBookingId > 0)
            {
                model.EventBookingId = EventBookingId;
                if (model.EventBookingId > 0)
                {
                    // Booking Details
                    var EventDetails = (from tb in objEntity.tbBookingVenues
                                        where tb.BookingVenueID == EventBookingId
                                        select tb).FirstOrDefault();

                    if (EventDetails != null)
                    {
                        model.EventBookingId = EventDetails.BookingVenueID;
                        model.BookingDate = EventDetails.CreatedDate;
                        model.EventType = EventDetails.EventTypeID;
                        model.VenueType = EventDetails.VenueID;
                        model.GuestCount = EventDetails.GuestCount;
                    }
                }
            }

            // Event Types List for dropdown
            model.EventTypes = (from tb in objEntity.tbEventTypes
                                select new EventType
                                {
                                    EventId = tb.EventID,
                                    Type = tb.EventType
                                }).ToList();

            // Venue Types List for dropdown
            model.VenueTypes = (from tb in objEntity.tbVenues
                                select new VenueType
                                {
                                    VenueId = tb.VenueID,
                                    VenueName = tb.VenueName
                                }).ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateEvent(EventBooking model, string btnSubmit = "")
        {
            //if (btnSubmit == "Send Invitation")
            {
                return RedirectToAction("SendInvitations", "Events", model);
            }

            if (model.EventBookingId > 0)
            {
                var tabledate = (from tb in objEntity.tbBookingVenues where tb.BookingVenueID == model.EventBookingId select tb).FirstOrDefault();
                if (tabledate != null)
                {
                    tabledate.CreatedDate = model.BookingDate;
                    tabledate.GuestCount = model.GuestCount;
                    tabledate.EventTypeID = model.EventType;
                    tabledate.VenueID = model.VenueType;
                    objEntity.SaveChanges();
                }
            }
            else
            {
                if (model != null)
                {
                    tbBookingVenue table = new tbBookingVenue();
                    table.CreatedDate = model.BookingDate;
                    table.EventTypeID = model.EventType;
                    table.VenueID = model.VenueType;
                    table.GuestCount = model.GuestCount;
                    table.BookingID = Convert.ToInt32(GenerateBookingNumber());

                    objEntity.tbBookingVenues.Add(table);
                    objEntity.SaveChanges();
                }
            }

            return RedirectToAction("EventsList", "Events");
        }

        public string GenerateBookingNumber(int LastBookingNumber = 0)
        {
            string formattedBookingNumber = "";
            if (LastBookingNumber > 0)
            {
                return formattedBookingNumber = Convert.ToString(LastBookingNumber++);
            }
            else
            {
                currentBookingNumber = 0;
                // Get the current year
                int yearNow = DateTime.Now.Year;

                // If the year has changed, reset the booking number
                if (yearNow > currentYear)
                {
                    currentYear = yearNow;
                    currentBookingNumber = 0;
                }

                // Increment the booking number
                currentBookingNumber++;

                // Format the booking number with leading zeros
                formattedBookingNumber = $"{currentYear % 100}{currentBookingNumber:D4}";

                return formattedBookingNumber;
            }
        }

        public ActionResult SendInvitations(EventBooking model)
        {
            try
            {
                //    if (file != null && file.ContentLength > 0)
                //    {
                //        string str = Path.GetFileName(file.FileName);
                //        string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(file.FileName));
                //        file.SaveAs(path);
                var VenueName = (from tb in objEntity.tbVenues where tb.VenueID == model.VenueType select tb.VenueName).FirstOrDefault();
                sendEmail(model.EmailId, VenueName);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return RedirectToAction("EventsList", "Events");
        }

        protected void sendEmail(string EmailTo, string Venue)
        {
            //string strFrom = ConfigurationManager.AppSettings["Email"].ToString();
            //string pass = ConfigurationManager.AppSettings["Password"].ToString();
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
            //  NetworkCredential basicCredential = new NetworkCredential("A00271994@mycambrian.ca", "Sudbury123");
            _ = new SmtpClient();
            smtpClient.Credentials = basicCredential;
            mailMessage.From = fromAddress;
            mailMessage.Subject = strSubject;
            mailMessage.IsBodyHtml = true;
            // added for style sheet & hyper link  
            string link = "http://c-sharpcorner.com/";
            string color_Blue = "color:blue";
            string list_Bold = "font-weight: bold;";
            // added for image attachment in body  
            // var contentID = "Image";
            //var inlineLogo = new Attachment(@"E:\Images\Screenshot.png");
            //inlineLogo.ContentId = contentID;
            //inlineLogo.ContentDisposition.Inline = true;
            //inlineLogo.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            //mailMessage.Attachments.Add(inlineLogo);
            mailMessage.Body = "<htm><body> <h1 style=\"" + color_Blue + "\">Please join our event</h1> <p>Dear Friend,</p> <p><strong>We are pleased to invite you to attend the event.</strong></p><ol><li style=\"" + list_Bold + "\">Event Venue:" + Venue + "</li><li style=\"" + list_Bold + "\">Event Date: 2023 </li><li style=\"" + list_Bold + "\">Event Start Time: 9 AM sharp </li><li style=\"" + list_Bold + "\">Conatct Us : 123487972 </li> <ol><p>Thanks</p><br/></body></html>";
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

        public ActionResult Delete(int EventBookingId)
        {
            if (EventBookingId > 0)
            {
                using (var dbcontext = new dbEventPlannerEntities())
                {
                    var x = (from y in dbcontext.tbBookingVenues
                             where y.BookingVenueID == EventBookingId
                             select y).FirstOrDefault();
                    dbcontext.tbBookingVenues.Remove(x);
                    dbcontext.SaveChanges();

                    ViewBag.message = "Event Deleted Successfully !!";
                }
            }
            return RedirectToAction("EventsList");
        }

        public ActionResult AddTodo(int EventBookingId = 0)
        {
            TodoMainModel model = new TodoMainModel();
            List<TodoList> list = new List<TodoList>();

            var todoEvent = (from tb in objEntity.tbEventsTodoes select tb.EventId).Distinct().ToList();

            list = (from tb in objEntity.tbEventsTodoes
                    join tb2 in objEntity.tbEventTypes on tb.EventId equals tb2.EventID
                    where tb.EventId == EventBookingId
                    select new TodoList
                    {
                        TodoId = tb.Id,
                        EventName = tb2.EventType,
                        DueDate = tb.DueDate,
                        IsPending = tb.IsPending,
                    }).ToList();

            //list = (from tb in objEntity.tbEventsTodoes
            //        join tb2 in objEntity.tbEventTypes on tb.EventId equals tb2.EventID
            //        select new TodoList
            //        {
            //            TodoId = tb.Id,
            //            EventName = tb2.EventType,
            //            DueDate = tb.DueDate,
            //            IsPending = tb.IsPending,
            //        }).ToList();

            if (list != null)
            {
                model.ToDoList = list;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateTodo(int TodoId = 0)
        {
            TodoMainModel objModel = new TodoMainModel();
            if (TodoId > 0)
            {
                var table = objEntity.tbEventsTodoes.Where(x => x.Id == TodoId).FirstOrDefault();
                if (table != null)
                {
                    objModel.TodoId = table.Id;
                    objModel.TaskName = table.TaskName;
                    objModel.Pending = table.IsPending == true ? "Pending" : "Done";
                    objModel.Description = table.Description;
                    objModel.EventType = table.EventId;
                }
            }
            // Event Types List for dropdown
            objModel.EventTypes = (from tb in objEntity.tbEventTypes
                                   select new EventType
                                   {
                                       EventId = tb.EventID,
                                       Type = tb.EventType
                                   }).ToList();
            return View(objModel);
        }
    }
}