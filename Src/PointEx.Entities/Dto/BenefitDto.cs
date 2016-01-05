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

        public bool GetDistanceFromLatLonInKm(double lat2, double lon2, int distance)
        {
            var r = 6371; // Radius of the earth in km
            var dLat = Deg2Rad(lat2 - this.ShopLocation.Latitude ?? 0);  // deg2rad below
            var dLon = Deg2Rad(lon2 - this.ShopLocation.Longitude ?? 0);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Deg2Rad(this.ShopLocation.Latitude ?? 0)) * Math.Cos(Deg2Rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = r * c; // Distance in km
            return (distance <= d);
        }

        private double Deg2Rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
