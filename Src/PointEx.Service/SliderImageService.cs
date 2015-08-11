using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public class SliderImageService : ServiceBase, ISliderImageService
    {
        private readonly IClock _clock;

        public SliderImageService(IPointExUow uow, IClock clock)
        {
            _clock = clock;
            Uow = uow;
        }

        public IQueryable<SliderImage> GetAll()
        {
            return Uow.SliderImages.GetAll();
        }

        public List<SliderImageDto> GetAll(string sortBy, string sortDirection, string criteria, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<SliderImage, bool>> where = x => ((string.IsNullOrEmpty(criteria) || x.Name.Contains(criteria)));

            var results = Uow.SliderImages.GetAll(pagingCriteria, where);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<SliderImageDto>().ToList();
        }

        public SliderImage GetById(int id)
        {
            return Uow.SliderImages.Get(p => p.Id == id, p => p.File);
        }

        public SliderImage GetByName(string name)
        {
            return Uow.SliderImages.Get(e => e.Name == name);
        }

        public void Create(SliderImage sliderImage)
        {
            if (!IsNameAvailable(sliderImage.Name, sliderImage.Id))
            {
                throw new ApplicationException("Una imágen con el mismo nombre ya ha sido creada");
            }
            sliderImage.CreatedDate = _clock.Now;           
            Uow.SliderImages.Add(sliderImage);
            Uow.Commit();
        }

        public void Edit(SliderImage sliderImage)
        {
            var currentSliderImage = this.GetById(sliderImage.Id);

            if (currentSliderImage.File != null && sliderImage.File == null)
            {
                Uow.FileContents.Delete(currentSliderImage.File.Id);
                Uow.Files.Delete(currentSliderImage.File.Id);
            }
            else if (currentSliderImage.File == null && sliderImage.File != null)
            {
                //Add new image
                sliderImage.File.CreatedDate = _clock.Now;
                currentSliderImage.File = sliderImage.File;
            }
            else if (currentSliderImage.File != null && sliderImage.File != null && sliderImage.File.FileContent != null && sliderImage.File.FileContent.Content != null)
            {
                //Edit actual
                currentSliderImage.File.Name = sliderImage.File.Name;
                currentSliderImage.File.ContentType = sliderImage.File.ContentType;
                currentSliderImage.File.ModifiedDate = _clock.Now;

                //Edit content
                sliderImage.File.FileContent.Id = currentSliderImage.File.Id;
                Uow.FileContents.Edit(sliderImage.File.FileContent);
            }

            currentSliderImage.ModifiedDate = _clock.Now;
            currentSliderImage.Name = sliderImage.Name;
            
            Uow.SliderImages.Edit(currentSliderImage);
            Uow.Commit();
        }

        public void Delete(int sliderImageId)
        {
            var currentSliderImage = GetById(sliderImageId);

            if (currentSliderImage.File != null)
            {
                Uow.FileContents.Delete(currentSliderImage.File.Id);
                Uow.Files.Delete(currentSliderImage.File.Id);
            }

            Uow.SliderImages.Delete(sliderImageId);
            Uow.Commit();
        }

        public bool IsNameAvailable(string name, int id)
        {
            var currentSliderImage = this.GetByName(name);

            if (currentSliderImage == null)
            {
                return true;
            }

            return currentSliderImage.Id == id;
        }
    }
}
