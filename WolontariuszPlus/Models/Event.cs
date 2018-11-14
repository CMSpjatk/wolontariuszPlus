using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Models
{
    public class Event
    {
        public int EventId { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public ICollection<string> Tags { get; set; }

        public int RequiredPoints { get; set; }

        public virtual Organizer Organizer { get; set; }

        public virtual Address Adress { get; set; }

        public virtual ICollection<VolunteerOnEvent> VolunteersOnEvent { get; set; }
    }
}
