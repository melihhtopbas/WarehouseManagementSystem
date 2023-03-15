using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Warehouse.ViewModels.Admin
{
    public class OrderPackageProductViewModel
    {
        
    }
    public class OrderPackageIntoAddProductViewModel
    {
        public OrderPackageIntoAddProductViewModel()
        {
            ProductGroupsAddViewModels = new List<ProductGroupsAddViewModel>();
        }
        public long Id { get; set; }

        public IEnumerable<ProductGroupsAddViewModel> ProductGroupsAddViewModels { get; set; }


        public long OrderId { get; set; }
        public long PackageId { get; set; }
    }
    public class OrderPackageIntoListProductViewModel
    {

    }
    public class ProductGroupsAddViewModel
    {
        public long Id { get; set; }

        [Display(Name = "İçerik")]
        public string Content { get; set; }
        public string SKU { get; set; }
        [Display(Name = "Paketlenmemiş Ürün Sayısı")]
        public int? ProductCount { get; set; }
        [Display(Name = "Toplam Ürün Sayısı")]
        public int? TotalCount { get; set; }
        [Display(Name = "Paketlenmiş Ürün Sayısı")]
        public int? PackagedProductCount { get; set; }
        [Display(Name = "Paketlenecek Ürün Sayısı")]
        [Range(1, int.MaxValue, ErrorMessage = "0'dan farklı adet giriniz")]
        public int? IsPackagedProductCount { get; set; }
        public long ProductId { get; set; }


        public long PackageId { get; set; }

        public bool isChecked { get; set; }

    }
}
