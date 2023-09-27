using EventManagementWeb.ViewModels;
using EventPlannerData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventManagementWeb.Interface
{
    public interface IVenue
    {
        Task<List<VenueList>> VenueList(string Entry);
        Task<tbVenue> GetVenueByID(int EntryID);
    }
}
