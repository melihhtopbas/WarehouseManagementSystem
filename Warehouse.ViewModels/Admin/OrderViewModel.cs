using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Data;

namespace Warehouse.ViewModels.Admin
{
    public class OrderViewModel
    {

    }
    public class OrderCrudBaseViewModel
    {
        public OrderCrudBaseViewModel()
        {
            ProductTransactionGroup = new List<ProductTransactionGroupViewModel>();
            OrderPackageGroups = new List<PackageListViewModel>();
        }
        public OrderCountryIdSelectViewModel Country { get; set; }
        public OrderCargoServiceTypeIdSelectViewModel CargoService { get; set; }
        public OrderCurrencyUnitIdSelectViewModel CurrenyUnit { get; set; }
        public long Id { get; set; }

        [Display(Name = "Gönderici Adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        [StringLength(200)]

        public string SenderName { get; set; }
        [Display(Name = "Gönderici Mail")]
        
        public string SenderMail { get; set; }
        [Display(Name = "Gönderici Telefonu")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        [MaxLength(11)]
        [MinLength(7,ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Telefon numarası numerik olmalıdır!")]


        public string SenderPhone { get; set; }
        [Display(Name = "Gönderici Kimlik No")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numerik olmalıdır!")]

        public string SenderIdentityNumber { get; set; }
        [Display(Name = "Gönderici Fatura No")]

        public string SenderInvoiceNumber { get; set; }
        [Display(Name = "Alıcı Adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientName { get; set; }
        [Display(Name = "Alıcı Adres")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientAddress { get; set; }
        [Display(Name = "Gönderici Adres")]
        
        public string SenderAddress { get; set; }
        [Display(Name = "Alıcı Şehir")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientCity { get; set; }

        [Display(Name = "Alıcı Posta Kodu")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numerik olmalıdır!")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientZipCode { get; set; }
        [Display(Name = "Alıcı Telefon")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        [MaxLength(11)]
        [MinLength(7, ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz!")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Telefon numarası numerik olmalıdır!")]
        public string RecipientPhone { get; set; }
        [Display(Name = "Alıcı Kimlik No")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Numerik olmalıdır!")]

        public string RecipientIdentityNumber { get; set; }
        [Display(Name = "Alıcı Fatura No")]

        public string RecipientInvoiceNumber { get; set; }
        [Display(Name = "Alıcı Mail")]

        public string RecipientMail { get; set; }

 
        [Display(Name = "Sipariş Açıklaması")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string OrderDescription { get; set; }

        public virtual CargoServiceTypeViewModel CargoServiceTypes { get; set; }
        public virtual CountryViewModel Countries { get; set; }
        public virtual CurrencyUnitViewModel CurrencyUnits { get; set; }
        public virtual ProductTransactionGroupViewModel ProductTransactionGroupViewModel { get; set; }
        public IEnumerable<ProductTransactionGroupViewModel> ProductTransactionGroup { get; set; }

        public virtual ICollection<OrderPackageGroupViewModel> OrderPackageGroupViewModel { get; set; }
        public IEnumerable<PackageListViewModel> OrderPackageGroups { get; set; }
    }
    public class OrderAddViewModel : OrderCrudBaseViewModel
    {
        
    }
    public class OrderListViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Gönderici Adı")]

        public string SenderName { get; set; }
        [Display(Name = "Gönderici Telefon")]


        public string SenderPhone { get; set; }

        [Display(Name = "Alıcı Adı")]

        public string RecipientName { get; set; }
        [Display(Name = "Alıcı Fatura No")]

        public string RecipientInvoiceNumber { get; set; }
        [Display(Name = "Gönderici Fatura No")]

        public string SenderInvoiceNumber { get; set; }
        [Display(Name = "Alıcı Adres")]

        public string RecipientAddress { get; set; }
        [Display(Name = "Gönderici Adres")]

        public string SenderAddress { get; set; }
        [Display(Name = "Gönderici Adres(diğer)")]

        public string SenderAddressOther { get; set; }
        [Display(Name = "Alıcı Şehir")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientCity { get; set; }
        [Display(Name = "Alıcı Ülke")]

        public long? RecipientCountryId { get; set; }


        [Display(Name = "Alıcı Posta Kodu")]

        public string RecipientZipCode { get; set; }
        [Display(Name = "Alıcı Telefon")]

        public string RecipientPhone { get; set; }


        public List<CurrencyUnitViewModel> CurrencyUnitList { get; set; }

        [Display(Name = "Kargo Servis Tipi")]

        public long? CargoServiceTypeId { get; set; }
        [Display(Name = "Koli Adeti")]

        public int? PackageCount { get; set; }
        [Display(Name = "Sipariş Açıklaması")]

        public string OrderDescription { get; set; }
        [Display(Name = "Ülke")]

        public string RecipientCountry { get; set; }
        [Display(Name = "Para Birimi")]

        public string CurrencyUnit { get; set; }
        [Display(Name = "Kargo Servis Tipi")]

        public string CargoService { get; set; }
        public bool? isPackage { get; set; }


        public virtual CargoServiceTypeViewModel CargoServiceTypes { get; set; }
        public virtual CountryViewModel Countries { get; set; }
        public virtual CurrencyUnitViewModel CurrencyUnits { get; set; }
        public virtual ICollection<ProductTransactionGroupViewModel> ProductTransactionGroup { get; set; }
    }
    public class OrderSearchViewModel
    {
        [Display(Name ="Dil")]
        public long LanguageId { get; set; }
        [Display(Name = "Arama")]
        public string SearchName { get; set; }
    }
    public class OrderEditViewModel : OrderCrudBaseViewModel
    {
        //public long Id { get; set; }

    }
    public class OrderAdressViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Adress")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Name { get; set; }

    }
}