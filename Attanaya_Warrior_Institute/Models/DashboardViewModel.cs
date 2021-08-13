using System.Collections.Generic;

namespace Attanaya_Warrior_Institute.Models
{
    public class DashboardViewModel
    {
        public List<HoursOfOperation> Hours { get; set; }
        public List<Rooms> Rooms { get; set; }
        public List<Discounts> Discounts { get; set; }
        public List<ClosedDates> ClosedDates { get; set; }
        public List<EnhancedBookingStub> EBookings { get; set; }
    }
}