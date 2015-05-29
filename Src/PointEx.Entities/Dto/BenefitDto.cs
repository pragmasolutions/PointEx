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
    public class BenefitDto : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountPercentageCeiling { get; set; }
        public string DefaultFileId { get; set; }
        public string ShopName { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            Mapper.CreateMap<Benefit, BenefitDto>()
                .ForMember(dest => dest.DefaultFileId, opt => opt.MapFrom(src => src.BenefitFiles.FirstOrDefault().FileId));
        }
    }
}
