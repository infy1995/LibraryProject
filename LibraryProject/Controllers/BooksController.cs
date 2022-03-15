using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using LibraryProject.DAL;
using LibraryProject.Models;
using PagedList;
using LibraryProject.ViewModels;
using ActionResult = System.Web.Mvc.ActionResult;
using Controller = System.Web.Mvc.Controller;
using JsonResult = System.Web.Mvc.JsonResult;

namespace LibraryProject.Controllers
{
    public class BooksController : Controller
    {
        private LibraryContext db = new LibraryContext();

        // GET: Books
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AuthorSortParm = sortOrder == "Author" ? "author_desc" : "Author";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.Categories = db.Categories.ToList();

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var books = from b in db.Books
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

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Obsługa")]
        public ActionResult Create()
        {
            var model = new CreateBookViewModel
            {
                AuthorsCollection = db.Author.ToList(),
                PublishersCollection = db.Publishers.ToList(),
                CategoriesCollection = db.Categories.ToList()
            };

            return View(model);
        }

        // POST: Books/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public ActionResult Create(CreateBookViewModel viewModel)
        {
            if (db.Books.Any(b => b.Isbn == viewModel.Isbn))
            {
                viewModel.AuthorsCollection = db.Author.ToList();
                viewModel.PublishersCollection = db.Publishers.ToList();
                viewModel.CategoriesCollection = db.Categories.ToList();
                ModelState.AddModelError("Isbn", "Książka o takim isbn istnieje.");
                return View(viewModel);
            }
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["fileImage"];
                if (file != null && file.ContentLength > 0)
                {
                    viewModel.Image = System.Guid.NewGuid().ToString()+".jpg";
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + viewModel.Image);
                }

                viewModel.Author = db.Author.First(a => a.ID == viewModel.SelectedAuthorID);
                viewModel.Publisher = db.Publishers.First(a => a.ID == viewModel.SelectedPublisherID);

                viewModel.Categories = new List<Category>();
                foreach (var i in viewModel.SelectedCategoryID)
                {
                    viewModel.Categories.Add(db.Categories.Single(a => a.ID == i));
                }
                
                db.Books.Add(viewModel.GetBook());
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Obsługa")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            List<int> categories = new List<int>();
            foreach (var bookCategory in book.Categories)
            {
                categories.Add(bookCategory.ID);
            }
            var model = new CreateBookViewModel
            {
                AuthorsCollection = db.Author.ToList(),
                PublishersCollection = db.Publishers.ToList(),
                CategoriesCollection = db.Categories.ToList(),
                SelectedCategoryID = categories.ToList(),
                ID = book.ID,
                Isbn = book.Isbn,
                Title = book.Title,
                Description = book.Description,
                ReleaseDate = book.ReleaseDate,
                Quantity = book.Quantity,
                Author = book.Author,
                Categories = book.Categories,
                Publisher = book.Publisher
            };
            return View(model);
        }

        // POST: Books/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public ActionResult Edit(CreateBookViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Book book = db.Books.Single(b => b.ID == viewModel.ID);
                UpdateModel(book);
                
                HttpPostedFileBase file = Request.Files["fileImage"];
                if (file != null && file.ContentLength > 0)
                {
                    book.Image = System.Guid.NewGuid().ToString() + ".jpg";
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + book.Image);
                }

                if(book.Author == null)
                    book.Author = db.Author.First(a => a.ID == viewModel.SelectedAuthorID);
                if(book.Publisher == null)
                    book.Publisher = db.Publishers.First(a => a.ID == viewModel.SelectedPublisherID);

                if (book.Categories.Count == 0)
                {
                    foreach (var i in viewModel.SelectedCategoryID)
                    {
                        book.Categories.Add(db.Categories.SingleOrDefault(a => a.ID == i));
                    }
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Obsługa")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [System.Web.Mvc.HttpPost, System.Web.Mvc.ActionName("Delete")]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult IsIsbnAvailable(string isbn)
        {
            // Check if the e-mail already exists
            return Json(!db.Books.Any(b => b.Isbn ==isbn), JsonRequestBehavior.AllowGet);
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
