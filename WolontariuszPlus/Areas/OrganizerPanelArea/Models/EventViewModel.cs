using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WolontariuszPlus.Models;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Models
{
    public class EventViewModel
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Wymagane punkty")]
        public int RequiredPoints { get; set; }


        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [MaxLength(50, ErrorMessage = "Pole \"{0}\" nie może mieć więcej niż {1} znaków")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [MaxLength(50, ErrorMessage = "Pole \"{0}\" nie może mieć więcej niż {1} znaków")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Display(Name = "Numer budynku")]
        public int BuildingNumber { get; set; }

        [Display(Name = "Numer lokalu")]
        public int? ApartmentNumber { get; set; }

        [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
        [RegularExpression("\\d{2}[-]\\d{3}", ErrorMessage = "Wprowadzony kod pocztowy ma niepoprawny format.")]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        public ICollection<string> Tags { get; set; }
    }
}
