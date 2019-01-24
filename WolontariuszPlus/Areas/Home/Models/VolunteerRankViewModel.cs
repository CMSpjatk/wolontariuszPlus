using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.Home.Models
{
    public class VolunteerRankViewModel
    {
        public int VolunteerId { get; set; }

        [Display(Name = "Imię i nazwisko")]
        public string FullName { get; set; }

        [Display(Name = "Punkty")]
        public int Points { get; set; }
    }
}
