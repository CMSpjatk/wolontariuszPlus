using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WolontariuszPlus.Models;

namespace WolontariuszPlus.Areas.OrganizerPanelArea.Models
{
    public class DeleteEventViewModel
    {
        public int EventId { get; set; }
        public string Name { get; set; }
    }
}
