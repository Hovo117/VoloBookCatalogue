//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BooksEntityDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Attributes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Attributes()
        {
            this.Attribute_Book = new HashSet<Attribute_Book>();
        }
    
        public int AttributeID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attribute_Book> Attribute_Book { get; set; }
    }
}
