﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Models
{
    public class DisplayEventViewModel
    {
        public int EventId { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Wymagane punkty")]
        public int RequiredPoints { get; set; }

        public ICollection<string> Tags { get; set; }

        [Display(Name = "Organizator")]
        public string OrganizerName { get; set; }

        [Display(Name = "Adres")]
        public string Address { get; set; }
    }
}