using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using TicketRetailSystem.Models.Entity;
using TicketRetailSystem.Models.Enums;
using TicketRetailSystem.ViewModels;

namespace TicketRetailSystem.Controllers
{
    [RoutePrefix("app")]
    public class AppController : Controller
    {
        private RetailContext ctx;

        public AppController()
        {
            ctx = new RetailContext();
        }

        protected override void Dispose(bool disposing)
        {
            ctx.Dispose();
        }

        [Route("buy")]
        public ActionResult ShowBuyForm()
        {
            var buyTicketViewModel = new BuyTicketViewModel
            {
                CardTypes = ctx.CardTypeDictionaries.ToList(),
                DiscountTypes = ctx.DiscountTypeDictionaries.ToList(),
                PaymentTypes = ctx.PaymentTypeDictionaries.ToList(),
                TicketPeriods = ctx.TicketPeriodDictionaries.ToList(),
                Zones = ctx.ZoneDictionaries.ToList()
            };
            return View(buyTicketViewModel);
        }

        [HttpPost]
        public ActionResult BuyTicket(BuyTicketViewModel buyTicketViewModel)
        {
            TempData["ViewModel"] = buyTicketViewModel;
            return RedirectToAction("Checkout", "App");
        }

        public ActionResult Checkout()
        {
            var viewModel = TempData["ViewModel"] as BuyTicketViewModel;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Finalize(BuyTicketViewModel buyTicketViewModel)
        {
            if (!Validate(buyTicketViewModel.TicketType))
                return RedirectToAction("Failure", "App", new MessageViewModel { Message = "Oops%20something%20went%20wrong" });
            List<Ticket> tickets = new List<Ticket>();
            for (int i = 0; i < buyTicketViewModel.NumberOfTickets; ++i)
            {
                tickets.Add(new Ticket
                {
                    TicketType = buyTicketViewModel.TicketType
                });
            }

            var numberFormatInfo = new NumberFormatInfo {NumberDecimalSeparator = "."};
            var transaction = new Transaction
            {
                Date = DateTime.Now,
                TotalPrice = Decimal.Parse("2.0", numberFormatInfo),
                Tickets = tickets,
                PaymentType = buyTicketViewModel.PaymentType
            };
            foreach (var ticket in tickets)
            {
                ticket.Transaction = transaction;
                ctx.Tickets.Add(ticket);
            }

            ctx.Transactions.Add(transaction);

            // ctx.SaveChanges();
            return RedirectToAction("Success", "App");
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failure(MessageViewModel viewModel)
        {
            return View(viewModel);
        }

        private Boolean Validate(TicketType ticketType)
        {
            var type = (from tt in ctx.TicketTypes
                where tt.TicketPeriod == ticketType.TicketPeriod &&
                      tt.DiscountType == ticketType.DiscountType &&
                      tt.Zone == ticketType.Zone && tt.TicketBinding == ticketType.TicketBinding
                select new
                {
                    Id = tt.Id
                }).Count();
//            return type > 0;
            return false;
        }
    }
}