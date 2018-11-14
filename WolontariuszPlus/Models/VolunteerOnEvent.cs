using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Models
{
    public class VolunteerOnEvent
    {
        public int VolunteerOnEventId { get; set; }

        public double AmountOfMoneyCollected { get; set; }

        public int PointsReceived { get; set; } //wyliczlny

        public string OpinionAboutVolunteer { get; set; }

        public string OpinionAboutEvent { get; set; }

        public char EventRate { get; set; }

        public virtual Event Event { get; set; }

        public virtual Volunteer Volunteer { get; set; }
    }
}
