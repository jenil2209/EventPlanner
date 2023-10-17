using EventPlannerData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventManagementWeb.ViewModels
{
    public class EventBooking
    {
        public int EventBookingId { get; set; }
        [Required]
        [Display(Name = "Event Type")]
        public int? EventType { get; set; }
        [Required]
        [Display(Name = "Venue Type")]
        public int? VenueType { get; set; }
        [Required]
        [Display(Name = "Guest Count")]
        public int? GuestCount { get; set; }
        [Required]
        [Display(Name = "Booking Date")]
        public DateTime? BookingDate { get; set; }
        public List<EventType> EventTypes { get; set; }
        public List<VenueType> VenueTypes { get; set; }
        public string EmailId { get; set; }
    }

    public class VenueType
    {
        public int VenueId { get; set; }
        public string VenueName { get; set; }
    }

    public class EventType
    {
        public int EventId { get; set; }
        public string Type { get; set; }
    }
    public class EventBookinkgListView
    {
        public List<EventBookingsList> EventBookingsLists { get; set; }
    }
    public class EventBookingsList
    {
        public int EventBookingId { get; set; }
        public int? BookingNo { get; set; }
        public DateTime? BookingDate { get; set; }
        public string EventType { get; set; }
        public string Venue { get; set; }
    }

    public class TodoMainModel
    {
        public int TodoId { get; set; }
        public int? EventBookingId { get; set; }
        [Required]
        [Display(Name = "Event Type")]
        public int? EventType { get; set; }
        [Required]
        [Display(Name = "Event Name")]
        public int? EventName { get; set; }
        [Required]
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Booking Date")]
        public DateTime? DueDate { get; set; }

        public bool? Pending { get; set; }
        public List<EventType> EventTypes { get; set; }
        public List<TodoList> ToDoList { get; set; }
    }

    public class TodoList
    {
        public int TodoId { get; set; }
        public string TodoName { get; set; }
        public int? EventId { get; set; }
        public string EventName { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? IsPending { get; set; }
    }
}