using System.ComponentModel.DataAnnotations;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Models
{
    public class VolunteerViewModel
    {
        public int VolunteerOnEventId { get; set; }

        [Display(Name = "Imię i nazwisko")]
        public string Name { get; set; }

        [Display(Name = "Ilość zebranych pieniędzy")]
        public double CollectedMoney { get; set; }

        [Display(Name = "Zdobyte punkty")]
        public int ReceivedPoints { get; set; }

        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Oceniony?")]
        public bool IsRated { get; set; }
    }
}