using EventPlannerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManagementWeb.ViewModels
{
    public class VenueModel
    {
        public tbVenue tbVenue { get; set; }
        public List<VenueList> VenueList { get; set; }
    }

    public class VenueList
    {
        public int VenueId { get; set; }
        public string VenueName { get; set; }
        public int? VenueCost { get; set; }

    }
}