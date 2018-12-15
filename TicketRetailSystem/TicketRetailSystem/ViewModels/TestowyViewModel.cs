using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Entity;

namespace TicketRetailSystem.ViewModels
{
    public class TestowyViewModel
    {

        public TestowyViewModel(List<Ticket> ticket)
        {
            Tickets = ticket;
        }


        public TestowyViewModel(IEnumerable<Transaction> transaction )
        {
            Transactions = transaction;
        }

        public List<Ticket> Tickets { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }     


    }
}