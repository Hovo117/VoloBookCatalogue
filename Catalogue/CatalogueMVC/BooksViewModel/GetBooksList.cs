using BooksEntitiesDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatalogueMVC.BooksViewModel
{
    public class GetBooksList
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
                    Attribute_Book = item.Attribute_Book
                });
            }

            return gr;
        }
    }
}