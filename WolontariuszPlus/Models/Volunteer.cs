using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Models
{
    public class Volunteer : AppUser
    {
        [StringLength(11)]
        public string PESEL { get; private set; }
        public int Points => VolunteersOnEvent.Where(v => v.VolunteerId == AppUserId).Sum(v => v.PointsReceived); // wyliczalny

        public virtual ICollection<VolunteerOnEvent> VolunteersOnEvent { get; private set; }


        protected Volunteer()
        {
            Initialize();
        }

        public Volunteer(string identityUserId, string firstName, string lastName, string phoneNumber, Address address, string pesel) 
            : base(identityUserId, firstName, lastName, phoneNumber, address)
        {
            Initialize();
            PESEL = pesel;
        }

        private void Initialize()
        {
            VolunteersOnEvent = new List<VolunteerOnEvent>();
        }

        public void Update(string phoneNumber, string city, string street, int buildingNumber, int? apartmentNumber, string postalCode, string PESEL)
        {
            base.Update(phoneNumber, city, street, buildingNumber, apartmentNumber, postalCode);
            this.PESEL = PESEL;
        }
    }
}
