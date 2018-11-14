using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WolontariuszPlus.Models
{
    public class Volunteer : AppUser
    {
        public string PESEL { get; set; }
        public int Points { get; set; } // wyliczalny
    }
}
