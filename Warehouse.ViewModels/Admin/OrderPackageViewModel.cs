using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.ViewModels.Admin
{
    public class OrderPackageViewModel
    {

        public OrderPackageViewModel()
        {
            this.OrderPackageGroupViewModel = new HashSet<OrderPackageGroupViewModel>();
        }
            
        public long Id { get; set; }
        public long Height { get; set; }
        public long Weight { get; set; }
        public long Width { get; set; }
        public long Length { get; set; }
        public long? OrderId { get; set; }
        public virtual ICollection<OrderPackageGroupViewModel> OrderPackageGroupViewModel { get; set; }
        public IEnumerable<OrderPackageGroupViewModel> OrderPackageGroups { get; set; }


    }
    public class OrderPackageGroupViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Boy")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Height { get; set; }
        [Display(Name = "Ağırlık")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Weight { get; set; }
        [Display(Name = "En")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Width { get; set; }
        [Display(Name = "Uzunluk")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Length { get; set; }
    }
}
