using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Framework.Common.Mapping;

namespace PointEx.Entities.Dto
{
    public class BenefitBranchOfficeDto : IMapFrom<BenefitBranchOffice>
    {
        public int Id { get; set; }
        public int BenefitId { get; set; }
        public int BranchOfficeId { get; set; }
        public string BenefitName { get; set; }
        public string BranchOfficeAddress { get; set; }
    }
}
