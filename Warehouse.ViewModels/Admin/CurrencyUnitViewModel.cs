using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.ViewModels.Admin
{
    public class CurrencyUnitViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Para Birimi")]
        [Required(ErrorMessage = "Lütfen  giriniz")]
        public string Name { get; set; }
        public long? LanguageId { get; set; }

        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "Lütfen  giriniz")]
        public string Description { get; set; }

    }
    public class CurrencyUnitListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Para Birimi")]
        
        public string Name { get; set; }
        public long? LanguageId { get; set; }
        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "Lütfen  giriniz")]
        public string Description { get; set; }

    }
    public class OrderCurrencyUnitIdSelectViewModel
    {
        [Display(Name = "Para Birimi")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? CurrencyUnitId { get; set; }
    }
    public class CurrencyUnitSearchViewModel
    {
        [Display(Name = "Dil")]
        public long LanguageId { get; set; }
        [Display(Name = "Arama Metni")]
        public string SearchName { get; set; }
    }
}
