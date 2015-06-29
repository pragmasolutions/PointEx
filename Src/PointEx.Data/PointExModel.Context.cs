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
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
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
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ShopCategory> ShopCategories { get; set; }
        public virtual DbSet<Beneficiary> Beneficiaries { get; set; }
        public virtual DbSet<Prize> Prizes { get; set; }
        public virtual DbSet<PointsExchange> PointsExchanges { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<FileContent> FileContents { get; set; }
        public virtual DbSet<Benefit> Benefits { get; set; }
        public virtual DbSet<BenefitFile> BenefitFiles { get; set; }
        public virtual DbSet<BranchOffice> BranchOffices { get; set; }
        public virtual DbSet<BenefitBranchOffice> BenefitBranchOffices { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<SectionItem> SectionItems { get; set; }
        public virtual DbSet<BenefitType> BenefitTypes { get; set; }
    
        public virtual ObjectResult<PointTransaction> BeneficiaryPurchasesAndExchanges(Nullable<int> beneficiaryId)
        {
            var beneficiaryIdParameter = beneficiaryId.HasValue ?
                new ObjectParameter("BeneficiaryId", beneficiaryId) :
                new ObjectParameter("BeneficiaryId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PointTransaction>("BeneficiaryPurchasesAndExchanges", beneficiaryIdParameter);
        }
    
        public virtual ObjectResult<RptPurchases> RptPurchases(Nullable<System.DateTime> from, Nullable<System.DateTime> to, Nullable<int> shopId, Nullable<int> educationalInstitutionId)
        {
            var fromParameter = from.HasValue ?
                new ObjectParameter("From", from) :
                new ObjectParameter("From", typeof(System.DateTime));
    
            var toParameter = to.HasValue ?
                new ObjectParameter("To", to) :
                new ObjectParameter("To", typeof(System.DateTime));
    
            var shopIdParameter = shopId.HasValue ?
                new ObjectParameter("ShopId", shopId) :
                new ObjectParameter("ShopId", typeof(int));
    
            var educationalInstitutionIdParameter = educationalInstitutionId.HasValue ?
                new ObjectParameter("EducationalInstitutionId", educationalInstitutionId) :
                new ObjectParameter("EducationalInstitutionId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RptPurchases>("RptPurchases", fromParameter, toParameter, shopIdParameter, educationalInstitutionIdParameter);
        }
    
        public virtual ObjectResult<RptMostExchangedPrizes> RptMostExchangedPrizes(Nullable<System.DateTime> from, Nullable<System.DateTime> to, Nullable<int> educationalInstitutionId)
        {
            var fromParameter = from.HasValue ?
                new ObjectParameter("From", from) :
                new ObjectParameter("From", typeof(System.DateTime));
    
            var toParameter = to.HasValue ?
                new ObjectParameter("To", to) :
                new ObjectParameter("To", typeof(System.DateTime));
    
            var educationalInstitutionIdParameter = educationalInstitutionId.HasValue ?
                new ObjectParameter("EducationalInstitutionId", educationalInstitutionId) :
                new ObjectParameter("EducationalInstitutionId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RptMostExchangedPrizes>("RptMostExchangedPrizes", fromParameter, toParameter, educationalInstitutionIdParameter);
        }
    
        public virtual ObjectResult<RptGeneratedPoints> RptGeneratedPoints(Nullable<System.DateTime> from, Nullable<System.DateTime> to, Nullable<int> shopId, Nullable<int> beneficiaryId, Nullable<int> educationalInstitutionId)
        {
            var fromParameter = from.HasValue ?
                new ObjectParameter("From", from) :
                new ObjectParameter("From", typeof(System.DateTime));
    
            var toParameter = to.HasValue ?
                new ObjectParameter("To", to) :
                new ObjectParameter("To", typeof(System.DateTime));
    
            var shopIdParameter = shopId.HasValue ?
                new ObjectParameter("ShopId", shopId) :
                new ObjectParameter("ShopId", typeof(int));
    
            var beneficiaryIdParameter = beneficiaryId.HasValue ?
                new ObjectParameter("BeneficiaryId", beneficiaryId) :
                new ObjectParameter("BeneficiaryId", typeof(int));
    
            var educationalInstitutionIdParameter = educationalInstitutionId.HasValue ?
                new ObjectParameter("EducationalInstitutionId", educationalInstitutionId) :
                new ObjectParameter("EducationalInstitutionId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RptGeneratedPoints>("RptGeneratedPoints", fromParameter, toParameter, shopIdParameter, beneficiaryIdParameter, educationalInstitutionIdParameter);
        }
    
        public virtual ObjectResult<RptMostUsedBenefits> RptMostUsedBenefits(Nullable<System.DateTime> from, Nullable<System.DateTime> to, Nullable<int> shopId, Nullable<int> educationalInstitutionId)
        {
            var fromParameter = from.HasValue ?
                new ObjectParameter("From", from) :
                new ObjectParameter("From", typeof(System.DateTime));
    
            var toParameter = to.HasValue ?
                new ObjectParameter("To", to) :
                new ObjectParameter("To", typeof(System.DateTime));
    
            var shopIdParameter = shopId.HasValue ?
                new ObjectParameter("ShopId", shopId) :
                new ObjectParameter("ShopId", typeof(int));
    
            var educationalInstitutionIdParameter = educationalInstitutionId.HasValue ?
                new ObjectParameter("EducationalInstitutionId", educationalInstitutionId) :
                new ObjectParameter("EducationalInstitutionId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RptMostUsedBenefits>("RptMostUsedBenefits", fromParameter, toParameter, shopIdParameter, educationalInstitutionIdParameter);
        }
    
        public virtual ObjectResult<RptBenefitsUsed> RptBenefitsUsed(Nullable<System.DateTime> from, Nullable<System.DateTime> to, Nullable<int> shopId)
        {
            var fromParameter = from.HasValue ?
                new ObjectParameter("From", from) :
                new ObjectParameter("From", typeof(System.DateTime));
    
            var toParameter = to.HasValue ?
                new ObjectParameter("To", to) :
                new ObjectParameter("To", typeof(System.DateTime));
    
            var shopIdParameter = shopId.HasValue ?
                new ObjectParameter("ShopId", shopId) :
                new ObjectParameter("ShopId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RptBenefitsUsed>("RptBenefitsUsed", fromParameter, toParameter, shopIdParameter);
        }
    
        public virtual ObjectResult<RptBenefitsUsedChart> RptBenefitsUsedChart(Nullable<System.DateTime> from, Nullable<System.DateTime> to, Nullable<int> shopId)
        {
            var fromParameter = from.HasValue ?
                new ObjectParameter("From", from) :
                new ObjectParameter("From", typeof(System.DateTime));
    
            var toParameter = to.HasValue ?
                new ObjectParameter("To", to) :
                new ObjectParameter("To", typeof(System.DateTime));
    
            var shopIdParameter = shopId.HasValue ?
                new ObjectParameter("ShopId", shopId) :
                new ObjectParameter("ShopId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RptBenefitsUsedChart>("RptBenefitsUsedChart", fromParameter, toParameter, shopIdParameter);
        }
    }
}
