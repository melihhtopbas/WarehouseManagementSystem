using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warehouse.ViewModels.Common
{
    public class TicketBoxViewModel
    {
        public long Id { get; set; }
        [StringLength(100, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Arama Metni")]
        public string SearchName { get; set; }
        [Display(Name = "Gönderici")]
        public string SenderName { get; set; }


        [StringLength(1000, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Konu")]
        public string Message { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.WebSiteViewItems.WebSiteViewItems), Name = "Subject")]
        public string Subject { get; set; }
        public int TimeHour { get; set; }
        public int TimeDay { get; set; }
        public int TimeMinute { get; set; }
        public DateTime Date { get; set; }
        public bool isShow { get; set; }
        public bool isAnswer { get; set; }
    }
    public class TicketBoxAddViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Alıcı Kullanıcı Adı")]
        public string ReceiverName { get; set; }
        [StringLength(1000, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Mesaj")]
        public string Message { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Konu")]
        public string Subject { get; set; }



    }
    public class TicketBoxShowViewModel
    {
        public long Id { get; set; }
        [StringLength(100, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Arama Metni")]
        public string SearchName { get; set; }
        [Display(Name = "Gönderici")]
        public string SenderName { get; set; }


        [StringLength(1000, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Konu")]
        public string Message { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.WebSiteViewItems.WebSiteViewItems), Name = "Subject")]
        public string Subject { get; set; }
        public int TimeHour { get; set; }
        public int TimeDay { get; set; }
        public int TimeMinute { get; set; }
        public DateTime Date { get; set; }
        public bool isShow { get; set; }
        public bool isAnswer { get; set; }
    }
}
