using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManagementWeb.ViewModels;
using EventPlannerData;

namespace EventManagementWeb.Controllers
{
    public class VenueController : Controller
    {
        dbEventPlannerEntities objEntity = new dbEventPlannerEntities();
        [HttpGet]
        public ActionResult VenueList()
        {
            VenueModel objModel = new VenueModel();
            objModel.VenueList = new List<VenueList>();

            //if (objEntity.tbVenues.Count() > 1)
            //{
            objModel.VenueList = (from tb in objEntity.tbVenues
                                  select new VenueList
                                  {
                                      VenueId = tb.VenueID,
                                      VenueName = tb.VenueName,
                                      VenueCost = tb.VenueCost,
                                  }).ToList();
            //}

            return View(objModel);
        }

        [HttpGet]
        public ActionResult CreateVenue(int VenueId = 0)
        {
            tbVenue Venue = new tbVenue();
            if (VenueId > 0)
            {
                Venue = objEntity.tbVenues.Where(x => x.VenueID == VenueId).FirstOrDefault();
            }
            return View(Venue);
        }

        [HttpPost]
        public ActionResult CreateVenue(tbVenue tbVenue)
        {
            if (tbVenue.VenueID > 0)
            {
                var tabledate = (from tb in objEntity.tbVenues where tb.VenueID == tbVenue.VenueID select tb).FirstOrDefault();
                if (tabledate != null)
                {
                    tabledate.VenueCost = tbVenue.VenueCost;
                    tabledate.VenueName = tbVenue.VenueName;
                    objEntity.SaveChanges();
                }
            }
            if (tbVenue != null)
            {
                tbVenue.CreatedDate = DateTime.Now;

                objEntity.tbVenues.Add(tbVenue);
                objEntity.SaveChanges();
            }

            return RedirectToAction("VenueList", "Venue");
        }

    }
}