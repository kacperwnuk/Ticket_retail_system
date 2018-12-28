using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.Models.Entity
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public virtual List<Ticket> Tickets { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }
    }
}