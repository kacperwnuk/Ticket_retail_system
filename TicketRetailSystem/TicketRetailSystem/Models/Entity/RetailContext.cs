using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Entity.DictionaryTypes;

namespace TicketRetailSystem.Models.Entity
{
    public class RetailContext : DbContext
    {

        public RetailContext() : base("name=TicketRetailDBConnectionString")
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketType> TicketTypes { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<TransportCard> TransportCards { get; set; }

        public DbSet<CardTypeDictionary> CardTypeDictionaries { get; set; }

        public DbSet<DiscountTypeDictionary> DiscountTypeDictionaries { get; set; }

        public DbSet<PaymentTypeDictionary> PaymentTypeDictionaries { get; set; }

        public DbSet<TicketBindingDictionary> TickedBindingDictionaries { get; set; }

        public DbSet<TicketPeriodDictionary> TicketPeriodDictionaries { get; set; }

        public DbSet<ZoneDictionary> ZoneDictionaries { get; set; }
    }
}