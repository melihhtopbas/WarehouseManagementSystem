using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Warehouse.ViewModels.Common;

namespace Warehouse.ViewModels.Admin
{
    public class BlogViewModel
    {
        public long LanguageId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "RequiredFieldError")]
        [StringLength(100, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Title")]
        public string Title { get; set; }


        [Required(ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "RequiredFieldError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Vision")]
        public string Vision { get; set; }

        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Mission")]
        public string Mission { get; set; }

        public string FileName { get; set; }

        public string FileName2 { get; set; }
    }
}
