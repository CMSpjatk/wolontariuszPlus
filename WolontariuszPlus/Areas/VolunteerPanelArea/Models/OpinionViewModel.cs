using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.VolunteerPanelArea.Models
{
    public class OpinionViewModel
    {
        [Display(Name = "Opinia")]
        [MaxLength(500, ErrorMessage = "Pole {0} nie może mieć więcej niż {1} znaków")]
        public string Opinion { get; set; }

        [Display(Name = "Ocena")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public char? Rate { get; set; }

        public int EventId { get; set; }
    }
}
