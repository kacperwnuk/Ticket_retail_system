using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using TicketRetailSystem.Models.Entity;
using TicketRetailSystem.Models.Enums;
using TicketRetailSystem.ViewModels;

namespace TicketRetailSystem.Controllers
{
    [RoutePrefix("raport")]
    public class TicketController : Controller
    {
        private RetailContext ctx;

        public TicketController()
        {
            ctx = new RetailContext();
        }

        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
        }

        //5
        [Route("people")]
        public ActionResult SearchForAmountOfPeople()
        {
            return View();
        }

        //6
        [Route("ticketType")]
        public ActionResult CountTicketsInZones()
        {
            return View();
        }

        //7
        [Route("transactions")]
        public ActionResult SearchForTransaction()
        {
            return View();
        }

        //8
        [Route("user")]
        public ActionResult SearchByUserId()
        {
            return View();
        }

        //9
        [Route("totalProfit")]
        public ActionResult SearchForProfit()
        {
            return View();
        }


        //pomocnicza - niepotrzebna do projektu
        [Route("ticket/{id}")]
        public ActionResult GetById(int id)
        {
            var ticket = ctx.Tickets.SingleOrDefault(t => t.Id == id);

            if (ticket == null)
            {
                return HttpNotFound();
            }

            return View(ticket);
        }

        //pomocnicza - niepotrzebna do projektu
        [Route("ticket/all")]
        public ActionResult ShowAllTickets()
        {
            var viewModel = new TestowyViewModel(ctx.Tickets.ToList());
            return View(viewModel);
        }

        //pomocnicza - niepotrzebna do projektu
        [Route("transactions/all")]
        public ActionResult GetAllTransactions()
        {
            var transactions = new TestowyViewModel(ctx.Transactions.ToList());
            return View(transactions);
        }

        //5.Użytkownik powinien móc sprawdzić ile osób kupiło bilet danego typu w określonym przez użytkownika przedziale czasu.
        [Route("ticket/people")]
        [HttpPost]
        public ActionResult GetAmountOfPeople(ChosenListViewModel chosenData)
        {
            chosenData.EndTime = chosenData.EndTime.AddHours(23).AddMinutes(59).AddSeconds(59);
            if (chosenData.Zone == null || chosenData.DiscountType == null || chosenData.PaymentType == null)
            {
                return View();
            }
            var findEverything = new EverythingViewModel()
            {
                ChosenData = chosenData,
                DetailedInfo = (from tr in ctx.Transactions
                                join t in ctx.Tickets on tr.Id equals t.Transaction.Id
                                join c in ctx.TransportCards on t.Card.Id equals c.Id
                                join u in ctx.Users on c.User.Id equals u.Id
                                where DateTime.Compare(tr.Date, chosenData.StartTime) >= 0
                                      && DateTime.Compare(tr.Date, chosenData.EndTime) <= 0
                                      && (!chosenData.DiscountType.Any() || chosenData.DiscountType.Contains(t.TicketType.DiscountType))
                                      && (!chosenData.PaymentType.Any() || chosenData.PaymentType.Contains(tr.PaymentType))
                                      && (!chosenData.Zone.Any() || chosenData.Zone.Contains(t.TicketType.Zone))
                                select new DetailedInfoViewModel()
                                {
                                    TransactionId = tr.Id,
                                    PaymentType = tr.PaymentType,
                                    TicketId = t.Id,
                                    TicketIssuedPrice = t.IssuedPrice,
                                    TicketType = t.TicketType,
                                    CardId = c.Id,
                                    UserId = u.Id,
                                    TransactionDate = tr.Date

                                })
                        .Union(from tr in ctx.Transactions
                               join t in ctx.Tickets on tr.Id equals t.Transaction.Id
                               where t.Card == null
                                     && DateTime.Compare(tr.Date, chosenData.StartTime) >= 0
                                     && DateTime.Compare(tr.Date, chosenData.EndTime) <= 0
                                     && (!chosenData.DiscountType.Any() || chosenData.DiscountType.Contains(t.TicketType.DiscountType))
                                     && (!chosenData.PaymentType.Any() || chosenData.PaymentType.Contains(tr.PaymentType))
                                     && (!chosenData.Zone.Any() || chosenData.Zone.Contains(t.TicketType.Zone))
                               select new DetailedInfoViewModel
                               {
                                   TransactionId = tr.Id,
                                   PaymentType = tr.PaymentType,
                                   TicketId = t.Id,
                                   TicketIssuedPrice = t.IssuedPrice,
                                   TicketType = t.TicketType,
                                   CardId = -1,
                                   UserId = -1,
                                   TransactionDate = tr.Date
                               }
                    ).ToList(),

                TotalAmount = (from tr in ctx.Transactions
                               join t in ctx.Tickets on tr.Id equals t.Transaction.Id
                               join c in ctx.TransportCards on t.Card.Id equals c.Id
                               join u in ctx.Users on c.User.Id equals u.Id
                               where DateTime.Compare(tr.Date, chosenData.StartTime) >= 0
                                     && DateTime.Compare(tr.Date, chosenData.EndTime) <= 0
                                     && (!chosenData.DiscountType.Any() || chosenData.DiscountType.Contains(t.TicketType.DiscountType))
                                     && (!chosenData.PaymentType.Any() || chosenData.PaymentType.Contains(tr.PaymentType))
                                     && (!chosenData.Zone.Any() || chosenData.Zone.Contains(t.TicketType.Zone))
                               select t.Id
                    ).Count()
            };


            return View(findEverything);
        }

        //6 Użytkownik powinien móc sprawdzić ile biletów zostało kupionych danego rodzaju(kartonikowy, legitymacja studencka, karta miejska itd.) w określonym przez użytkownika przedziale czasu.
        [Route("ticket/ticketType")]
        [HttpPost]
        public ActionResult GetAmountOfTickets(DatesViewModel dates)
        {
            var groupByTicketTypes = new TicketTypeViewModel()
            {
                TicketZonesWithAmount = (from tr in ctx.Transactions
                                         join t in ctx.Tickets on tr.Id equals t.Transaction.Id
                                         join c in ctx.TransportCards on t.Card.Id equals c.Id
                                         where DateTime.Compare(tr.Date, dates.StartTime) >= 0 &&
                                               DateTime.Compare(tr.Date, dates.EndTime) <= 0
                                         group new { t, c, tr } by c.CardType
                        into types
                                         select new TicketTypeViewModel
                                         {
                                             CardType = types.Key,
                                             AmountOfTickets = types.Count()
                                         }
                    )
                    .Union(from tr in ctx.Transactions
                           join t in ctx.Tickets on tr.Id equals t.Transaction.Id
                           where t.Card == null && DateTime.Compare(tr.Date, dates.StartTime) >= 0 &&
                                 DateTime.Compare(tr.Date, dates.EndTime) <= 0
                           group new { t, tr } by t.Card
                        into types
                           select new TicketTypeViewModel
                           {
                               CardType = CardType.Paper,
                               AmountOfTickets = types.Count()
                           }
                    ).ToList()
            };

            return View(groupByTicketTypes);
        }

        //7 Użytkownik powinien mieć wgląd do historii transakcji w danym dniu.
        [Route("ticket/transactions")]
        [HttpPost]
        public ActionResult GetByDate(DateViewModel date)
        {
            var startDate = date.CurrentDate;
            var endDate = date.CurrentDate.AddDays(1);
            var transactions = new TransactionViewModel()
            {
                Transcations = (from tr in ctx.Transactions
                                join t in ctx.Tickets on tr.Id equals t.Transaction.Id
                                join c in ctx.TransportCards on t.Card.Id equals c.Id
                                join u in ctx.Users on c.User.Id equals u.Id
                                where tr.Date >= startDate && tr.Date <= endDate
                                select new TransactionViewModel
                                {
                                    TransactionId = tr.Id,
                                    TotalPrice = tr.TotalPrice,
                                    PaymentType = tr.PaymentType,
                                    TicketId = t.Id,
                                    TicketIssuedPrice = t.IssuedPrice,
                                    TicketType = t.TicketType,
                                    CardId = c.Id,
                                    UserId = u.Id,
                                    TransactionDate = tr.Date
                                }
                    ).ToList()
            };

            return View(transactions);
        }

        //8 Użytkownik powinien móc sprawdzić jakie bilety kupowała określona         osoba(wybierana poprzez id osoby).
        [Route("ticket/user")]
        [HttpPost]
        public ActionResult GetByUserId(int id)
        {
            var userTickets = new UserTicketsViewModel()
            {
                Tickets = (from u in ctx.Users
                           join c in ctx.TransportCards on u.Id equals c.User.Id
                           join t in ctx.Tickets on c.Id equals t.Card.Id
                           where u.Id == id
                           select new UserTicketsViewModel
                           {
                               UserId = u.Id,
                               UserName = u.Name,
                               UserSurname = u.Surname,
                               CardType = c.CardType,
                               IsActive = c.IsActive,
                               TicketType = t.TicketType,
                               ValidFromDate = t.ValidFromDate
                           }
                    ).ToList()
            };

            return View(userTickets);
        }

        //9 Użytkownik powinien móc sprawdzić łączny zarobek ze sprzedaży biletów w danym przez użytkownika przedziale czasu.
        [Route("ticket/totalProfit")]
        [HttpPost]
        public ActionResult GetTotalCostOfTickets(DatesViewModel dates)
        {
            var totalCost = new ProfitViewModel()
            {
                Profit = ctx.Transactions
                             .Where(el =>
                                 DateTime.Compare(el.Date, dates.StartTime) >= 0 &&
                                 DateTime.Compare(el.Date, dates.EndTime) <= 0).Sum(el => (decimal?)el.TotalPrice) ?? 0,
                StartDate = dates.StartTime,
                EndDate = dates.EndTime
            };

            return View(totalCost);
        }
    }
}