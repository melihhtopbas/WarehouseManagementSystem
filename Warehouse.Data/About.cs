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
    
    public partial class About
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long LanguageId { get; set; }
        public string FileName { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string FileName2 { get; set; }
    
        public virtual Languages Languages { get; set; }
    }
}