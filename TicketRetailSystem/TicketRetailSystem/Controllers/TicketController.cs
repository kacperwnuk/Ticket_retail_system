using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketRetailSystem.Models.Entity;
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
        
        // GET: Ticket
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

        [Route("ticket/all")]
        public ActionResult ShowAll()
        {

            var viewModel = new TestowyViewModel(ctx.Tickets.ToList());

            return View(viewModel);
        }


        [Route("ticket/ticketType/{id}")]
        public ActionResult GetByTicketType(int id)
        {
            //var tickets = new TestowyViewModel(ctx.Tickets.Where(ticket => ticket.TicketType.Id == id));
            var tickets = 1;
            return View(tickets);
        }

        //7 Użytkownik powinien mieć wgląd do historii transakcji w danym dniu.
        [Route("ticket/transactions/{date:datetime}")]
        public ActionResult GetByDate(DateTime date)
        {

            TransactionViewModel transactions = new TransactionViewModel();

            transactions.Transcations = ( from tr in ctx.Transactions
                                          join t in ctx.Tickets on tr.Id equals t.Transaction.Id
                                          join c in ctx.TransportCards on t.Card.Id equals c.Id
                                          join u in ctx.Users on c.User.Id equals u.Id
                                          where tr.Date == date
                                          select new TransactionViewModel
                                          {
                                              TransactionId = tr.Id,
                                              TotalPrice = tr.TotalPrice,
                                              PaymentType = tr.PaymentType,
                                              TicketId = t.Id,
                                              TicketIssuedPrice = t.IssuedPrice,

                                          }
                ).ToList();


            return Content(date.ToString());
        }

        //6 Użytkownik powinien móc sprawdzić ile biletów zostało kupionych danego rodzaju(kartonikowy, legitymacja studencka, karta miejska itd.) w określonym przez użytkownika przedziale czasu.
        [Route("ticket/ticketType/{from:datetime}/{to:datetime}")]
        public ActionResult GetAmountOfTickets()
        {
            return View();
        }

        [Route("transactions")]
        public ActionResult GetAllTransactions()
        {
            var transactions = new TestowyViewModel(ctx.Transactions.ToList());
            return View(transactions);
        }

        //9 Użytkownik powinien móc sprawdzić łączny zarobek ze sprzedaży biletów w danym przez użytkownika przedziale czasu.
        [Route("ticket/totalCost/{from:datetime}/{to:datetime}")]
        public ActionResult GetTotalCostOfTickets(DateTime from, DateTime to)
        {
            var totalCost = ctx.Transactions.Where(el => DateTime.Compare(el.Date, from) >= 0 && DateTime.Compare(el.Date, to) <= 0);
            return Content(totalCost.First().ToString());
        }

        //8 Użytkownik powinien móc sprawdzić jakie bilety kupowała określona         osoba(wybierana poprzez id osoby).
        [Route("ticket/user/{id}")]
        public ActionResult GetByUserId(int id)
        {
            UserTicketsViewModel userTickets = new UserTicketsViewModel();

            userTickets.Tickets = (from u in ctx.Users
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
                              }).ToList();

                              
            return View(userTickets);
        }


    }
}