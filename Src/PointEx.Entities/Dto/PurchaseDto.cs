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
    public class PurchaseDto : IMapFrom<Purchase>
    {
        public int Id { get; set; }
        public string CardBeneficiaryName { get; set; }
        public System.DateTime PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public string BenefitName { get; set; }
        public string BranchOfficeAddress { get; set; }
    }
}
