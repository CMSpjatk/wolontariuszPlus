using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; private set; }

        [Required]
        [MaxLength(50)]
        public string Street { get; private set; }

        [Required]
        public int BuildingNumber { get; private set; }
        
        public int? ApartmentNumber { get; private set; }

        [Required]
        [RegularExpression("\\d{2}[-]\\d{3}")]
        public string PostalCode { get; private set; }
        
        public virtual ICollection<AppUser> AppUsers { get; private set; }
        public virtual ICollection<Event> Events { get; private set; }


        protected Address()
        {
            AppUsers = new HashSet<AppUser>();
            Events = new List<Event>();
        }

        public Address(string city, string street, int buildingNumber, int? apartmentNumber, string postalCode) : this()
        {
            City = city;
            Street = street;
            BuildingNumber = buildingNumber;
            ApartmentNumber = apartmentNumber;
            PostalCode = postalCode;
        }

        public void AddUserToAddress(AppUser appUser)
        {
            if (appUser != null && !AppUsers.Contains(appUser))
            {
                AppUsers.Add(appUser);
                appUser.UpdateAddress(this);
            }
        }

        public void RemoveUser(AppUser appUser)
        {
            AppUsers.Remove(appUser);
        }

        public void AddEventToAddress(Event @event)
        {
            if (@event != null && !Events.Contains(@event))
            {
                Events.Add(@event);
                @event.UpdateAddress(this);
            }
        }

        public void RemoveEvent(Event newEvent)
        {
            Events.Remove(newEvent);
        }

        public void Update(string city, string street, int buildingNumber, int? apartmentNumber, string postalCode)
        {
            if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(street) || string.IsNullOrEmpty(postalCode) || buildingNumber <= 0)
            {
                throw new ArgumentException("Error in update method of Address");
            }

            City = city;
            Street = street;
            BuildingNumber = buildingNumber;
            ApartmentNumber = apartmentNumber;
            PostalCode = postalCode;
        }
    }
}
