using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Models
{
    public abstract class AppUser
    {
        public int AppUserId { get; private set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; private set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; private set; }

        [Required]
        public string Email { get; private set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; private set; }
        
        [Required]
        public string IdentityUserId { get; private set; }

        public int AddressId { get; private set; }
        public virtual Address Address { get; private set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";


        protected AppUser()
        {
        }

        public AppUser(string identityUserId, string firstName, string lastName, string email, string phoneNumber, Address address) : this()
        {
            IdentityUserId = identityUserId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;

            if (address == null) throw new Exception("Address cannot be null");
            UpdateAddress(address);
        }

        public void UpdateAddress(Address address)
        {
            if (address != null && Address != address)
            {
                if (Address != null)
                {
                    Address.RemoveUser(this);
                }
                Address = address;
                address.AddUserToAddress(this);
            }
        }
        
        public void Update(string phoneNumber, string city, string street, int buildingNumber, int? apartmentNumber, string postalCode)
        {
            PhoneNumber = phoneNumber;
            Address.Update(city, street, buildingNumber, apartmentNumber, postalCode);
        }
    }
}
