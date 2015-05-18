using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using PointEx.Entities;
using PointEx.Service;

namespace PointEx.Web.Mapping
{
    public class ShopMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<int, Category>().ConvertUsing(id =>
            {
                var categoryService = IocContainer.GetContainer().Get<ICategoryService>();
                return categoryService.GetById(id);
            });
            Mapper.CreateMap<Category, int>().ConvertUsing((c => c.Id));
        }
    }
}