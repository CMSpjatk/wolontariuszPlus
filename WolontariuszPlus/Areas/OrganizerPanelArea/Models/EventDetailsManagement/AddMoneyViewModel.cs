using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WolontariuszPlus.Common;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Models.EventDetailsManagement
{
    public class AddMoneyViewModel
    {
        public int VolunteerOnEventId { get; set; }
        public int EventId { get; set; }

        [Display(Name = "Imię i nazwisko")]
        public string VolunteerName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ErrorMessagesProvider.VolunteerOnEventErrors.InvalidAmmountOfMoney)]
        [Display(Name = "Ilość zebranych pieniędzy")]
        public double CollectedMoney { get; set; }

        [Display(Name = "Nazwa wydarzenia")]
        public string EventName { get; set; }
    }
}
