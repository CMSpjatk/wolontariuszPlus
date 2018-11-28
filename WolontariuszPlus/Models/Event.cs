using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Models
{
    public class Event
    {
        public int EventId { get; private set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; private set; }

        [Required]
        public DateTime Date { get; private set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; private set; }

        [Required]
        public int RequiredPoints { get; private set; }

        public ICollection<string> Tags { get; private set; }

        public int? OrganizerId { get; private set; }
        public virtual Organizer Organizer { get; private set; }

        public int AddressId { get; private set; }
        public virtual Address Address { get; private set; }

        public virtual ICollection<VolunteerOnEvent> VolunteersOnEvent { get; private set; }

        [NotMapped]
        public double CollectedMoney => VolunteersOnEvent.Sum(ve => ve.AmountOfMoneyCollected);
        [NotMapped]
        public ICollection<Volunteer> Volunteers => VolunteersOnEvent.Select(voe => voe.Volunteer).ToList();

        protected Event()
        {
            VolunteersOnEvent = new List<VolunteerOnEvent>();
        }

        public Event(string name, DateTime date, string description, int requiredPoints, ICollection<string> tags, Organizer organizer, Address address) 
            : this()
        {
            Name = name;
            Date = date;
            Description = description;
            RequiredPoints = requiredPoints;
            Tags = tags;

            if (organizer == null) throw new Exception("Organizer cannot be null");
            UpdateOrganizer(organizer);

            if (address == null) throw new Exception("Address cannot be null");
            UpdateAddress(address);
        }

        public void UpdateOrganizer(Organizer organizer)
        {
            if (organizer != null && Organizer != organizer)
            {
                if (Organizer != null)
                {
                    Organizer.RemoveEvent(this);
                }
                Organizer = organizer;
                organizer.AddEventToOrganizer(this);
            }
        }

        public void UpdateAddress(Address address)
        {
            if (address != null && Address != address)
            {
                if (Address != null)
                {
                    Address.RemoveEvent(this);
                }
                Address = address;
                address.AddEventToAddress(this);
            }
        }
    }
}
