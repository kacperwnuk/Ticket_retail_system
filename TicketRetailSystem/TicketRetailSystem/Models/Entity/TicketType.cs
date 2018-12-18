using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.Models.Entity
{
    public class TicketType
    {
        public int Id { get; set; }

        [Required]
        public TicketPeriod TicketPeriod { get; set; }

        [Required]
        public Zone Zone { get; set; }

        [Required]
        public DiscountType DiscountType { get; set; }

        [Required]
        public TicketBinding TicketBinding { get; set; }
    }
}