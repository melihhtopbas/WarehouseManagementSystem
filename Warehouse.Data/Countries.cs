//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Warehouse.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Countries
    {
        public Countries()
        {
            this.Orders = new HashSet<Orders>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Orders> Orders { get; set; }
    }
}
