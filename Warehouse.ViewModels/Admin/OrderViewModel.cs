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
        public OrderCountryIdSelectViewModel Country { get; set; } //yapıldı
        public OrderCargoServiceTypeIdSelectViewModel CargoService { get; set; } //yapılmadı
        public OrderCurrencyUnitIdSelectViewModel CurrenyUnit { get; set; }//yapılmadı
    }
     public class OrderAddViewModel : OrderCrudBaseViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Gönderici Adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string SenderName { get; set; }
        [Display(Name = "Gönderici Mail-i")]
        
        public string SenderMail { get; set; }
        [Display(Name = "Gönderici Telefonu")]
        
        public string SenderPhone { get; set; }
        [Display(Name = "Gönderici Kimlik No")]
      
        public string SenderIdentityNumber { get; set; }
        [Display(Name = "Alıcı Adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientName { get; set; }
        [Display(Name = "Alıcı Adres")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientAddress { get; set; }
        [Display(Name = "Alıcı Şehir")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientCity { get; set; }
        [Display(Name = "Alıcı Ülke")]
         
        public long? RecipientCountryId { get; set; }

        public List<CountryViewModel> CountryList { get; set; }
        [Display(Name = "Alıcı Posta Kodu")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientZipCode { get; set; }
        [Display(Name = "Alıcı Telefon")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientPhone { get; set; }
        [Display(Name = "Alıcı Kimlik No")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientIdentityNumber { get; set; }
        [Display(Name = "Alıcı Mail-i")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string RecipientMail { get; set; }
        [Display(Name = "Para Birimi")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long ProductCurrencyUnitId { get; set; }
        public List<CurrencyUnitViewModel> CurrencyUnitList { get; set; }
        [Display(Name = "Ürün Açıklaması")]
      
        public string ProductOrderDescription { get; set; }
        [Display(Name = "Kargo Servis Tipi")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long CargoServiceTypeId { get; set; }
        [Display(Name = "Koli Adeti")]
         
        public int PackageCount { get; set; }
        [Display(Name = "Ağırlık(kg)")]
        
        public long? PackageWeight { get; set; }
        [Display(Name = "Boy(cm)")]
        public long? PackageHeight { get; set; }
        [Display(Name = "Uzunluk(cm)")]
        public long? PackageLength { get; set; }
        [Display(Name = "En(cm)")]
        public long? PackageWidth { get; set; }

        public virtual CargoServiceTypeViewModel CargoServiceTypes { get; set; }
        public virtual CountryViewModel Countries { get; set; }
        public virtual CurrencyUnitViewModel CurrencyUnits { get; set; }
        public virtual ICollection<ProductTransactionGroupViewModel> ProductTransactionGroup { get; set; }
    }
    public class OrderListViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Gönderici Adı")]
         
        public string SenderName { get; set; }
        [Display(Name = "Gönderici Mail-i")]
 

        public string SenderPhone { get; set; }
       
        [Display(Name = "Alıcı Adı")]
       
        public string RecipientName { get; set; }
        [Display(Name = "Alıcı Adres")]
        
        public string RecipientAddress { get; set; }
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
         
        public long CargoServiceTypeId { get; set; }
        [Display(Name = "Koli Adeti")]

        public int PackageCount { get; set; }
        [Display(Name = "Sipariş Açıklaması")]

        public string OrderDescription { get; set; }
        [Display(Name = "Ülke")]

        public string RecipientCountry { get; set; }
        [Display(Name = "Para Birimi")]

        public string CurrencyUnit { get; set; }


        public virtual CargoServiceTypeViewModel CargoServiceTypes { get; set; }
        public virtual CountryViewModel Countries { get; set; }
        public virtual CurrencyUnitViewModel CurrencyUnits { get; set; }
        public virtual ICollection<ProductTransactionGroupViewModel> ProductTransactionGroup { get; set; }
    }
    public class OrderSearchViewModel
    {
        public long Id { get; set; }
        public string SearchName { get; set; }
    }
}
