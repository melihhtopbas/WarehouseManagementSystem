﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WarehouseManagementSystemEntities : DbContext
    {
        public WarehouseManagementSystemEntities()
            : base("name=WarehouseManagementSystemEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CargoServiceTypes> CargoServiceTypes { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<CurrencyUnits> CurrencyUnits { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<ProductTransactionGroup> ProductTransactionGroup { get; set; }
    }
}