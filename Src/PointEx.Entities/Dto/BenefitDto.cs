using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Framework.Common.Mapping;
using PointEx.Entities.Enums;

namespace PointEx.Entities.Dto
{
    public class BenefitDto : IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BenefitTypesEnum? BenefitTypeId { get; set; }
        public string BenefitTypeName { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountPercentageCeiling { get; set; }
        public int? DefaultFileId { get; set; }
        public string ShopName { get; set; }
        public DbGeography ShopLocation { get; set; }
        public StatusEnum StatusId { get; set; }
        public string BenefitStatusName 
        { 
            get {
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
        public void CreateMappings(IConfiguration configuration)
        {
            Mapper.CreateMap<Benefit, BenefitDto>()
                .ForMember(dest => dest.DefaultFileId, opt => opt.MapFrom(src => src.BenefitFiles.OrderBy(bf => bf.Order)
                .FirstOrDefault().FileId));
        }
    }
}
