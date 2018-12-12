using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.VolunteerPanelArea.Models
{
    public class EventsViewModel
    {
        public IEnumerable<EventViewModel> EventViewModels { get; set; }
        public PanelViewType ViewType { get; set; }
        public EventViewModel DefaultEvent => new EventViewModel();
    }

    public enum PanelViewType
    {
        UPCOMING_EVENTS, ARCHIVED_EVENTS, EVENTS_WITHOUT_OPINION
    }
}
