using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using TicketRetailSystem.Models.Entity;
using TicketRetailSystem.Models.Entity.DictionaryTypes;
using TicketRetailSystem.Models.Enums;
using TicketRetailSystem.ViewModels;
using static System.Boolean;

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
        public ActionResult ShowBuyForm(String cardId)
        {
            int cardIdNumber;
            
            var buyTicketViewModel = new BuyTicketViewModel
            {
                CardTypes = ctx.CardTypeDictionaries.ToList(),
                DiscountTypes = ctx.DiscountTypeDictionaries.ToList(),
                PaymentTypes = ctx.PaymentTypeDictionaries.ToList(),
                Zones = ctx.ZoneDictionaries.ToList(),
                TicketTypes = ctx.TicketTypes.ToList()
            };
            if (cardId != null)
            {
                try
                {
                    cardIdNumber = Int32.Parse(cardId);
                    buyTicketViewModel.CardId = cardIdNumber;
                    buyTicketViewModel.TicketPeriods = (from p in ctx.TicketPeriodDictionaries
                        where p.TicketPeriodId == TicketPeriod.Month || p.TicketPeriodId == TicketPeriod.ThreeMonths
                        select new TicketPeriodDto
                        {
                            TicketPeriodId = p.TicketPeriodId,
                            Description = p.Description
                        }).ToList();
                }
                catch (FormatException)
                {
                    buyTicketViewModel.CardId =  -1;
                    buyTicketViewModel.TicketPeriods = (from p in ctx.TicketPeriodDictionaries
                        select new TicketPeriodDto
                        {
                            TicketPeriodId = p.TicketPeriodId,
                            Description = p.Description
                        }).ToList();
                }
            }
            else
            {
                buyTicketViewModel.CardId = -1;
                buyTicketViewModel.TicketPeriods = (from p in ctx.TicketPeriodDictionaries
                    select new TicketPeriodDto
                    {
                        TicketPeriodId = p.TicketPeriodId,
                        Description = p.Description
                    }).ToList();
            }

            
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
            var price = GetPrice(buyTicketViewModel.TicketType);
            var numberOfTickets = buyTicketViewModel.NumberOfTickets;
            var cardId = buyTicketViewModel.CardId;
            var card = cardId != -1;
            var travelCard = GetCard(cardId);
            if (price == null || (card && travelCard == null && numberOfTickets != 1) ||
                numberOfTickets < 1 || numberOfTickets > 10)
            {
                var message = HttpUtility.UrlEncode("Oops something went wrong", new UTF8Encoding());
                return RedirectToAction("Failure", "App", new MessageViewModel {Message = message});
            }

            List<Ticket> tickets = new List<Ticket>();
            if (!card)
            {
                for (var i = 0; i < numberOfTickets; ++i)
                {
                    tickets.Add(new Ticket
                    {
                        TicketType = ctx.TicketTypes.Find(buyTicketViewModel.TicketType.Id),
                        IssuedPrice = price.Value * numberOfTickets
                    });
                }
            }
            else
            {
                var lastTicket = GetLastTicketView(cardId);
                var validFromDate = DateTime.Now;
                if (lastTicket != null && lastTicket.Date > validFromDate)
                {
                    validFromDate = lastTicket.Date;
                }

                tickets.Add(new Ticket
                {
                    TicketType = ctx.TicketTypes.Find(buyTicketViewModel.TicketType.Id),
                    IssuedPrice = price.Value * numberOfTickets,
                    ValidFromDate = validFromDate,
                    ValidToDate =
                        validFromDate.AddMinutes(
                            SuperDuperDumbDictionary[buyTicketViewModel.TicketType.TicketPeriod.ToString()]),
                    Card = travelCard
                });
            }

            var transaction = new Transaction
            {
                Date = DateTime.Now,
                TotalPrice = price.Value * numberOfTickets,
                Tickets = tickets,
                PaymentType = buyTicketViewModel.PaymentType
            };
            foreach (var ticket in tickets)
            {
                ticket.Transaction = transaction;
                ctx.Tickets.Add(ticket);
            }

            ctx.Transactions.Add(transaction);

            ctx.SaveChanges();

            List<int> boughtTickets = new List<int>();
            foreach (var ticket in tickets)
            {
                boughtTickets.Add(ticket.Id);
            }
            TempData["BoughtTickets"] = boughtTickets;
            return RedirectToAction("Success", "App");
        }

        public ActionResult Success()
        {
            var boughtTickets = TempData["BoughtTickets"] as List<int>;
            var viewModel = new BoughtTicketsViewModel
            {
                TicketId = boughtTickets
            };
            return View(viewModel);
        }

        public ActionResult Failure(MessageViewModel viewModel)
        {
            return View(viewModel);
        }

        public ActionResult BuyETicket()
        {
            var buyTicketViewModel = new BuyTicketViewModel
            {
                CardTypes = ctx.CardTypeDictionaries.ToList(),
                DiscountTypes = ctx.DiscountTypeDictionaries.ToList(),
                PaymentTypes = ctx.PaymentTypeDictionaries.ToList(),
                TicketPeriods = (from p in ctx.TicketPeriodDictionaries
                                 where p.TicketPeriodId == TicketPeriod.Month || p.TicketPeriodId == TicketPeriod.ThreeMonths
                                 select new TicketPeriodDto
                                 {
                                     TicketPeriodId = p.TicketPeriodId,
                                     Description = p.Description
                                 }).ToList(),
                Zones = ctx.ZoneDictionaries.ToList(),
                TicketTypes = ctx.TicketTypes.ToList()
            };
            return View(buyTicketViewModel);
        }

        public class TicketPeriodDto
        {
            public TicketPeriod TicketPeriodId { get; set; }
            public string Description { get; set; }
        }

        [HttpGet]
        public JsonResult CheckIfCardExists(string cardId)
        {
            int cardIdNumber;
            try
            {
                cardIdNumber = Int32.Parse(cardId);
            }
            catch (FormatException e)
            {
                return Json(FalseString, JsonRequestBehavior.AllowGet);
            }

            return Json(GetCard(cardIdNumber) != null ? TrueString : FalseString, JsonRequestBehavior.AllowGet);
        }

        TransportCard GetCard(int cardId)
        {
            //            var cardCount = (from c in ctx.TransportCards
            //                where c.Id == cardId
            //                select new
            //                {
            //                    Id = c.Id
            //                }).Count();
            //            return cardCount == 1;
            return ctx.TransportCards.Find(cardId);
        }

        [HttpGet]
        public JsonResult GetLastTicket(string cardId)
        {
            int cardIdNumber;
            try
            {
                cardIdNumber = Int32.Parse(cardId);
            }
            catch (FormatException e)
            {
                return Json(FalseString, JsonRequestBehavior.AllowGet);
            }

            var ticketView = GetLastTicketView(cardIdNumber);
            ticketView.DateString = ticketView.Date.ToString(new System.Globalization.CultureInfo("pl-PL"));

            return Json(ticketView, JsonRequestBehavior.AllowGet);
        }

        class TicketView
        {
            public DateTime Date { get; set; }
            public String DateString { get; set; }
            public String Zone { get; set; }
            public String DiscountType { get; set; }
            public String TicketPeriod { get; set; }
            public int TicketTypeId { get; set; }
        }

        TicketView GetLastTicketView(int cardId)
        {
            var ticket = (from t in ctx.Tickets
                where t.Card.Id == cardId && t.ValidToDate != null
                select new TicketView
                {
                    Date = t.ValidToDate ?? DateTime.MinValue,
                    Zone = t.TicketType.Zone.ToString(),
                    DiscountType = t.TicketType.DiscountType.ToString(),
                    TicketPeriod = t.TicketType.TicketPeriod.ToString(),
                    TicketTypeId = t.TicketType.Id
                }).OrderByDescending(a => a.Date).FirstOrDefault();
            return ticket;
        }

        private Price GetPrice(TicketType ticketType)
        {
            var price = (from tt in ctx.TicketTypes
                where tt.TicketPeriod == ticketType.TicketPeriod &&
                      tt.DiscountType == ticketType.DiscountType &&
                      tt.Zone == ticketType.Zone && tt.TicketBinding == ticketType.TicketBinding
                select new Price
                {
                    Value = tt.TicketPrice
                }).FirstOrDefault();
            return price;
        }

        class Price
        {
            public Decimal Value { get; set; }
        }

        private Dictionary<String, int> SuperDuperDumbDictionary = new Dictionary<String, int>()
        {
            {
                "TwentyMinutes", 20
            },

            {
                "Hour", 60
            },

            {"FourHours", 240},
            {"Day", 24 * 60},
            {
                "TwoDays", 48 * 60
            },

            {
                "Month", 30 * 24 * 60
            },

            {
                "ThreeMonths", 90 * 24 * 60
            }
        };
    }
}