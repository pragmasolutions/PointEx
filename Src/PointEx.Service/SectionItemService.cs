using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Common.Utility;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Service.Exceptions;

namespace PointEx.Service
{
    public class SectionItemService : ServiceBase, ISectionItemService
    {
        private readonly ISectionService _sectionService;
        private readonly IClock _clock;

        public SectionItemService(IPointExUow uow, ISectionService sectionService, IClock clock)
        {
            _sectionService = sectionService;
            _clock = clock;
            Uow = uow;
        }

        public IList<SectionItem> GetBySectionId(int sectionId)
        {
            return Uow.SectionItems.GetAll(si => si.SectionId == sectionId, 
                            si => si.Benefit.BenefitFiles, 
                            si => si.Prize,
                            si => si.SliderImage).OrderBy(c => c.Order).ToList();
        }

        public SectionItem GetById(int sectionItemId)
        {
            return Uow.SectionItems.Get(si => si.Id == sectionItemId, si => si.Benefit, si => si.Prize);
        }

        public IList<SectionItem> GetBySectionName(string sectionName)
        {
            return Uow.SectionItems.GetAll(si => si.Section.Name == sectionName, 
                                        si => si.Benefit.BenefitFiles, 
                                        si => si.Prize,
                                        si => si.SliderImage, 
                                        si => si.Section).OrderBy(c => c.Order).ToList();
        }

        public void Order(int sectionItemId, IList<int> sectionItemIdsOrdered)
        {
            var sectionItems = this.GetBySectionId(sectionItemId);

            if (sectionItemIdsOrdered.Count != sectionItems.Count)
            {
                throw new ApplicationException("La cantidad de imagenes ha cambiado");
            }

            for (int i = 0; i < sectionItemIdsOrdered.Count; i++)
            {
                var sectionItemToUpdate = sectionItems.Single(si => si.Id == sectionItemIdsOrdered[i]);
                sectionItemToUpdate.Order = i + 1;
                Uow.SectionItems.Edit(sectionItemToUpdate);
            }

            Uow.Commit();
        }

        public void Create(SectionItem sectionItem)
        {
            CreateInternal(sectionItem);
            Uow.Commit();
        }

        public void Create(IList<SectionItem> sectionItems)
        {
            if (!sectionItems.Any())
            {
                return;
            }

            var section = _sectionService.GetById(sectionItems.First().SectionId);
            var currentItems = GetBySectionId(sectionItems.First().SectionId);

            foreach (var sectionItem in sectionItems)
            {
                CreateInternal(sectionItem);
            }
            Uow.Commit();
        }

        private void CreateInternal(SectionItem sectionItem)
        {
            Uow.SectionItems.Add(sectionItem);
        }

        public void Delete(int sectionItemId)
        {
            Uow.SectionItems.Delete(sectionItemId);
            Uow.Commit();
        }
        
        public IList<SectionItem> GetBySliderImage(int sliderImageId)
        {
            return Uow.SectionItems.GetAll().Where(s => s.SliderImageId == sliderImageId).ToList();
        }
    }
}
