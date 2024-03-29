//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Warehouse.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShippingPrices
    {
        public long Id { get; set; }
        public Nullable<long> CargoServiceId { get; set; }
        public Nullable<long> LanguageId { get; set; }
        public bool Active { get; set; }
        public Nullable<long> CountryId { get; set; }
        public decimal Price { get; set; }
        public string DeliveryTime { get; set; }
    
        public virtual CargoServiceTypes CargoServiceTypes { get; set; }
        public virtual Countries Countries { get; set; }
        public virtual Languages Languages { get; set; }
    }
}
