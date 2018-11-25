using System.Security.Cryptography.X509Certificates;
using TicketRetailSystem.Models.Entity.DictionaryTypes;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TicketRetailSystem.Models.Entity.RetailContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TicketRetailSystem.Models.Entity.RetailContext";
        }

        protected override void Seed(TicketRetailSystem.Models.Entity.RetailContext context)
        {
            context.CardTypeDictionaries.AddOrUpdate(x => x.CardTypeId,
               new CardTypeDictionary() { CardTypeId = CardType.CityCard, Description = "City Card" },
               new CardTypeDictionary() { CardTypeId = CardType.StudentId, Description = "Student Id card" },
               new CardTypeDictionary() { CardTypeId = CardType.WarsawInhabitant, Description = "Adult warsaw inhabitant" },
               new CardTypeDictionary() { CardTypeId = CardType.YoungInhabitant, Description = "Non adult warsaw inhabitant" }
            );

            context.DiscountTypeDictionaries.AddOrUpdate(x => x.DiscountTypeId,
                new DiscountTypeDictionary() { DiscountTypeId = DiscountType.Normal, Description = "No discount", PercentDiscount = 0 },
                new DiscountTypeDictionary() { DiscountTypeId = DiscountType.School, Description = "Person attending to school", PercentDiscount = 37 },
                new DiscountTypeDictionary() { DiscountTypeId = DiscountType.Student, Description = "Person studying", PercentDiscount = 50 },
                new DiscountTypeDictionary() { DiscountTypeId = DiscountType.Soldier, Description = "Soldier", PercentDiscount = 78 },
                new DiscountTypeDictionary() { DiscountTypeId = DiscountType.InvalidGuide, Description = "Invalid person guide", PercentDiscount = 95 }
            );

            context.PaymentTypeDictionaries.AddOrUpdate(x => x.PaymentTypeId,
               new PaymentTypeDictionary() { PaymentTypeId = PaymentType.Card, Description = "Payment with card" },
               new PaymentTypeDictionary() { PaymentTypeId = PaymentType.Cash, Description = "Payment with cash" },
               new PaymentTypeDictionary() { PaymentTypeId = PaymentType.BankTransfer, Description = "Payment with bank transfer" }
             );

            context.TickedBindingDictionaries.AddOrUpdate(x => x.TicketBindingId,
                new TicketBindingDictionary() { TicketBindingId = TicketBinding.Bearer, Description = "Ticket bound to bearer" },
                new TicketBindingDictionary() { TicketBindingId = TicketBinding.Personal, Description = "Ticket bound to person" }
            );

            context.TicketPeriodDictionaries.AddOrUpdate(x => x.TicketPeriodId,
                new TicketPeriodDictionary() { TicketPeriodId = TicketPeriod.TwentyMinutes, Description = "Twenty minutes" },
                new TicketPeriodDictionary() { TicketPeriodId = TicketPeriod.Hour, Description = "Hour"},
                new TicketPeriodDictionary() { TicketPeriodId = TicketPeriod.FourHours, Description = "Four hours" },
                new TicketPeriodDictionary() { TicketPeriodId = TicketPeriod.Day, Description = "Day" },
                new TicketPeriodDictionary() { TicketPeriodId = TicketPeriod.TwoDays, Description = "Two days" },
                new TicketPeriodDictionary() { TicketPeriodId = TicketPeriod.Month, Description = "Month" },
                new TicketPeriodDictionary() { TicketPeriodId = TicketPeriod.ThreeMonths, Description = "Three months" }
            );


            context.ZoneDictionaries.AddOrUpdate(x => x.ZoneId,
               new ZoneDictionary() { ZoneId = Zone.First, Description = "First zone" },
               new ZoneDictionary() { ZoneId = Zone.Second, Description = "Second zone" },
               new ZoneDictionary() { ZoneId = Zone.FirstAndSecond, Description = "First and second zone" }
            );


        }
    }
}
