using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.Models.Entity.DictionaryTypes
{
    public class ZoneDictionary
    {
        [Required, Key]
        public Zone ZoneId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}