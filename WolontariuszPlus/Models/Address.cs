using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Models
{
    public class Address
    {
        public int AddressId { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public int BuildingNumber { get; set; }

        public int ApartmentNumber { get; set; }

        public string PostalCode { get; set; }

        public virtual ICollection<AppUser> AppUsers { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
