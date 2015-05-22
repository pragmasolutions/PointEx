﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PointEx.Entities;

namespace PointEx.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PointExDbContext : DbContext
    {
        public PointExDbContext()
            : base("name=PointExDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<EducationalInstitution> EducationalInstitutions { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<Benefit> Benefits { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ShopCategory> ShopCategories { get; set; }
        public virtual DbSet<Beneficiary> Beneficiaries { get; set; }
        public virtual DbSet<Prize> Prizes { get; set; }
        public virtual DbSet<PointsExchange> PointsExchanges { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<FileContent> FileContents { get; set; }
    }
}
