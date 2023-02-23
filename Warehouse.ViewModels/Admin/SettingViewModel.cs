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
    public class SettingViewModel
    {
        public long LanguageId { get; set; }



        [StringLength(250, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "SeoTitle")]
        public string SeoTitle { get; set; }

        [StringLength(500, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "SeoKeywords")]
        public string SeoKeywords { get; set; }

        [StringLength(500, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "SeoDescription")]
        public string SeoDescription { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        public string Facebook { get; set; }


        [StringLength(250, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        public string Instagram { get; set; }


        [StringLength(250, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        public string Twitter { get; set; }


        [StringLength(250, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        public string Youtube { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        public string Gplus { get; set; }


        [StringLength(250, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "Adress")]
        public string Adress { get; set; }

        [StringLength(500, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "Phone")]
        public string Phone { get; set; }

        [StringLength(500, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Telefon2")]
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Meta { get; set; }
        public string Maps { get; set; }
        public string Analytics { get; set; }
        public string Logo { get; set; }
        public string Favicon { get; set; }
    }
}
