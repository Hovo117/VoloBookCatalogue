using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BooksEntitiesDAL;
using System.IO;
using System.ComponentModel.DataAnnotations;
using PagedList;
using System.Web.Helpers;

namespace CatalogueMVC.Controllers
{
    public class BooksController : Controller
    {
        private BooksCatalogueDBEntities db = new BooksCatalogueDBEntities();
        private BooksCatalogueDBEntities db1 = new BooksCatalogueDBEntities();

        // GET: Books
        public ActionResult Index(string searchString, string sortOption, int page = 1)
        {
            try
            {

                int pageSize = 3;

                var books = db.Books.AsQueryable();

                if (!String.IsNullOrEmpty(searchString))
                {
                    books = books.Where(n => n.Title.Contains(searchString) || n.Author.FullName.Contains(searchString));
                    if (!books.Any())
                    {
                        return PartialView("NotFound", searchString);
                    }
                }

                switch (sortOption)
                {
                    case "title_acs":
                        books = books.OrderBy(p => p.Title);
                        break;
                    case "title_desc":
                        books = books.OrderByDescending(p => p.Title);
                        break;
                    case "author_acs":
                        books = books.OrderBy(p => p.AuthorID);
                        break;
                    case "author_desc":
                        books = books.OrderByDescending(p => p.AuthorID);
                        break;
                    case "country_acs":
                        books = books.OrderBy(p => p.CountryID);
                        break;
                    case "country_desc":
                        books = books.OrderByDescending(p => p.CountryID);
                        break;
                    case "pages_acs":
                        books = books.OrderBy(p => p.PagesCount);
                        break;
                    case "pages_desc":
                        books = books.OrderByDescending(p => p.PagesCount);
                        break;
                    case "price_acs":
                        books = books.OrderBy(p => p.Price);
                        break;
                    case "price_desc":
                        books = books.OrderByDescending(p => p.Price);
                        break;
                    default:
                        books = books.OrderBy(p => p.BookID);
                        break;
                }

                if (page > books.ToPagedList(page, pageSize).PageCount)
                {
                    page = 1;
                }

                return Request.IsAjaxRequest()
                    ? (ActionResult)PartialView("GridBooks", books.ToPagedList(page, pageSize))
                    : View(books.ToPagedList(page, pageSize));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        const string _noImage = "no-image.png";

        // GET: Books/Create
        [Authorize]
        public ActionResult Create()
        {
            try
            {
                ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName");
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Country1");
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookID,Title,AuthorID,CountryID,Price,Description,PagesCount,Picture")] Book book, HttpPostedFileBase file)
        {
            //try
            //{
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 270)
                            img.Resize(260, 400);
                        img.Save(path);
                        book.Picture = fileName;
                    }
                    else
                    {
                        book.Picture = _noImage;
                    }

                    db.Books.Add(book);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName", book.AuthorID);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Country1", book.CountryID);
                return View(book);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        // GET: Books/Edit/5

        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Book book = await db.Books.FindAsync(id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName", book.AuthorID);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Country1", book.CountryID);
                return View(book);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookID,Title,AuthorID,CountryID,Price,Description,PagesCount,Picture")] Book book, HttpPostedFileBase file)
        {
            //try
            //{
                string pic = "";
                using (db1)
                {
                    pic = db1.Books.Find(book.BookID).Picture;
                }
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".png", ".jpeg" };
                        var checkextension = Path.GetExtension(file.FileName).ToLower();

                        if (!allowedExtensions.Contains(checkextension))
                        {
                            return RedirectToAction("Index");
                        }

                        if (pic != _noImage)
                        {
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Images"), pic));
                        }

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 270)
                            img.Resize(260, 400);
                        img.Save(path);
                        book.Picture = fileName;
                    }
                    else
                    {
                        book.Picture = pic;
                    }

                    db.Entry(book).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName", book.AuthorID);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Country1", book.CountryID);
                return View(book);
            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //}

        }

        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Book book = await db.Books.FindAsync(id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                return View(book);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        // POST: Books/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Book book = await db.Books.FindAsync(id);

                if (book.Picture != null && book.Picture != _noImage)
                {
                    var fileName = Path.GetFileName(book.Picture);
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                db.Books.Remove(book);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw ex;
            }
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
