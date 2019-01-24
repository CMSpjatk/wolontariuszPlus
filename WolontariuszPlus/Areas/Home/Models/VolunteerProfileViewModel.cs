using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.Home.Models
{
    public class VolunteerProfileViewModel
    {
        public int VolunteerId { get; set; }

        [Display(Name = "Imię i nazwisko")]
        public string FullName { get; set; }

        public string Email { get; set; }

        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Miasto")]
        public string City { get; set; }

        public ICollection<PastEventsViewModel> PastEventViewModelList { get; set; }

        [Display(Name = "Punkty")]
        public int Points { get; set; }
    }
}
