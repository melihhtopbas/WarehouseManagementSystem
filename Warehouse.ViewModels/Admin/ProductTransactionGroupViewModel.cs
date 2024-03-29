﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;

namespace Warehouse.ViewModels.Admin
{
    public class ProductTransactionGroupViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Ürün İçeriği")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Content { get; set; }
        [Display(Name = "Ürün Adeti")]
        [Required(ErrorMessage = "Lütfen giriniz")]
       
        public int? Count { get; set; }
         
        [Display(Name = "Birim Fiyatı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        
        public long? QuantityPerUnit { get; set; }
        [Required(ErrorMessage = "Lütfen giriniz")]
        [Display(Name = "SKU")]
         
        public string SKU { get; set; }
        [Display(Name = "GtipCode(isteğe bağlı)")]
        public string GtipCode { get; set; }
        public long? OrderId { get; set; }

        public bool? isPackage { get; set; }
        public bool isChecked { get; set; }
        public bool? isReadOnly { get; set; }
        [Display(Name = "Paketlenecek Ürün Adeti")]
        [Range(1, int.MaxValue, ErrorMessage = "0'dan farklı bir değer giriniz!")]

        public int? PackagedCount { get; set; }
        [Display(Name = "Paketlenmemiş Ürün Adeti")]


        public int? isPackagedCount { get; set; }

        public virtual OrderAddViewModel Orders { get; set; }
 

    }
    public class ProductGroupShowViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Ürün İçeriği")]
       
        public string Content { get; set; }
        [Display(Name = "Ürün Adeti")]
         

        public int? Count { get; set; }
        [Display(Name = "Paketlenecek Ürün Adeti")]
        [Range(1, int.MaxValue, ErrorMessage = "0'dan farklı bir değer giriniz!")]

        public int? PackagedCount { get; set; }
        [Display(Name = "Paketlenmemiş Ürün Adeti")]


        public int? isPackagedCount { get; set; }
        [Display(Name = "Birim Fiyatı")]
    
        public long? QuantityPerUnit { get; set; }
        
        [Display(Name = "SKU")]

        public string SKU { get; set; }
        [Display(Name = "GtipCode(isteğe bağlı)")]
        public string GtipCode { get; set; }
        public long? OrderId { get; set; }

        public bool? isPackage { get; set; }
        public bool isChecked { get; set; }
        public bool? isReadOnly { get; set; }

        public virtual OrderAddViewModel Orders { get; set; }

    }
    public class ProductGroupAddViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Ürün İçeriği")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Content { get; set; }
        [Display(Name = "Ürün Adeti")]

        [Required(ErrorMessage = "Lütfen giriniz")]
        public int? Count { get; set; }
 
        [Display(Name = "Birim Fiyatı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? QuantityPerUnit { get; set; }

        [Display(Name = "SKU")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string SKU { get; set; }
        [Display(Name = "GtipCode(isteğe bağlı)")]
        public string GtipCode { get; set; }
        public long? OrderId { get; set; }

     

    }
    public class ProductGroupEditViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Ürün İçeriği")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Content { get; set; }
        [Display(Name = "Ürün Adeti")]

        [Required(ErrorMessage = "Lütfen giriniz")]
        [Range(1, int.MaxValue, ErrorMessage = "0'dan farklı bir değer giriniz!")]
        public int? Count { get; set; }

        [Display(Name = "Birim Fiyatı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public long? QuantityPerUnit { get; set; }

        [Display(Name = "SKU")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string SKU { get; set; }
        [Display(Name = "GtipCode(isteğe bağlı)")]
        public string GtipCode { get; set; }
        public long? OrderId { get; set; }



    }
    public class ProductSearchViewModel
    {
        [Display(Name = "Arama Metni")]
     
        public string SearchName { get; set; }

        [Display(Name = "Sipariş No")]
        public long OrderId { get; set; }

    }
}
