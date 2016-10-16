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
using CatalogueMVC.BooksViewModel;

namespace CatalogueMVC.Controllers
{
    //[HandleError]
    public class BooksController : Controller
    {
        private BooksCatalogueDBEntities db = new BooksCatalogueDBEntities();
        // used 2nd dbcontext to get picture name while edit(with default one it was throwing error)
        private BooksCatalogueDBEntities db1 = new BooksCatalogueDBEntities();

        // GET: Books
        public ActionResult Index(string searchString, string sortOption, int page = 1)
        {
            try
            {

                int pageSize = 3;

                var books = db.Books.ToList();

                if (!String.IsNullOrEmpty(searchString))
                {
                    books = (db.Books.Include(b => b.Author).Include(b => b.Country).Where(n => n.Title.Contains(searchString) || n.Author.FullName.Contains(searchString))).ToList();
                    if (!books.Any())
                    {
                        return PartialView("NotFound", searchString);
                    }
                }

                List<BookModel> bm = GetBooks.GetResult(books);

                switch (sortOption)
                {
                    case "title_acs":
                        bm = bm.OrderBy(p => p.Title).ToList();
                        break;
                    case "title_desc":
                        bm = bm.OrderByDescending(p => p.Title).ToList();
                        break;
                    case "author_acs":
                        bm = bm.OrderBy(p => p.Author.FullName).ToList();
                        break;
                    case "author_desc":
                        bm = bm.OrderByDescending(p => p.Author.FullName).ToList();
                        break;
                    case "country_acs":
                        bm = bm.OrderBy(p => p.Country.Country1).ToList();
                        break;
                    case "country_desc":
                        bm = bm.OrderByDescending(p => p.Country.Country1).ToList();
                        break;
                    case "pages_acs":
                        bm = bm.OrderBy(p => p.PagesCount).ToList();
                        break;
                    case "pages_desc":
                        bm = bm.OrderByDescending(p => p.PagesCount).ToList();
                        break;
                    case "price_acs":
                        bm = bm.OrderBy(p => p.TotalPrice).ToList();
                        break;
                    case "price_desc":
                        bm = bm.OrderByDescending(p => p.TotalPrice).ToList();
                        break;
                    default:
                        bm = bm.OrderBy(p => p.BookID).ToList();
                        break;
                }

                if (page > bm.ToPagedList(page, pageSize).PageCount)
                {
                    return RedirectToAction("Index");
                }

                return Request.IsAjaxRequest()
                    ? (ActionResult)PartialView("GridBooks", bm.ToPagedList(page, pageSize))
                    : View(bm.ToPagedList(page, pageSize));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Books/Details/5
        //modified details action to work with viewmodel to get values from Attribute_Book 
        //couldnt get the attribute name
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);

            BookModel model = GetBooks.Details(book);

            ViewBag.AttName = db.Attribute_Book.Include(x => x.Attribute).Include(x=>x.Book).Where(x => x.BookID == id).Select(x => new SelectListItem { Value = x.Attribute.Name });

            if (book == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        const string _noImage = "no-image.png";

        // GET: Books/Create
        //modified to get lists of Attributes 
        [Authorize]
        public ActionResult Create()
        {
            try
            {
                ViewBag.AttributeID = new SelectList(db.Attributes, "AttributeID", "Name");
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookID,Title,AuthorID,CountryID,Price,Description,PagesCount,Picture")] BookModel book,
            HttpPostedFileBase file, string attText, int? AttributeID, DateTime? attDate)
        {
            try
            {
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

                    var mBook = GetBooks.Create(book, attText, AttributeID, attDate);
                    db.Books.Add(mBook);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ViewBag.AttributeID = new SelectList(db.Attributes, "AttributeID", "Name");
                ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName", book.AuthorID);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Country1", book.CountryID);
                return View(book);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Books/Edit/5
        //Edit doesnt work for extra attributes
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            //try
            //{
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            ViewBag.AttName = db.Attribute_Book.Include(b => b.Attribute).Include(b => b.Book).Where(b => b.BookID == id);

            ViewBag.Attribute_BookID = new SelectList(db.Attribute_Book, "ID", "ValueTypeText", book.Attribute_Book);
            ViewBag.AttributeID = new SelectList(db.Attributes, "AttributeID", "Name");
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName", book.AuthorID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Country1", book.CountryID);
            return View(book);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        // POST: Books/Edit/5
        //Edit doesnt work for extra attributes
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "BookID,Title,AuthorID,CountryID,Price,Description,PagesCount,Picture,Attribute_Book")] BookModel book,
            HttpPostedFileBase file, string attText, int? AttributeID, DateTime? date)
        {
            try
            {
                string pic = "";
                using (db1)
                {
                    pic = db1.Books.Find(book.BookID).Picture;
                }
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
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

                    var mBook = GetBooks.Edit(book, attText, AttributeID, date);
                    db.Entry(mBook).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ViewBag.AttributeID = new SelectList(db.Attributes, "AttributeID", "Name");
                ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FullName", book.AuthorID);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "Country1", book.CountryID);
                return View(book);
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
            catch (Exception ex)
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

                foreach (var item in db.Attribute_Book)
                {
                    var v1 = db.Attribute_Book.Where(b => b.BookID == book.BookID).FirstOrDefault();

                    if (v1 != null)
                        db.Attribute_Book.Remove(v1);
                }

                db.Books.Remove(book);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
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
