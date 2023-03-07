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
    public class IncomingMessageViewModel
    {
        public long Id { get; set; }
        [StringLength(100, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Arama Metni")]
        public string FullName { get; set; }

        [StringLength(1000, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Konu")]
        public string Message { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.WebSiteViewItems.WebSiteViewItems), Name = "Subject")]
        public string Subject { get; set; }
        public int TimeHour { get; set; } 
        public int TimeDay { get; set; } 
        public int TimeMinute { get; set; } 
    }
}
