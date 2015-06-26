using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PointEx.Entities.Models
{
    public class InformationRequestModel
    {
        [DisplayName("Motivo")]
        [UIHint("InformationRequestCause")]
        [Required]
        public string Cause { get; set; }

        [DisplayName("Consulta")]
        [Required]
        public string Text { get; set; }
        
        [DisplayName("Apellido y Nombres")]
        [Required]
        public string Name { get; set; }
        
        [DisplayName("Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Teléfono")]
        [Required]
        public string PhoneNumber { get; set; }

    }
}