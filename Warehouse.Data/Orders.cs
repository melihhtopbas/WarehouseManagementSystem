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
    
    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            this.RecipientAddresses = new HashSet<RecipientAddresses>();
            this.SenderAddresses = new HashSet<SenderAddresses>();
            this.Packages = new HashSet<Packages>();
            this.ProductTransactionGroup = new HashSet<ProductTransactionGroup>();
        }
    
        public long Id { get; set; }
        public string SenderName { get; set; }
        public string SenderMail { get; set; }
        public string SenderInvoiceNumber { get; set; }
        public string SenderPhone { get; set; }
        public string SenderIdentityNumber { get; set; }
        public string RecipientName { get; set; }
        public string RecipientCity { get; set; }
        public Nullable<long> RecipientCountryId { get; set; }
        public string RecipientZipCode { get; set; }
        public string RecipientInvoiceNumber { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientIdentityNumber { get; set; }
        public string RecipientMail { get; set; }
        public Nullable<long> ProductCurrencyUnitId { get; set; }
        public string ProductOrderDescription { get; set; }
        public Nullable<long> CargoServiceTypeId { get; set; }
        public Nullable<int> PackageCount { get; set; }
        public Nullable<long> LanguageId { get; set; }
        public Nullable<bool> isPackage { get; set; }
    
        public virtual CargoServiceTypes CargoServiceTypes { get; set; }
        public virtual Countries Countries { get; set; }
        public virtual CurrencyUnits CurrencyUnits { get; set; }
        public virtual Languages Languages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecipientAddresses> RecipientAddresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SenderAddresses> SenderAddresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Packages> Packages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductTransactionGroup> ProductTransactionGroup { get; set; }
    }
}
