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
    
    public partial class Town
    {
        public Town()
        {
            this.EducationalInstitutions = new HashSet<EducationalInstitution>();
            this.Shops = new HashSet<Shop>();
            this.Students = new HashSet<Student>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<EducationalInstitution> EducationalInstitutions { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}