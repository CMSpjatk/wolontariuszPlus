using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.Home.Models
{
    public class VolunteerProfileViewModel
    {
        public int VolunteerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public ICollection<PastEventsViewModel> PastEventViewModelList { get; set; }
        public int Points { get; set; }
    }
}
