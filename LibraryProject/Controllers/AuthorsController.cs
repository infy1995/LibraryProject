using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using LibraryProject.DAL;
using LibraryProject.Models;

namespace LibraryProject.Controllers
{
    public class AuthorsController : Controller
    {
        private LibraryContext db = new LibraryContext();

        // GET: Author
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.Count = sortOrder == "Count" ? "count_desc" : "Count";
            ViewBag.Categories = db.Categories.ToList();

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var authors = from b in db.Author
                          select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                authors = authors.Where(b => b.FirstName.Contains(searchString)
                                         || b.LastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    authors = authors.OrderByDescending(b => b.LastName);
                    break;
                case "Count":
                    authors = authors.OrderBy(b => b.Books.Count);
                    break;
                case "count_desc":
                    authors = authors.OrderByDescending(b => b.Books.Count);
                    break;
                default:
                    authors = authors.OrderBy(b => b.LastName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(authors.ToPagedList(pageNumber, pageSize));
        }

        // GET: Author/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Author.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Author/Create
        [Authorize(Roles = "Obsługa")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Author/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Author author, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    author.Image = System.Guid.NewGuid().ToString() + ".jpg";
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + author.Image);
                }
                db.Author.Add(author);
                db.SaveChanges();
                return RedirectToAction("Details", new { author.ID });
            }

            return View(author);
        }

        // GET: Author/Edit/5
        [Authorize(Roles = "Obsługa")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Author.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Author/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Description")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Author/Delete/5
        [Authorize(Roles = "Obsługa")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Author.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Author/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Author.Find(id);
            db.Author.Remove(author);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
