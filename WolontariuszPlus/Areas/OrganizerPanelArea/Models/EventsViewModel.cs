using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Models
{
    public class EventsViewModel
    {
        public IEnumerable<DisplayEventViewModel> EventViewModels { get; set; }
        public PanelViewType ViewType { get; set; }
        public DisplayEventViewModel DefaultEvent => new DisplayEventViewModel();
    }

    public enum PanelViewType
    {
        UPCOMING_EVENTS, ARCHIVED_EVENTS
    }
}
