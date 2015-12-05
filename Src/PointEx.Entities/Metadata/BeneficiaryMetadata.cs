using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PointEx.Entities
{
    [MetadataType(typeof(BeneficiaryMetadata))]
    public partial class Beneficiary
    {
        public int Points
        {
            get
            {
                var purchases = this.Cards.SelectMany(c => c.Purchases);
                var acumulatedPoints = 0;
                foreach (var purchase in purchases)
                {
                    acumulatedPoints += Convert.ToInt32(Math.Floor(purchase.Amount));
                }
                var exchanges = this.PointsExchanges.Sum(pe => pe.PointsUsed);
                return Convert.ToInt32(acumulatedPoints - exchanges);
            }
        }
    }

    public class BeneficiaryMetadata
    {
        [DisplayName("Ciudad")]
        [Required(ErrorMessage = null)]
        public int TownId { get; set; }

        [DisplayName("Fecha de Creación")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Fecha de Modificación")]
        public DateTime? ModifiedDate { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = null)]
        public string Name { get; set; }

        [DisplayName("Fecha de Nacimiento")]
        [UIHint("Date")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("Dirección")]
        public string Address { get; set; }

        [DisplayName("Teléfono")]
        [StringLength(10)]
        public long? TelephoneNumber { get; set; }

        [DisplayName("Escuela")]
        public int? EducationalInstitutionId { get; set; }

        [DisplayName("Sexo")]
        public int Sex { get; set; }
        
        [DisplayName("Barrio")]
        [Required(ErrorMessage = null)]
        public string Neighborhood { get; set; }
    }
}
