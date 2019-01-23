using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Models
{
    public class PastEventsViewModel
    {
        public string Name { get; set; }
        public string OrganizerName { get; set; }
        public string Rating { get; set; }
        public List<PastEventsViewModel> PastEvents { get; set; }
    }
}
