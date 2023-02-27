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


    public class FaqCategoryCrudViewModel : CategoryCrudBaseViewModel { }

    public class CategorySearchViewModel
    {
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Language")]
        public long LanguageId { get; set; }


        [StringLength(200, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages),
            ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Name")]
        public string Name { get; set; }
    }

    public class CategoryListViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CategoryType { get; set; }

    }

    public class CategoryCrudBaseViewModel
    {
        public long Id { get; set; }
        public long LanguageId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "RequiredFieldError")]
        [StringLength(200, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Active")]
        public bool Active { get; set; }

        [Required(ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "RequiredFieldError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "SortOrder")]
        public int SortOrder { get; set; }

    }


    public class FaqCategoryIdSelectModel
    {
        [Required(ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "RequiredFieldError")]
        [Display(ResourceType = typeof(Localization.ViewModel.ModelItems), Name = "Category")]
        public long CategoryId { get; set; }

    }
}

