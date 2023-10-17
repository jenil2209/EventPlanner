using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        // Read the upload folder path from web.config


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
            string uploadFolderPath = @"C:\EventManagerStorage\Events";
            HttpPostedFileBase file = Request.Files["File"];
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            string fileName = "";
            fileName = Path.GetFileName(file.FileName);

            if (tbVenue.VenueID > 0)
            {
                var tabledate = (from tb in objEntity.tbVenues where tb.VenueID == tbVenue.VenueID select tb).FirstOrDefault();
                if (tabledate != null)
                {
                    tabledate.VenueCost = tbVenue.VenueCost;
                    tabledate.VenueName = tbVenue.VenueName;
                    tabledate.VenueFileName = fileName;
                    objEntity.SaveChanges();
                }
            }
            else
            {
                var Duplicate = (from tb in objEntity.tbVenues where tb.VenueName == tbVenue.VenueName select tb).FirstOrDefault();
                if (Duplicate == null)
                {
                    tbVenue.CreatedDate = DateTime.Now;
                    tbVenue.VenueFileName = fileName;

                    objEntity.tbVenues.Add(tbVenue);
                    objEntity.SaveChanges();
                }
                else
                {
                    TempData["message"] = "This venue already added!! having cost = " + Duplicate.VenueCost;
                    return View();
                }
            }

            if (file != null && file.ContentLength > 0)
            {
                if (tbVenue.VenueID > 0)
                {
                    uploadFolderPath = Path.Combine(uploadFolderPath, tbVenue.VenueID.ToString());
                }
                // Check if the folder exists; if not, create it
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }
                // Generate a unique file name to avoid overwriting existing files
                string filePath = Path.Combine(uploadFolderPath, fileName);
                file.SaveAs(filePath);
            }
            return RedirectToAction("VenueList", "Venue");
        }


        public ActionResult Delete(int VenueId)
        {
            if (VenueId > 0)
            {
                using (var dbcontext = new dbEventPlannerEntities())
                {
                    var x = (from y in dbcontext.tbVenues
                             where y.VenueID == VenueId
                             select y).FirstOrDefault();
                    dbcontext.tbVenues.Remove(x);
                    dbcontext.SaveChanges();

                    ViewBag.message = "Venue Deleted Successfully !!";
                }
            }
            return RedirectToAction("VenueList");
        }
    }
}