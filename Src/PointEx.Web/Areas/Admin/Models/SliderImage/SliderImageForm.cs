using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Mapping;
using Framework.Common.Web.Metadata;
using PointEx.Entities;
using PointEx.Web.Infrastructure.Extensions;

namespace PointEx.Web.Models
{
    public class SliderImageForm : IMapFrom<SliderImage>
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public int SectionId { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [Remote("IsNameAvailable", "SliderImage", "Admin", ErrorMessage = "Ya existe una imágen con este nombre", AdditionalFields = "Id")]
        public string Name { get; set; }
        
        [Render(ShowForEdit = false)]
        public HttpPostedFileBase ImageFile { get; set; }

        [UIHint("ImageFile")]
        [File(HttpPostedFileBaseProperty = "ImageFile")]

        public File File { get; set; }

        [HiddenInput]
        public int? FileId { get; set; }

        [HiddenInput]
        public bool OriginalFileWasRemoved { get; set; }

        public SliderImage ToSliderImage()
        {
            var sliderImage = new SliderImage()
            {
                File = this.File,
                FileId = this.FileId.GetValueOrDefault(),
                Id = this.Id,
                Name = this.Name
            };

            if (ImageFile != null)
            {
                sliderImage.File = ImageFile.ToFile();
            }
            else
            {
                //if the file was not removed we send the same file content
                if (!OriginalFileWasRemoved && this.FileId != default(int) && this.FileId.HasValue)
                {
                    sliderImage.File = new File
                    {
                        Id = this.FileId.Value,
                        FileContent = new FileContent()
                    };
                }                
            }

            return sliderImage;
        }

        public static SliderImageForm FromSliderImage(SliderImage sliderImage)
        {
            var form = Mapper.Map<SliderImage, SliderImageForm>(sliderImage);
            return form;
        }
    }
}
