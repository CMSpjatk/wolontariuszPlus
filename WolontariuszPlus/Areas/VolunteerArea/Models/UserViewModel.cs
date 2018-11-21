using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.VolunteerArea.Models
{
    public class UserViewModel
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }
        
        [Required]
        [MaxLength(50)]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Numer budynku")]
        public int BuildingNumber { get; set; }

        [Display(Name = "Numer mieszkania")]
        public int ApartmentNumber { get; set; }

        [Required]
        [RegularExpression("\\d{2}[-]\\d{3}")]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }


        // Volunteer
        [StringLength(11)]
        [MinLength(11)]
        public string PESEL { get; set; }

        [Display(Name = "Punkty")]
        public int Points { get; set; }


        // Organizer
        [Display(Name = "Liczba zorganizowanych wydarzeń")]
        public int CreatedEventsCount { get; set; }
       
        // Helper prop
        public bool IsVolunteer { get; set; }
    }
}
