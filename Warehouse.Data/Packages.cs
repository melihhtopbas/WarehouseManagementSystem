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
    
    public partial class Packages
    {
        public long Id { get; set; }
        public long Height { get; set; }
        public long Width { get; set; }
        public long Weight { get; set; }
        public long Length { get; set; }
        public Nullable<long> OrderId { get; set; }
    
        public virtual Orders Orders { get; set; }
    }
}
