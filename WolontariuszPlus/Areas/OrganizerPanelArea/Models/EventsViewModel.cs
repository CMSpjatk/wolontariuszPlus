using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Models
{
    public class EventsViewModel
    {
        public IEnumerable<DisplayEventViewModel> EventViewModels { get; set; }
        public VolunteerPanelViewType ViewType { get; set; }
        public DisplayEventViewModel DefaultEvent => new DisplayEventViewModel();
    }

    public enum VolunteerPanelViewType
    {
        UPCOMING_EVENTS, ARCHIVED_EVENTS
    }
}
