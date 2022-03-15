using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LibraryProject.DAL;
using LibraryProject.Models;
using LibraryProject.Services;
using LibraryProject.ViewModels;

namespace LibraryProject.Controllers
{
    public class ShoppingCartController : Controller
    {
        private LibraryContext db = new LibraryContext();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            var orders = db.Orders.Where(o => o.Profile.Login != User.Identity.Name && o.OrderStatus == Order.OrderStatusEnum.Placed && o.OrderType == Order.OrderTypeEnum.Delivery).ToList();

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                Profile = profile,
                Orders = orders
            };
            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5
        [HttpPost]
        public ActionResult ChangeOrderType(int type)
        {
            Order.OrderTypeEnum orderType = Order.OrderTypeEnum.Pickup;
            switch (type)
            {
                case 1:
                    orderType = Order.OrderTypeEnum.Pickup;
                    break;
                case 2:
                    orderType = Order.OrderTypeEnum.Delivery;
                    break;
            }
           // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.ChangeOrderType(orderType);

            return Json("ok");
        }

        //
        // GET: /Store/AddToCart/5
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedAlbum = db.Books
                .Single(b=> b.ID == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ShoppingCartAddViewModel results = cart.AddToCart(addedAlbum);

            return Json(results);
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult AddHelperOrder(int id)
        {
            var order = db.Orders.Single(o => o.ID == id);
            var profile = db.Profiles.Single(p => p.Login == User.Identity.Name);

            order.HelperProfile = profile;
            profile.HelperOrders = order;
            
            db.SaveChanges();

            return Json("ok");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveHelperOrder()
        {
            var profile = db.Profiles.Single(p => p.Login == User.Identity.Name);

            var order = profile.HelperOrders;
            order.HelperProfile = null;
            profile.HelperOrders = null;

            db.SaveChanges();

            return Json("ok");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string bookName = db.Carts
                .Single(item => item.ID == id).Book.Title;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = "Ksiązka " + Server.HtmlEncode(bookName) +
                    " została usunięta z koszyka.",
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}