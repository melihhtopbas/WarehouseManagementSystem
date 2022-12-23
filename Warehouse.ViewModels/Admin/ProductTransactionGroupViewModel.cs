﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [Range(0, long.MaxValue, ErrorMessage = "Ürün adeti 0 olamaz!")]
        public int Count { get; set; }
        [Display(Name = "Birim Fiyatı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        [Range(0, long.MaxValue, ErrorMessage = "Ürün fiyatı 0 olamaz!")]
        public long QuantityPerUnit { get; set; }
        [Display(Name = "SKU(isteğe bağlı)")]
         
        public string SKU { get; set; }
        [Display(Name = "GtipCode(isteğe bağlı)")]
        public string GtipCode { get; set; }
        public long? OrderId { get; set; }

        public virtual OrderAddViewModel Orders { get; set; }

        public OrderCurrencyUnitIdSelectViewModel CurrenyUnit { get; set; }//yapılmadı

    }
}
