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
    
    public partial class PackagedProductGroups
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<long> QuantityPerUnit { get; set; }
        public string SKU { get; set; }
        public string GtipCode { get; set; }
        public Nullable<long> PackageId { get; set; }
        public Nullable<long> ProductId { get; set; }
    
        public virtual Packages Packages { get; set; }
    }
}
