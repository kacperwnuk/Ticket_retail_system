using System;
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
            return RedirectToAction("ShowBuyForm");
        }

    }
}