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
    public class PrizeForm : IMapFrom<Prize>
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [Remote("IsNameAvailable", "Prize", "Admin", ErrorMessage = "Ya existe un premio con este nombre", AdditionalFields = "Id")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Puntos necesarios")]
        [Range(1, int.MaxValue)]
        public int? PointsNeeded { get; set; }

        [Display(Name = @"Descripción")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Render(ShowForEdit = false)]
        public HttpPostedFileBase ImageFile { get; set; }

        [UIHint("ImageFile")]
        [File(HttpPostedFileBaseProperty = "ImageFile")]

        public File File { get; set; }

        [HiddenInput]
        public int? ImageFileId { get; set; }

        [HiddenInput]
        public bool OriginalFileWasRemoved { get; set; }

        public Prize ToPrize()
        {
            var prize = Mapper.Map<PrizeForm, Prize>(this);

            if (ImageFile != null)
            {
                prize.File = ImageFile.ToFile();
            }
            else
            {
                //if the file was not removed we send the same file content
                if (!OriginalFileWasRemoved && this.ImageFileId != default(int) && this.ImageFileId.HasValue)
                {
                    prize.File = new File
                    {
                        Id = this.ImageFileId.Value,
                        FileContent = new FileContent()
                    };
                }                
            }

            return prize;
        }

        public static PrizeForm FromPrize(Prize prize)
        {
            var form = Mapper.Map<Prize, PrizeForm>(prize);
            return form;
        }
    }
}
