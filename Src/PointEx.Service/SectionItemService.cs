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
            return Uow.SectionItems.GetAll(si => si.SectionId == sectionId, bf => bf.Benefit.BenefitFiles, bf => bf.Prize).OrderBy(c => c.Order).ToList();
        }

        public SectionItem GetById(int sectionItemId)
        {
            return Uow.SectionItems.Get(bf => bf.Id == sectionItemId, bf => bf.Benefit, bf => bf.Prize);
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
                var sectionItemToUpdate = sectionItems.Single(bf => bf.Id == sectionItemIdsOrdered[i]);
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
            if (sectionItems.Count + currentItems.Count > section.MaxNumberOfItems)
            {
                throw new ApplicationException("El número de item supero el maximo permitido para la sección");
            }

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
    }
}
