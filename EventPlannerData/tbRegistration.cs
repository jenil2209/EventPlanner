//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventPlannerData
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbRegistration
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<int> CountryID { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}