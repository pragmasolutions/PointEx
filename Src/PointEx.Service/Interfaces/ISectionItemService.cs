using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface ISectionItemService
    {
        IList<SectionItem> GetBySectionId(int sectionId);

        IList<SectionItem> GetBySectionName(string sectionName);

        void Create(SectionItem sectionItem);

        void Create(IList<SectionItem> sectionItems);

        void Delete(int sectionItemId);

        void Order(int sectionId, IList<int> sectionItemIdsOrdered);

        IList<SectionItem> GetBySliderImage(int sliderImageId);
    }
}