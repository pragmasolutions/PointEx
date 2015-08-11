using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointEx.Entities
{
    public partial class SectionItem
    {
        public int? DefaultFileId
        {
            get
            {
                if (this.Benefit != null)
                    return this.Benefit.DefaultFileId;
                
                if (this.Prize != null)
                    return this.Prize.ImageFileId;

                if (this.SliderImage != null)
                    return this.SliderImage.FileId;
                
                return null;
            }
        }

        public string Description
        {
            get
            {
                if (this.Benefit != null)
                {
                    return this.Benefit.Name;
                }

                if (this.Prize != null)
                {
                    return this.Prize.Name;
                }

                return null;
            }
        }
    }
}
