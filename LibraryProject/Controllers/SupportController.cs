using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using LibraryProject.DAL;
using LibraryProject.Models;
using LibraryProject.Services;
using LibraryProject.ViewModels;

namespace LibraryProject.Controllers
{
    [Authorize(Roles = "Obsługa")]
    public class SupportController : Controller
    {
        private LibraryContext db = new LibraryContext();
        // GET: Support
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Placed = sortOrder == "Placed" ? "placed" : "placed";
            ViewBag.Ready = sortOrder == "Ready" ? "ready" : "ready";
            ViewBag.Rented = sortOrder == "Rented" ? "rented" : "rented";
            ViewBag.Returned = sortOrder == "Returned" ? "returned" : "returned";
            ViewBag.AllOrders = sortOrder == "AllOrders" ? "allorders" : "allorders";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var orders = from b in db.Orders
                select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                int ajdi = int.Parse(searchString);
                orders = orders.Where(b => b.ID == ajdi);

            }

            switch (sortOrder)
            {
                case "placed":
                    orders = orders.Where(b => b.OrderStatus == Order.OrderStatusEnum.Placed).OrderByDescending(b => b.OrderDate);
                    break;
                case "ready":
                    orders = orders.Where(b => b.OrderStatus == Order.OrderStatusEnum.Ready).OrderByDescending(b => b.OrderDate);
                    break;
                case "rented":
                    orders = orders.Where(b => b.OrderStatus == Order.OrderStatusEnum.Rented).OrderByDescending(b => b.OrderDate);
                    break;
                case "returned":
                    orders = orders.Where(b => b.OrderStatus == Order.OrderStatusEnum.Returned).OrderByDescending(b => b.OrderDate);
                    break;
                case "allorders":
                    orders = orders.OrderByDescending(b => b.OrderDate);
                    break;
                default:
                    orders = orders.OrderByDescending(b => b.OrderDate);
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            return View(orders.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult Ready(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = db.Orders.Find(id);
            order.OrderStatus = Order.OrderStatusEnum.Ready;
            var details = order.OrderDetails.ToList();
            foreach (var orderDetail in details)
            {
                orderDetail.DetailStatus = OrderDetail.DetailStatusEnum.Ready;
                var book = db.Books.Single(b => b.ID == orderDetail.Book.ID);
                book.Quantity--;
            }

            var books = "Twoje zamówienie numer " + id + " jest gotowe do odbioru.\nZamówiono następujące książki: ";

            foreach (var orderDetail in order.OrderDetails)
            {
                books += "\n" + orderDetail.Book.Title;
            }

            books += "\nZapraszamy po odbiór.";

            MailSender.SendMail(order.Profile.Login, "Zamowienie numer " + id + " jest gotowe do odbioru", books);

            db.SaveChanges();
            if (order == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Details", new {id});
        }

        public ActionResult Rent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = db.Orders.Find(id);
            order.OrderStatus = Order.OrderStatusEnum.Rented;
            var details = order.OrderDetails.ToList();
            foreach (var orderDetail in details)
            {
                orderDetail.DetailStatus = OrderDetail.DetailStatusEnum.Rented;
            }

            db.SaveChanges();
            if (order == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Details", new { id });
        }

        public ActionResult Return(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var orderDetails = db.OrderDetails.Find(id);
            orderDetails.DetailStatus = OrderDetail.DetailStatusEnum.Returned;
            var book = db.Books.Single(b => b.ID == orderDetails.Book.ID);
            book.Quantity++;

            var order = orderDetails.Order;

            var returnedCount = order.OrderDetails.Count(o => o.DetailStatus == OrderDetail.DetailStatusEnum.Returned);
            if (returnedCount == order.OrderDetails.Count)
            {
                order.OrderStatus = Order.OrderStatusEnum.Returned;
            }

            db.SaveChanges();
            if (orderDetails == null)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Details", new { order.ID });
        }
    }
}