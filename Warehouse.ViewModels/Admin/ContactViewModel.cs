﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warehouse.ViewModels.Admin
{
    public class ContactViewModel
    {
        [StringLength(100, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Arama Metni")]
        public string FullName { get; set; }

        [StringLength(1000, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(Name = "Konu")]
        public string Message { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.WebSiteViewItems.WebSiteViewItems), Name = "Subject")]
        public string Subject { get; set; }

        [StringLength(20, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.WebSiteViewItems.WebSiteViewItems), Name = "Phone")]
        public string Phone { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof(Localization.Validation.ValidationMessages), ErrorMessageResourceName = "StringLengthMaxLengthError")]
        [Display(ResourceType = typeof(Localization.WebSiteViewItems.WebSiteViewItems), Name = "Email")]
        public string Email { get; set; }

        public long Id { get; set; }
        public DateTime Date { get; set; }
        public bool isShow { get; set; }

    }
}
