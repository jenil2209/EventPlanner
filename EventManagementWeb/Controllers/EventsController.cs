using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagementWeb.Controllers
{
    public class EventsController : Controller
    {
        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateEvent()
        {

            return View();
        }
    }
}