using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using LibraryProject.DAL;
using LibraryProject.Models;
using LibraryProject.ViewModels;

namespace LibraryProject.Controllers
{
    public class ProfileController : Controller
    {
        private LibraryContext db = new LibraryContext();

        // GET: Profile
        [Authorize]
        public ActionResult Index()
        {
            var profile = db.Profiles.Single(p => p.Login == User.Identity.Name);

            var viewModel = new ProfileViewModel
            {
                Profile = profile,
                RentedCount = db.OrderDetails.Count(o => o.DetailStatus == OrderDetail.DetailStatusEnum.Rented && o.Order.Profile.Login == profile.Login),
                ReturnedCount = db.OrderDetails.Count(o => o.DetailStatus == OrderDetail.DetailStatusEnum.Returned && o.Order.Profile.Login == profile.Login),
                Placed = db.Orders.Where(o=>o.OrderStatus == Order.OrderStatusEnum.Placed && o.Profile.Login == profile.Login).ToList(),
                Rented = db.OrderDetails.Where(o => o.DetailStatus == OrderDetail.DetailStatusEnum.Rented && o.Order.Profile.Login == profile.Login).ToList(),
                Returned = db.Orders.Where(o => o.OrderStatus == Order.OrderStatusEnum.Returned && o.Profile.Login == profile.Login).ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Edit()
        {
            Profile profile = db.Profiles.Single(p => p.Login == User.Identity.Name);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Author/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Profile profile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profile);
        }
    }
}