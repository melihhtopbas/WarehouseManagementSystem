using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.ViewModels.Admin
{
    public class CountryViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Ülke adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Warehouse.Localization.ViewModel.ModelItems), Name = "Active")]
        public bool Active { get; set; }

        public long? LanguageId { get; set; }
        public OrderCurrencyUnitIdSelectViewModel CurrenyUnit { get; set; }
    }
    public class CountryListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Ülke adı")]
         
        public string Name { get; set; }
        [Display(Name = "Dil")]
        public long? LanguageId { get; set; }
        public bool? Active { get; set; }
        [Display(Name = "Para Birimi")]
        public string CurrencyUnitName { get; set; }

    }

    public class OrderCountryIdSelectViewModel
    {
        [Display(Name = "Ülke Adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? CountryId { get; set; }
    }
    public class CountrySearchViewModel
    {
        [Display(Name = "Dil")]
        public long LanguageId { get; set; }
        [Display(Name = "Arama Metni")]
        public string SearchName { get; set; }
    }
}
