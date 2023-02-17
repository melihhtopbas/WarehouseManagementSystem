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
        [Required(ErrorMessage = "Lütfen giriniz")]
        [StringLength(100, ErrorMessage = "StringLengthMaxLengthError")]
        [Display(Name = "Ad")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Lütfen giriniz")]
        [StringLength(100, ErrorMessage = "StringLengthMaxLengthError")]
        [Display(Name = "Mesaj")]
        public string Message { get; set; }
        [Required(ErrorMessage = "Lütfen giriniz")]
        [StringLength(100, ErrorMessage = "StringLengthMaxLengthError")]
        [Display(Name = "Konu")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Lütfen giriniz")]
        [StringLength(100, ErrorMessage = "StringLengthMaxLengthError")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Lütfen giriniz")]
        [StringLength(100, ErrorMessage = "StringLengthMaxLengthError")]
        [Display(Name = "E-Posta")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Lütfen giriniz")]
        [StringLength(100, ErrorMessage = "StringLengthMaxLengthError")]
        [Display(Name = "Ürün")]
        public string Product { get; set; }
        [Required(ErrorMessage = "Lütfen giriniz")]
        [StringLength(100, ErrorMessage = "StringLengthMaxLengthError")]
        [Display(Name = "Şirket")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Lütfen giriniz")]
        [StringLength(100, ErrorMessage = "StringLengthMaxLengthError")]
        [Display(Name = "KDV Oranı")]
        public string VatRate { get; set; }

    }
}
