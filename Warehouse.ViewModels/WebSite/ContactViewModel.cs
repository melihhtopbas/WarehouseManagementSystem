using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warehouse.ViewModels.WebSite
{
    public class ContactViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.WebSiteViewItems.WebSiteViewItems), Name = "FullName")]
        public string FullName { get; set; }
        [Required]
        [StringLength(1000, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.WebSiteViewItems.WebSiteViewItems), Name = "Message")]
        public string Message { get; set; }
        [Required]
        [StringLength(250, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.WebSiteViewItems.WebSiteViewItems), Name = "Subject")]
        public string Subject { get; set; }
        [Required]
        [StringLength(20, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.WebSiteViewItems.WebSiteViewItems), Name = "Phone")]
        public string Phone { get; set; }
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Warehouse.Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Warehouse.Localization.WebSiteViewItems.WebSiteViewItems), Name = "Email")]
        public string Email { get; set; }

        public long Id { get; set; }
        public DateTime Date { get; set; }

    }
}
