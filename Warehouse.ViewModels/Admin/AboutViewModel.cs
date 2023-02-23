using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warehouse.ViewModels.Admin
{
    public class AboutViewModel
    {
        public long LanguageId { get; set; }

        [StringLength(100, ErrorMessage = "StringLengthMaxLengthError")]
        [Required(ErrorMessage = "RequiredFieldError")]
        [Display(Name = "Title")]
        public string Title { get; set; }


        [Required(ErrorMessage = "RequiredFieldError")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Vision")]
        public string Vision { get; set; }

        [Display(Name = "Mission")]
        public string Mission { get; set; }

        public string FileName { get; set; }

        public string FileName2 { get; set; }
    }
}
