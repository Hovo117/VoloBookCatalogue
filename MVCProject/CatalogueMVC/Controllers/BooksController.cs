using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnititiesDAL;
using System.ComponentModel.DataAnnotations;

namespace CatalogueMVC.Controllers
{
    public class BooksController : Controller
    {
        private BooksCatalogueEntities db = new BooksCatalogueEntities();

        // GET: Books
        public async Task<ActionResult> Index()
        {
            var books = db.Books.Include(b => b.Authors).Include(b => b.Countries).Include(b => b.Images);
            return View(await books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = await db.Books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // GET: Books/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName");
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name");
            ViewBag.ImageID = new SelectList(db.Images, "ImageID", "ImageID");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookID,Title,AuthorID,CountryID,Price,Description,PagesCount,ImageID")] Books books)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(books);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName", books.AuthorID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name", books.CountryID);
            ViewBag.ImageID = new SelectList(db.Images, "ImageID", "ImageID", books.ImageID);
            return View(books);
        }

        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = await db.Books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName", books.AuthorID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name", books.CountryID);
            ViewBag.ImageID = new SelectList(db.Images, "ImageID", "ImageID", books.ImageID);
            return View(books);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookID,Title,AuthorID,CountryID,Price,Description,PagesCount,ImageID")] Books books)
        {
            if (ModelState.IsValid)
            {
                db.Entry(books).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName", books.AuthorID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Name", books.CountryID);
            ViewBag.ImageID = new SelectList(db.Images, "ImageID", "ImageID", books.ImageID);
            return View(books);
        }

        // GET: Books/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Books books = await db.Books.FindAsync(id);
            if (books == null)
            {
                return HttpNotFound();
            }
            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Books books = await db.Books.FindAsync(id);
            db.Books.Remove(books);
            await db.SaveChangesAsync();
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
