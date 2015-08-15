using System.Collections.Generic;
using System.Drawing.Printing;
using PointEx.Entities;

namespace PointEx.Web.Models
{
    public class AddSliderImageModel
    {
        public SliderImageListModel SliderImageListModel { get; set; }

        public IList<int> SelectedSliderImageIds { get; set; }

        public string SectionName { get; set; }

        public int SectionId { get; set; }
    }

    public class AddSliderImageForm
    {
        public int SectionId { get; set; }

        public int SliderImageId { get; set; }
    }
}
