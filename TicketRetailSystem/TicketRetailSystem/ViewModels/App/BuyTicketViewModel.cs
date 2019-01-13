using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Entity;
using TicketRetailSystem.Models.Entity.DictionaryTypes;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.ViewModels
{
    public class BuyTicketViewModel
    { 
        public int CardId { get; set; }
        public TicketType TicketType { get; set; }
        public PaymentType PaymentType { get; set; }
        public int NumberOfTickets { get; set; }
        public IEnumerable<CardTypeDictionary> CardTypes { get; set; }
        public IEnumerable<DiscountTypeDictionary> DiscountTypes { get; set; }
        public IEnumerable<PaymentTypeDictionary> PaymentTypes { get; set; }
        public IEnumerable<TicketPeriodDictionary> TicketPeriods { get; set; }
        public IEnumerable<ZoneDictionary> Zones { get; set; }
        public IEnumerable<TicketType> TicketTypes { get; set; }
    }
}