using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Models
{
    public class VolunteerOnEvent
    {
        public int VolunteerOnEventId { get; set; }

        public double AmountOfMoneyCollected { get; set; }
        
        public int PointsReceived => (int)(AmountOfMoneyCollected/2); //wyliczalny

        [MaxLength(500)]
        public string OpinionAboutVolunteer { get; set; }

        [MaxLength(500)]
        public string OpinionAboutEvent { get; set; }

        [RegularExpression("[+-]")]
        public char? EventRate { get; set; }

        public int EventId { get; set; }
        public virtual Event Event { get; set; }

        public int? VolunteerId { get; set; }
        public virtual Volunteer Volunteer { get; set; }
    }
}
