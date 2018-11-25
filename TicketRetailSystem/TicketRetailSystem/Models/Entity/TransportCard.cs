using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.Models.Entity
{
    public class TransportCard
    {
        public int Id { get; set; }

        [Required]
        public CardType CardType { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual List<Ticket> Tickets { get; set; }
    }
}