//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TattooProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vwBooking
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> bookingdate { get; set; }
        public Nullable<System.TimeSpan> time { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Nullable<int> ArtistID { get; set; }
        public string AName { get; set; }
        public string ASurname { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
