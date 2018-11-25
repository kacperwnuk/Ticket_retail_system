using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketRetailSystem.Models.Entity;

namespace TicketRetailSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var ctx = new RetailContext())
            {
                var stud = new User() { Name = "Bill" , Surname = "Fajny", PersonalId = "69696969669"};

                ctx.Users.Add(stud);
                ctx.SaveChanges();
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}