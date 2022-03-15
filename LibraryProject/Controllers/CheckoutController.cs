using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryProject.DAL;
using LibraryProject.Models;
using LibraryProject.Services;

namespace LibraryProject.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private LibraryContext db = new LibraryContext();
        //
        // GET: /Checkout/Summary
        public ActionResult Summary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewBag.Cart = cart.GetCartItems();
            var helperOrder = db.Profiles.Single(p => p.Login == User.Identity.Name).HelperOrders;
            if (helperOrder != null) ViewBag.HelperOrderId = db.Profiles.Single(p => p.Login == User.Identity.Name).HelperOrders.ID;
            return View();
        }
        //
        // POST: /Checkout/Summary
        [HttpPost]
        public ActionResult Summary(FormCollection values)
        {
            var order = new Order();
            if (ModelState.IsValid)
            {
                TryUpdateModel(order);
                order.ProfileID = db.Profiles.Single(p => p.Login == User.Identity.Name).ID;
                order.OrderDate = DateTime.Now;

                var cart = ShoppingCart.GetCart(this.HttpContext);

                ViewBag.Cart = cart.GetCartItems();
                
                db.SaveChanges();

                cart.CreateOrder(order);
            }
            
            return RedirectToAction("Complete",
                    new { id = order.ID });
        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = db.Orders.Any(
                o => o.ID == id &&
                     o.Profile.Login == User.Identity.Name);
            var books = "Twoje zamówienie ma numer " + id + "<br>Zamówiono następujące książki: ";

            var order = db.Orders.Single(o => o.ID == id);
            foreach (var orderDetail in order.OrderDetails)
            {
                books += "<br>" + orderDetail.Book.Title;
            }
            var profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            var helperOrder = db.Profiles.Single(p => p.Login == User.Identity.Name).HelperOrders;
            if(helperOrder != null)
            {
                books += "<br> Zamówienie użytkownika " + helperOrder.Profile.Login + " o numerze " + helperOrder.ID;
                books += "<br> skontaktuj się prosze z właścicielem zamówienia pod numerem " + order.Profile.PhoneNumber;
                books += "<br> Adres: " + order.Profile.FullAdress;
                foreach (var helperOrderDetail in helperOrder.OrderDetails)
                {
                    books += "<br>" + helperOrderDetail.Book.Title;
                }
            }

            books += "<br>Zapraszamy po odbiór.";

            MailSender.SendMail(order.Profile.Login, "Zamowienie numer "+id, books);

            profile.HelperOrders = null;
            db.SaveChanges();

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}