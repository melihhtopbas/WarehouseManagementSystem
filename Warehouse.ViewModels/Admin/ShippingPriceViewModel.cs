using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Warehouse.Data;

namespace Warehouse.ViewModels.Admin
{
    public class ShippingPriceViewModel
    {
        public long Id { get; set; }
        
        public Nullable<long> LanguageId { get; set; }
        
       
        public string CountryName { get; set; }
        public string CurrencyUnitName { get; set; }


        public List<CountryShippingPriceViewModel> CountryShippingPriceViewModels { get; set; }


    }
    public class ShippingPriceListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Ülke adı")]

        public string CountryName { get; set; }
        [Display(Name = "Dil")]
        public long? LanguageId { get; set; }
        public bool? Active { get; set; }
        [Display(Name = "Para Birimi")]
        public string CurrencyUnitName { get; set; }
        public string CargoServiceName { get; set; }
    }
    public class ShippingPriceSearchViewModel
    {
        [Display(Name = "Dil")]
        public long LanguageId { get; set; }
        [Display(Name = "Arama Metni")]
        public string SearchName { get; set; }
    }
    public class CountryShippingPriceViewModel
    {
        public long Id { get; set; }
      
        public bool Active { get; set; }
        public Nullable<long> CountryId { get; set; }

        [Required(ErrorMessage ="Lütfen doldurunuz!")]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        public string CargoServiceName { get; set; }
        public string CurrencyUnitName { get; set; }
        [Display(Name = "Süre")]
        public string DeliveryTime { get; set; }

    }
}
