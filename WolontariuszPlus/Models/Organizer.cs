using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Models
{
    public class Organizer : AppUser
    {
        public int CreatedEventsCount => Events.Count; // wyliczalny

        public virtual ICollection<Event> Events { get; private set; }


        protected Organizer()
        {
            Initialize();
        }

        public Organizer(string identityUserId, string firstName, string lastName, string email, string phoneNumber, Address address)
            : base(identityUserId, firstName, lastName, email, phoneNumber, address)
        {
            Initialize();
        }

        private void Initialize()
        {
            Events = new List<Event>();
        }

        public void RemoveEvent(Event @event)
        {
            Events.Remove(@event);
        }

        public void AddEventToOrganizer(Event @event)
        {
            if (@event != null && !Events.Contains(@event))
            {
                Events.Add(@event);
                @event.UpdateOrganizer(this);
            }
        }
    }
}
