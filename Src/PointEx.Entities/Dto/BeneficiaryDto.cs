using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Framework.Common.Mapping;
using PointEx.Entities.Enums;

namespace PointEx.Entities.Dto
{
    public class BeneficiaryDto : IMapFrom<Beneficiary>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? EducationalInstitutionId { get; set; }
        public string EducationalInstitutionName { get; set; }
        public int TownId { get; set; }
        public string TownName { get; set; }
        public string Address { get; set; }
        public string Neighborhood { get; set; }
        public long? TelephoneNumber { get; set; }
        public StatusEnum StatusId { get; set; }
        public string BenefitStatusName
        {
            get
            {
                switch (StatusId)
                {
                    case StatusEnum.Approved:
                        return "Aprobado";
                    case StatusEnum.Rejected:
                        return "Rechazado";
                    default:
                        return "Pendiente";
                }
            }
        }
    }
}
