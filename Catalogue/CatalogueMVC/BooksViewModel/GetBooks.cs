using BooksEntitiesDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CatalogueMVC.BooksViewModel
{
    public class GetBooks
    {
        public static List<BookModel> GetResult(IEnumerable<Book> books)
        {
            var gr = new List<BookModel>();

            foreach (var item in books)
            {
                gr.Add(new BookModel
                {
                    BookID = item.BookID,
                    Title = item.Title,
                    PagesCount = Convert.ToInt32(item.PagesCount),
                    Description = item.Description,
                    Price = item.Price,
                    CountryID = item.CountryID,
                    AuthorID = item.AuthorID,
                    Author = item.Author,
                    Country = item.Country,
                    Picture = item.Picture,
                    TotalPrice = item.Country.TelCode + item.Price,
                    Attribute_Book = item.Attribute_Book.ToList()
                });
            }
            return gr;
        }

        public static Book Create(BookModel model,string attText,int id,DateTime date)
        {
            Book book = new Book
            {
                BookID = model.BookID,
                Title = model.Title,
                PagesCount = model.PagesCount,
                Description = model.Description, 
                Price = model.Price,
                Picture = model.Picture,
                AuthorID = model.AuthorID,
                CountryID = model.CountryID,
                Author = model.Author,
                Country = model.Country,
                Attribute_Book = new List<Attribute_Book> { new Attribute_Book { BookID = model.BookID, AttributeID = id , ValueTypeText = attText } }
            };
            return book;
        }

        public static BookModel Details(Book item)
        {
            var model = new BookModel
            {
                BookID = item.BookID,
                Title = item.Title,
                PagesCount = Convert.ToInt32(item.PagesCount),
                Description = item.Description,
                Price = item.Price,
                CountryID = item.CountryID,
                AuthorID = item.AuthorID,
                Author = item.Author,
                Country = item.Country,
                Picture = item.Picture,
                TotalPrice = item.Country.TelCode + item.Price,
                Attribute_Book = item.Attribute_Book.ToList()
            };
            return model;
        }
    }
}