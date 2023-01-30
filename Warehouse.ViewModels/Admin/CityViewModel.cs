using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warehouse.ViewModels.Admin
{
    public class CityViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Şehir adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Name { get; set; }
    }
    public class CityListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Ülke adı")]

        public string Name { get; set; }

    }

    public class OrderCityIdSelectViewModel
    {
        [Display(Name = "Şehir adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? CityId { get; set; }
    }
}
