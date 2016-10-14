using BooksEntitiesDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CatalogueMVC.AttBookValueModel
{
    public class AttBookModel
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public int AttributeID { get; set; }
        public string ValueTypeText { get; set; }
        public Nullable<System.DateTime> ValueTypeDate { get; set; }

        public virtual BooksEntitiesDAL.Attribute Attribute { get; set; }
        public virtual Book Book { get; set; }
    }
}