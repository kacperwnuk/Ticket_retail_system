using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.Models.Entity.DictionaryTypes
{
    public class TicketBindingDictionary
    {
        [Required, Key]
        public TicketBinding TicketBindingId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}