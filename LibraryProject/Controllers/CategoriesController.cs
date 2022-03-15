using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using LibraryProject.DAL;
using LibraryProject.Models;
using System.Data.Entity;

namespace LibraryProject.Controllers
{
    public class CategoriesController : Controller
    {
        private LibraryContext db = new LibraryContext();

        // GET: Categories
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

            var categories = from b in db.Categories
                             select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(b => b.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    categories = categories.OrderByDescending(b => b.Name);
                    break;
                case "Count":
                    categories = categories.OrderBy(b => b.Books.Count);
                    break;
                case "count_desc":
                    categories = categories.OrderByDescending(b => b.Books.Count);
                    break;
                default:
                    categories = categories.OrderBy(b => b.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(categories.ToPagedList(pageNumber, pageSize));
        }
        // GET: Categories/Details/5
        public ActionResult Details(int? id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AuthorSortParm = sortOrder == "Author" ? "author_desc" : "Author";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Category = category;

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var books = from b in category.Books
                        select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString)
                                         || b.Author.FirstName.Contains(searchString)
                                         || b.Author.LastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "Date":
                    books = books.OrderBy(b => b.ReleaseDate);
                    break;
                case "date_desc":
                    books = books.OrderByDescending(b => b.ReleaseDate);
                    break;
                case "Author":
                    books = books.OrderBy(b => b.Author.LastName);
                    break;
                case "author_desc":
                    books = books.OrderByDescending(b => b.Author.LastName);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);

            return View(books.ToPagedList(pageNumber, pageSize));
        }

        // GET: Categories/Create
        [Authorize(Roles = "Obsługa")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Obsługa")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Obsługa")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Author/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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