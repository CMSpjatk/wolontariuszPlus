using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.VolunteerPanelArea.Models
{
    public class EventViewModel
    {
        public int EventId { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public DateTime Date { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Wymagane punkty")]
        public int RequiredPoints { get; set; }

        [Display(Name = "Organizator")]
        public string OrganizerName { get; set; }

        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Display(Name = "Otrzymane punkty")]
        public int ReceivedPoints { get; set; }

        [Display(Name = "Ocenione?")]
        public bool IsRated { get; set; }
    }
}
