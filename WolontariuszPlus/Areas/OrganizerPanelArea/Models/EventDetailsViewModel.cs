using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Models
{
    public class EventDetailsViewModel
    {
        public int EventId { get; set; }
        public string Name { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d MMMM yyyy} o godzinie {0:HH:mm}")]
        public DateTime Date { get; set; }

        [Display(Name = "Ilość zebranych pieniędzy")]
        public double CollectedMoneySum { get; set; }

        public bool CanModify { get; set; }

        public ICollection<VolunteerViewModel> Volunteers { get; set; }
        public VolunteerViewModel DefaultVolunteer => new VolunteerViewModel();
        public PanelViewType ViewType { get; set; }

        public string ImageRelativePath { get; set; }
    }
}
