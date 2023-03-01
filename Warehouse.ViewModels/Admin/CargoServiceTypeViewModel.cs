using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.ViewModels.Admin
{
    public class CargoServiceTypeViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Kargo Servis Tipi")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Name { get; set; }
        public long? LanguageId { get; set; }


    }
    public class CargoServiceTypeListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Kargo Servis Tipi")]
        
        public string Name { get; set; }
        public long? LanguageId { get; set; }

    }
    public class OrderCargoServiceTypeIdSelectViewModel
    {
        [Display(Name = "Kargo Servis Tipi")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? CargoServiceId { get; set; }
    }
    public class CargoServiceSearchViewModel
    {
        [Display(Name = "Dil")]
        public long LanguageId { get; set; }
        [Display(Name = "Arama Metni")]
        public string SearchName { get; set; }
    }
}
