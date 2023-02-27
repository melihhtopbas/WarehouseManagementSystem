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
    public class FaqListViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Link { get; set; }
    }
    public class FaqAddViewModel : FaqCrudBaseViewModel
    {


    }
    public class FaqEditViewModel : FaqCrudBaseViewModel
    {
        public long Id { get; set; }

    }
    public class FaqCrudBaseViewModel
    {

        public long LanguageId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "RequiredFieldError")]
        [StringLength(200, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "RequiredFieldError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Description")]
        public string Description { get; set; }

        public FaqCategoryIdSelectModel Category { get; set; }
    }

    public class FaqSearchViewModel
    {
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Language")]
        public long LanguageId { get; set; }
        [StringLength(200, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages),
            ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Name")]
        public string Name { get; set; }
    }
}
