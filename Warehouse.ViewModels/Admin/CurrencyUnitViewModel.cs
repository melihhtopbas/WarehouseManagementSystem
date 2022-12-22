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

    }
    public class OrderCurrencyUnitIdSelectViewModel
    {
        [Display(Name = "Para Birimi")]
        public long? CurrencyUnitId;
    }
}
