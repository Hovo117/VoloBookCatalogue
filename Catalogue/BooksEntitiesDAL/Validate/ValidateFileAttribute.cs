using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BooksEntitiesDAL.Validate
{
    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string[] sAllowedExt = new string[] { ".jpg", ".jpeg", ".png" };

            var file = value as HttpPostedFileBase;

            if (file != null && !sAllowedExt.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Please upload Your Image of type: " + string.Join(", ", sAllowedExt);
                return false;
            }
            else
                return true;
        }
    }
}
