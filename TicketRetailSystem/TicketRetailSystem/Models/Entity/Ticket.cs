using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketRetailSystem.Models.Entity
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public decimal IssuedPrice { get; set; }

        public DateTime? ValidFromDate { get; set; } 

        public DateTime? ValidToDate { get; set; }

        public virtual TransportCard Card {get; set; }

        [Required]
        public virtual TicketType TicketType { get; set; }

        [Required]
        public virtual Transaction Transaction { get; set; }
    }
}