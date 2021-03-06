//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PointEx.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class BranchOffice
    {
        public BranchOffice()
        {
            this.BenefitBranchOffices = new HashSet<BenefitBranchOffice>();
            this.Purchases = new HashSet<Purchase>();
        }
    
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int TownId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public System.Data.Entity.Spatial.DbGeography Location { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual Shop Shop { get; set; }
        public virtual Town Town { get; set; }
        public virtual ICollection<BenefitBranchOffice> BenefitBranchOffices { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
