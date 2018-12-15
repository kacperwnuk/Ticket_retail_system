using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Entity;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.ViewModels
{
    public class UserTicketsViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public CardType CardType { get; set; }
        public bool IsActive { get; set; }
        public TicketType TicketType { get; set; }
        public DateTime? ValidFromDate { get; set; }
        public IList<UserTicketsViewModel> Tickets {get; set;}
   
    }
}