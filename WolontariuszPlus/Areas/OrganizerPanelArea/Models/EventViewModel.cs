using Microsoft.AspNetCore.Http;
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

        [Display(Name = "Nazwa")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [MaxLength(50, ErrorMessage = "Pole \"{0}\" nie może mieć więcej niż {1} znaków")]
        public string Name { get; set; }

        [Display(Name = "Data")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public DateTime Date { get; set; }

        [Display(Name = "Opis")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [MaxLength(500, ErrorMessage = "Pole \"{0}\" nie może mieć więcej niż {1} znaków")]
        public string Description { get; set; }

        [Display(Name = "Wymagane punkty")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Range(1, int.MaxValue, ErrorMessage = "Liczba wymaganych puktów musi być większa od 0")]
        public int RequiredPoints { get; set; }


        [Display(Name = "Miasto")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [MaxLength(50, ErrorMessage = "Pole \"{0}\" nie może mieć więcej niż {1} znaków")]
        public string City { get; set; }

        [Display(Name = "Ulica")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [MaxLength(50, ErrorMessage = "Pole \"{0}\" nie może mieć więcej niż {1} znaków")]
        public string Street { get; set; }

        [Display(Name = "Numer budynku")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} musi być większy od 0")]
        public int BuildingNumber { get; set; }

        [Display(Name = "Numer lokalu")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} musi być większy od 0")]
        public int? ApartmentNumber { get; set; }

        [Display(Name = "Kod pocztowy")]
        [Required(ErrorMessage = "Pole \"{0}\" jest wymagane")]
        [RegularExpression("\\d{2}[-]\\d{3}", ErrorMessage = "Wprowadzony kod pocztowy ma niepoprawny format.")]
        public string PostalCode { get; set; }

        [Display(Name = "Zdjęcie tytułowe")]
        public IFormFile FormFile { get; set; }
        public string ImageRelativePath { get; set; }
        

        public ICollection<string> Tags { get; set; }
    }
}
