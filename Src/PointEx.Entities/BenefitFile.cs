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
    
    public partial class BenefitFile
    {
        public int Id { get; set; }
        public int BenefitId { get; set; }
        public int FileId { get; set; }
        public int Order { get; set; }
    
        public virtual Benefit Benefit { get; set; }
        public virtual File File { get; set; }
    }
}