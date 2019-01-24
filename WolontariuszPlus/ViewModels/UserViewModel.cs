using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WolontariuszPlus.Areas.OrganizerPanelArea.Models;

namespace WolontariuszPlus.ViewModels
{
    public class UserViewModel
    {
        public int VolunteerId { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
        [MaxLength(50, ErrorMessage = "Maksymalna długość pola \"{0}\" wynosi {1}")]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "{0} może zawierać jedynie litery.")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
        [MaxLength(50, ErrorMessage = "Maksymalna długość pola \"{0}\" wynosi {1}")]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "{0} może zawierać jedynie litery.")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
        [MaxLength(15, ErrorMessage = "Maksymalna długość pola \"{0}\" wynosi {1}")]
        [Display(Name = "Numer telefonu")]
        [Phone(ErrorMessage = "Wprowadzy numer telefonu jest niepoprawny")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
        [MaxLength(50, ErrorMessage = "Maksymalna długość pola \"{0}\" wynosi {1}")]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Nazwa miasta może zawierać jedynie litery.")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
        [MaxLength(50, ErrorMessage = "Maksymalna długość pola \"{0}\" wynosi {1}")]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Nazwa ulicy może zawierać jedynie litery.")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
        [Display(Name = "Numer budynku")]
        public int BuildingNumber { get; set; }

        [Display(Name = "Numer lokalu")]
        public int? ApartmentNumber { get; set; }

        [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
        [RegularExpression("\\d{2}[-]\\d{3}", ErrorMessage = "Wprowadzony kod pocztowy ma niepoprawny format.")]
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

        // List for volunteer profile

        public List<PastEventsViewModel> PastEventViewModelList { get; set; }
    }
}
