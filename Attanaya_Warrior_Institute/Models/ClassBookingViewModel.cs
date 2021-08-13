using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Attanaya_Warrior_Institute.Models
{
    public class ClassBookingViewModel
    {
        public Guid InstanceID { get; set; }
        public EduClass EduClass { get; set; }
        public StudentBookings Booking { get; set; }
        public Students Student { get; set; }
    }

    public class ClassBookingReceiptModel
    {
        public string EduClassId { get; set; }
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ClassTitle { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string MaxAttendees { get; set; }
        public string TotalPrice { get; set; }

    }
}