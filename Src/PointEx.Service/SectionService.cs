using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
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
    public class SectionService : ServiceBase, ISectionService
    {
        private readonly IClock _clock;

        public SectionService(IPointExUow uow, IClock clock)
        {
            _clock = clock;
            Uow = uow;
        }

        public IList<Section> GetAll()
        {
            return Uow.Sections.GetAll().ToList();
        }
        
        public Section GetById(int id)
        {
            return Uow.Sections.Get(id);
        }

        public Section GetByName(string name)
        {
            return Uow.Sections.Get(e => e.Name == name);
        }
    }
}
