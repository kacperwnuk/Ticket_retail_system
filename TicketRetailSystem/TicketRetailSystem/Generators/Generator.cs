using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TicketRetailSystem.Models.Entity;
using TicketRetailSystem.Models.Enums;

namespace TicketRetailSystem.Generators
{
    public class Generator
    {
        static Random random = new Random();
        private RetailContext context = new RetailContext();

        //
        // Tools
        //

        private static string RandomString(int length, string poolOfChars = "abcdefghijklmnopqrstuvwxyz0123456789")
        {
            string pool = poolOfChars;
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[random.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }

        private double RandomDouble(double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        private void GenerateCardWithRandomUser()
        {
            var users = context.Users.ToArray();
            var randomUser = (User)users[random.Next(users.Length)];
            var card = new TransportCard() { CardType = CardType.YoungInhabitant, IsActive = true, User = randomUser };
            context.TransportCards.Add(card);
        }

        //
        // Generators
        //

        private void GenerateUsersAndCards(int howMany = 1)
        {
            string[] names = { "Jan", "Andrzej", "Piotr", "Krzysztof", "Stanisław", "Tomasz", "Paweł", "Józef", "Marcin", "Marek", "Michał", "Grzegorz", "Jerzy", "Tadeusz", "Adam", "Łukasz", "Zbigniew", "Ryszard", "Dariusz", "Henryk", "Mariusz", "Kazimierz", "Wojciech", "Robert", "Mateusz", "Marian", "Rafał", "Jacek", "Janusz", "Mirosław", "Maciej", "Sławomir", "Jarosław", "Kamil", "Wiesław", "Roman", "Władysław", "Jakub", "Artur", "Zdzisław", "Edward", "Mieczysław", "Damian", "Dawid", "Przemysław", "Sebastian", "Czesław", "Leszek", "Daniel", "Waldemar", "Anna", "Maria", "Katarzyna", "Małgorzata", "Agnieszka", "Krystyna", "Barbara", "Ewa", "Elżbieta", "Zofia", "Janina", "Teresa", "Joanna", "Magdalena", "Monika", "Jadwiga", "Danuta", "Irena", "Halina", "Helena", "Beata", "Aleksandra", "Marta", "Dorota", "Marianna", "Grażyna", "Jolanta", "Stanisława", "Iwona", "Karolina", "Bożena", "Urszula", "Justyna", "Renata", "Alicja", "Paulina", "Sylwia", "Natalia", "Wanda", "Agata", "Aneta", "Izabela", "Ewelina", "Marzena", "Wiesława", "Genowefa", "Patrycja", "Kazimiera", "Edyta", "Stefania" };
            string[] lastNames = { "Nowak", "Kowalski", "Wiśniewski", "Dąbrowski", "Lewandowski", "Wójcik", "Kamiński", "Kowalczyk", "Zieliński", "Szymański", "Woźniak", "Kozłowski", "Jankowski", "Wojciechowski", "Kwiatkowski", "Kaczmarek", "Mazur", "Krawczyk", "Piotrowski", "Grabowski", "Nowakowski", "Pawłowski", "Michalski", "Nowicki", "Adamczyk", "Dudek", "Zając", "Wieczorek", "Jabłoński", "Król", "Majewski", "Olszewski", "Jaworski", "Wróbel", "Malinowski", "Pawlak", "Witkowski", "Walczak", "Stępień", "Górski", "Rutkowski", "Michalak", "Sikora", "Ostrowski", "Baran", "Duda", "Szewczyk", "Tomaszewski", "Pietrzak", "Marciniak", "Wróblewski", "Zalewski", "Jakubowski", "Jasiński", "Zawadzki", "Sadowski", "Bąk", "Chmielewski", "Włodarczyk", "Borkowski", "Czarnecki", "Sawicki", "Sokołowski", "Urbański", "Kubiak", "Maciejewski", "Szczepański", "Kucharski", "Wilk", "Kalinowski", "Lis", "Mazurek", "Wysocki", "Adamski", "Kaźmierczak", "Wasilewski", "Sobczak", "Czerwiński", "Andrzejewski", "Cieślak", "Głowacki", "Zakrzewski", "Kołodziej", "Sikorski", "Krajewski", "Gajewski", "Szymczak", "Szulc", "Baranowski", "Laskowski", "Brzeziński", "Makowski", "Ziółkowski", "Przybylski" };

            for (int i = 0; i < howMany; i++)
            {
                string randomName = names[random.Next(names.Length)];
                string randomLastName = lastNames[random.Next(lastNames.Length)];
                string randomId = RandomString(10, "0123456789");
                var user = new User() { Name = randomName, Surname = randomLastName, PersonalId = randomId };

                Array values = Enum.GetValues(typeof(CardType));
                CardType randomCardType = (CardType)values.GetValue(random.Next(values.Length));
                bool randomIsActive = (random.Next(2) == 0);

                var card = new TransportCard() { CardType = randomCardType, IsActive = randomIsActive, User = user };

                context.Users.Add(user);
                context.TransportCards.Add(card);
            }
        }

        private void UpdateTicketTypes()
        {
            var ticketTypesQuery = from tt in context.TicketTypes select tt;
            var ticketTypes = ticketTypesQuery.ToArray();

            foreach (TicketPeriod period in Enum.GetValues(typeof(TicketPeriod)))
            {
                foreach (Zone zone in Enum.GetValues(typeof(Zone)))
                {
                    foreach (DiscountType discountType in Enum.GetValues(typeof(DiscountType)))
                    {
                        bool found = false;
                        foreach (TicketType tt in ticketTypes)
                        {
                            if (tt.TicketPeriod == period && tt.Zone == zone && tt.DiscountType == discountType)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            context.TicketTypes.Add(new TicketType()
                            {
                                TicketPeriod = period,
                                Zone = zone,
                                DiscountType = discountType
                            }
                            );
                        }
                    }
                }
            }
        }

        private void GenerateTicketTypes()
        {
            // wszystkie strefy i wszystkie okresy (tylko ulgowe i normalne)
            foreach (TicketPeriod period in Enum.GetValues(typeof(TicketPeriod)))
            {
                foreach (Zone zone in Enum.GetValues(typeof(Zone)))
                {
                    context.TicketTypes.Add(new TicketType()
                    {
                        TicketPeriod = period,
                        Zone = zone,
                        DiscountType = DiscountType.School
                    });

                    context.TicketTypes.Add(new TicketType()
                    {
                        TicketPeriod = period,
                        Zone = zone,
                        DiscountType = DiscountType.Normal
                    });
                }

            }

            // bilety długookresowe (tylko kombatanci, przewodnicy i studeni)
            DiscountType[] discounts = { DiscountType.Soldier, DiscountType.Student, DiscountType.InvalidGuide };
            TicketPeriod[] periods = { TicketPeriod.Month, TicketPeriod.ThreeMonths };
            foreach (DiscountType discount in discounts)
            {
                foreach (Zone zone in Enum.GetValues(typeof(Zone)))
                {
                    foreach (TicketPeriod period in periods)
                    {
                        context.TicketTypes.Add(new TicketType()
                        {
                            TicketPeriod = period,
                            Zone = zone,
                            DiscountType = discount
                        });
                    }
                }
            }

        }

        private void GenerateTransactionsWithPaperTickets(int howMany, int maxTicketsInTransaction, DateTime start, DateTime end)
        {
            Array paymentTypes = Enum.GetValues(typeof(PaymentType));
            var ticketTypesQuery = from tt in context.TicketTypes
                                   where tt.TicketPeriod != TicketPeriod.ThreeMonths
                                   && tt.TicketPeriod != TicketPeriod.Month
                                   select tt;
            var ticketTypes = ticketTypesQuery.ToArray();

            for (int i = 0; i < howMany; i++)
            {
                // create transaction
                TimeSpan dt = end - start;
                TimeSpan timeOfDay = new TimeSpan(random.Next(7, 15), random.Next(60), random.Next(60));
                DateTime transactionDate = start + new TimeSpan(random.Next(dt.Days), 0, 0, 0) + timeOfDay;

                Transaction transaction = new Transaction()
                {
                    PaymentType = (PaymentType)paymentTypes.GetValue(random.Next(paymentTypes.Length)),
                    Tickets = new List<Ticket>(),
                    Date = transactionDate,
                    TotalPrice = 0.0M
                };

                // pick how many tickets were bought during this transaction
                int ticketsInTransaction = random.Next(maxTicketsInTransaction);
                if (ticketsInTransaction == 0)
                {
                    ticketsInTransaction = 1;
                }

                // pick a random short-term ticket type
                TicketType randomTicketType = (TicketType)ticketTypes[random.Next(ticketTypes.Length)];

                // add appropriate number of tickets to this transation
                for (int j = 0; j < ticketsInTransaction; j++)
                {
                    Ticket ticket = new Ticket()
                    {
                        IssuedPrice = (decimal)RandomDouble(1.40, 3.90),
                        ValidFromDate = null,
                        ValidToDate = null,
                        Card = null,
                        TicketType = randomTicketType,
                    };
                    transaction.TotalPrice += ticket.IssuedPrice;
                    transaction.Tickets.Add(ticket);
                }
                context.Transactions.Add(transaction);
            }
        }

        private void GenerateCardTickets(int howManyPerCard, DateTime end)
        {
            //var ticketTypes = context.TicketTypes.ToArray();
            var ticketTypesQuery = from tt in context.TicketTypes
                                   where tt.TicketPeriod == TicketPeriod.ThreeMonths
                                   || tt.TicketPeriod == TicketPeriod.Month
                                   select tt;
            var ticketTypes = ticketTypesQuery.ToArray();

            // add tickets to each card
            List<TransportCard> cards = context.TransportCards.ToList();
            foreach (var card in cards)
            {
                // pick a ticket type for this card
                TicketType randomTicketType = (TicketType)ticketTypes[random.Next(ticketTypes.Length)];

                TimeSpan ticketTimeSpan;
                int days = 0;
                if (randomTicketType.TicketPeriod == TicketPeriod.Month)
                    days = 30;
                else if (randomTicketType.TicketPeriod == TicketPeriod.ThreeMonths)
                    days = 90;
                else
                    days = 5;
                ticketTimeSpan = new TimeSpan(days, 0, 0, 0);

                // create tickets assigned to this card
                DateTime validFromDate;
                DateTime validToDate = end + ticketTimeSpan;
                for (int i = 0; i < howManyPerCard; i++)
                {
                    // create proper valid dates for this ticket
                    TimeSpan timeOfDay = new TimeSpan(random.Next(7, 15), random.Next(60), random.Next(60));
                    validToDate = validToDate - ticketTimeSpan + timeOfDay;
                    validToDate = validToDate - new TimeSpan(1 + random.Next(10), 0, 0, 0);
                    validFromDate = validToDate - ticketTimeSpan;
                    DateTime transactionDate = validFromDate;

                    // create transaction for this ticket on this card
                    Array paymentTypes = Enum.GetValues(typeof(PaymentType));
                    Transaction transaction = new Transaction()
                    {
                        PaymentType = (PaymentType)paymentTypes.GetValue(random.Next(paymentTypes.Length)),
                        Tickets = new List<Ticket>(),
                        Date = transactionDate,
                        TotalPrice = 0.0M
                    };

                    decimal price = days * 2.50M;
                    // finally create a ticket
                    Ticket ticket = new Ticket()
                    {
                        IssuedPrice = price,
                        ValidFromDate = validFromDate,
                        ValidToDate = validToDate,
                        Card = card,
                        TicketType = randomTicketType
                    };

                    // add created ticket to the appropriate transaction
                    transaction.Tickets.Add(ticket);
                    transaction.TotalPrice += price;

                    // add transaction associated with this ticket
                    context.Transactions.Add(transaction);
                }
            }
        }

        //
        // Public members
        //

        public void Run()
        {
            /*
            UpdateTicketTypes(); context.SaveChanges();
            
            GenerateUsersAndCards(500); context.SaveChanges();

            GenerateTicketTypes(); context.SaveChanges();

            for(int i = 0; i < 10; i++)
            {
                GenerateTransactionsWithPaperTickets(
                    100,
                    5,
                    new DateTime(2017, 1, 1),
                    new DateTime(2018, 11, 11)
                );
                context.SaveChanges();
            }

            GenerateCardTickets(5, new DateTime(2018, 11, 11)); context.SaveChanges();
            */
        }
    }
}