using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Models
{
    public class Organizer : AppUser
    {
        public int CreatedEventsCount { get; set; } // Events.Count // wyliczalny

        public virtual ICollection<Event> Events { get; set; }
    }
}
