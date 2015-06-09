using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PointEx.Entities
{
    [MetadataType(typeof(StudentMetadata))]
    public partial class Student: Beneficiary
    {
        
    }

    public class StudentMetadata: BeneficiaryMetadata
    {
        [DisplayName("Escuela")]
        [Required(ErrorMessage = null)]
        public int? EducationalInstitutionId { get; set; }
    }
}
