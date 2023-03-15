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
        public IEnumerable<ProductGroupShowViewModel> OrderProductGroups { get; set; }
        public IEnumerable<ProductGroupShowViewModel> OrderChechBoxes { get; set; }



    }
    public class OrderPackageGroupViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Yükseklik")]
        [Range(1, 999, ErrorMessage = "Enter number between 1 to 999")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Height { get; set; }
        [Display(Name = "Ağırlık")]
        [Range(1, 300, ErrorMessage = "Enter number between 1 to 300")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Weight { get; set; }
        [Display(Name = "En")]
        [Range(1, 999, ErrorMessage = "Enter number between 1 to 999")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Width { get; set; }
        [Display(Name = "Boy")]
        [Range(1, 999, ErrorMessage = "Enter number between 1 to 999")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Length { get; set; }
        [Display(Name = "Adet")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        [Range(1, 30, ErrorMessage = "Enter number between 1 to 30")]
        public int? Count { get; set; }
        [Display(Name = "Desi")]

        public double? Desi { get; set; }
        public long? OrderId { get; set; }
    }
    public class PackageListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Yükseklik")]

        public long? Height { get; set; }
        [Display(Name = "Ağırlık")]

        public long? Weight { get; set; }
        [Display(Name = "En")]

        public long? Width { get; set; }
        [Display(Name = "Boy")]

        public long? Length { get; set; }
        [Display(Name = "Adet")]

        public int? Count { get; set; }
        [Display(Name = "Desi")]

        public decimal Desi { get; set; }
    }
    public class OrderPackageListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Yükseklik")]

        public long? Height { get; set; }
        [Display(Name = "Ağırlık")]

        public long? Weight { get; set; }
        [Display(Name = "En")]

        public long? Width { get; set; }
        [Display(Name = "Boy")]

        public long? Length { get; set; }
        [Display(Name = "Adet")]

        public int? Count { get; set; }
        [Display(Name = "Desi")]

        public decimal? Desi { get; set; }
        public IEnumerable<ProductGroupShowViewModel> OrderPackageProductGroups { get; set; }

        [Display(Name = "Koli içerisindeki toplam ürün sayısı")]
        public int? CountProductsInThePackage { get; set; }
        public long? OrderId { get; set; }
        public string Barcode { get; set; }
        public string CountryName { get; set; }
        public string ReceiverName { get; set; }




    }
    public class OrderPackageAddViewModel
    {
        public long Id { get; set; }
        public long OrderId { get; set; }


        public IEnumerable<ProductGroupShowViewModel> OrderProductGroups { get; set; }
        public IEnumerable<OrderPackageProductAddViewModel> OrderPackageProductAddViewModels { get; set; }
        public IEnumerable<OrderPackageProductListViewModel> OrderPackageProductListViewModels { get; set; }
    }
    public class OrderPackageProductAddViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Yükseklik")]
        public long? OrderId { get; set; }

        [Display(Name = "Yükseklik")]
        [Range(1, 999, ErrorMessage = "Enter number between 1 to 999")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Height { get; set; }
        [Display(Name = "Ağırlık")]
        [Range(1, 300, ErrorMessage = "Enter number between 1 to 300")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Weight { get; set; }
        [Display(Name = "En")]
        [Range(1, 999, ErrorMessage = "Enter number between 1 to 999")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Width { get; set; }
        [Display(Name = "Boy")]
        [Range(1, 999, ErrorMessage = "Enter number between 1 to 999")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? Length { get; set; }
        [Display(Name = "Adet")]

        public int? Count { get; set; }
        [Display(Name = "Desi")]

        public decimal Desi { get; set; }
        public IEnumerable<ProductGroupShowViewModel> OrderProductGroups { get; set; }
        [Display(Name = "Sipariş Miktarı")]
        public int? PackagedCount { get; set; }
    }
    public class OrderPackageProductListViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Yükseklik")]

        public long? Height { get; set; }
        [Display(Name = "Ağırlık")]

        public long? Weight { get; set; }
        [Display(Name = "En")]

        public long? Width { get; set; }
        [Display(Name = "Boy")]

        public long? Length { get; set; }
        [Display(Name = "Adet")]

        public int? Count { get; set; }
        [Display(Name = "Desi")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal Desi { get; set; }
        public IEnumerable<ProductGroupShowViewModel> OrderPackagedProductGroups { get; set; }

    }
    public class OrderPackageSearchViewModel
    {
        [Display(Name = "Barkod Id")]
        public string SearchName { get; set; }

        [Display(Name = "Sipariş Id")]
        public long? SearchId { get; set; }

        [Display(Name = "Package Id")]
        public long? PackageId { get; set; }


    }
    public class ProductGroupsEditViewModel
    {
        public long Id { get; set; }

        [Display(Name = "İçerik")]
        public string Content { get; set; }
       
        public long? QuantityPerUnit { get; set; }
        public string GtipCode { get; set; }
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
    public class OrderPackageProductEditViewModel
    {
        public OrderPackageProductEditViewModel()
        {
            ProductGroupsEditViewModels = new List<ProductGroupsEditViewModel>();
        }
        public long Id { get; set; }

        public IEnumerable<ProductGroupsEditViewModel> ProductGroupsEditViewModels { get; set; }

  
        public long OrderId { get; set; }

        public string Barcode { get; set; }
    }

}
