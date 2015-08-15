using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public interface ISliderImageService
    {
        IQueryable<SliderImage> GetAll();

        SliderImage GetById(int id);

        List<SliderImageDto> GetAll(string sortBy, string sortDirection, string criteria, 
                                int pageIndex, int pageSize, out int pageTotal);

        void Create(SliderImage sliderImage);

        void Edit(SliderImage sliderImage);

        void Delete(int sliderImageId);

        bool IsNameAvailable(string name, int id);
    }
}