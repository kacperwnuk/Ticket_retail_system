using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Entity;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.ViewModels
{
    public class TransactionViewModel
    {
        public int TransactionId { get; set; }
        public decimal TotalPrice  { get; set; }
        public PaymentType PaymentType { get; set; }
        public int TicketId { get; set; }
        public decimal TicketIssuedPrice { get; set; }
        public TicketType TicketType { get; set; }
        public int CardId { get; set; }
        public int UserId { get; set; }
        public IList<TransactionViewModel> Transcations { get; set; }

    }
}