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

    }
    public class CountryListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Ülke adı")]
         
        public string Name { get; set; }

    }

    public class OrderCountryIdSelectViewModel
    {
        [Display(Name = "Ülke Adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? CountryId { get; set; }
    }
}
