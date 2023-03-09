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
    public class TicketBoxAnswerViewModel
    {
        public long Id { get; set; }
        [StringLength(100, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Arama Metni")]
        public string SearchName { get; set; }
        [Display(Name = "Gönderici")]
        public string SenderName { get; set; }


        [StringLength(1000, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Mesaj")]
        public string Message { get; set; }

        [Display(Name = "Cevap")]
        public string AnswerMessage { get; set; }

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
    public class TicketBoxAnswerAddViewModel
    {
        public long Id { get; set; }
        public long TicketId { get; set; }
        public long UserId { get; set; }
        [Display(Name = "Alıcı Kullanıcı Adı")]
        public string SenderName { get; set; }
        [StringLength(1000, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Gelen Mesaj")]
        public string  ComingMessage { get; set; }
        [StringLength(1000, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Cevap")]
        public string AnswerMessage { get; set; }
        [StringLength(250, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Konu")]
        public string Subject { get; set; }
        public DateTime Date { get; set; }

        public bool isAnswer { get; set; }


    }
    public class TicketBoxAnswerShowViewModel
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
