﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WarehouseManagementSystemEntities1 : DbContext
    {
        public WarehouseManagementSystemEntities1()
            : base("name=WarehouseManagementSystemEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CargoServiceTypes> CargoServiceTypes { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<CurrencyUnits> CurrencyUnits { get; set; }
        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<RecipientAddresses> RecipientAddresses { get; set; }
        public virtual DbSet<SenderAddresses> SenderAddresses { get; set; }
        public virtual DbSet<Packages> Packages { get; set; }
        public virtual DbSet<ProductTransactionGroup> ProductTransactionGroup { get; set; }
        public virtual DbSet<Sliders> Sliders { get; set; }
        public virtual DbSet<About> About { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<Properties> Properties { get; set; }
        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<FAQ> FAQ { get; set; }
        public virtual DbSet<FAQCategories> FAQCategories { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<References> References { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<ShippingPrices> ShippingPrices { get; set; }
        public virtual DbSet<TicketAnswers> TicketAnswers { get; set; }
        public virtual DbSet<Tickets> Tickets { get; set; }
        public virtual DbSet<PackagedProductGroups> PackagedProductGroups { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
    }
}
