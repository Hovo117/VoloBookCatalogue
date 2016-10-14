using BooksEntitiesDAL;
using BooksEntitiesDAL.Validate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatalogueMVC.BooksViewModel
{
    public class BookModel
    {
        public int BookID { get; set; }

        [Required(ErrorMessage = "The field Title must be filled")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The field Author must be filled")]
        [Display(Name = "Author")]
        public int AuthorID { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "The field Country must be filled")]
        public int CountryID { get; set; }

        [Required(ErrorMessage = "The field Price must be filled")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Display(Name = "Pages")]
        public Nullable<int> PagesCount { get; set; }

        [ValidateFile]
        public string Picture { get; set; }

        public decimal TotalPrice { get; set; }

        public virtual List<Attribute_Book> Attribute_Book { get; set; }
        public virtual List<BooksEntitiesDAL.Attribute> Attributes { get; set; }
        public virtual Author Author { get; set; }
        public virtual Country Country { get; set; }
    }
}