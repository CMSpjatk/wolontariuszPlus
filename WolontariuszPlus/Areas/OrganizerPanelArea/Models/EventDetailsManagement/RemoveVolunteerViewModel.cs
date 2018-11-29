using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Models.EventDetailsManagement
{
    public class RemoveVolunteerViewModel
    {
        public int VolunteerOnEventId { get; set; }
        public int EventId { get; set; }

        [Display(Name = "Imię i nazwisko")]
        public string VolunteerName { get; set; }

        [Display(Name = "Ilość zebranych pieniędzy")]
        public double CollectedMoney { get; set; }

        [Display(Name = "Nazwa wydarzenia")]
        public string EventName { get; set; }
    }
}
