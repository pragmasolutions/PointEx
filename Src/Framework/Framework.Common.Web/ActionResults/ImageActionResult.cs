using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Simple.ImageResizer;

namespace Framework.Common.Web.ActionResults
{
    public class ImageResult : FileContentResult
    {
        private readonly int? _width;
        private readonly int? _height;

        public ImageResult(byte[] fileBytes, string contentType, int? width = null, int? height = null) :
            base(fileBytes, contentType)
        {
            _width = width;
            _height = height;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            var imageRisizer = new ImageResizer(this.FileContents);

            int width = _width ?? 0;
            int height = _height ?? 0;

            if (!_width.HasValue || !_height.HasValue)
            {
                using (Image originalImage = Image.FromStream(new MemoryStream(this.FileContents)))
                {
                    width = _width ?? originalImage.Width;
                    height = _width ?? originalImage.Height;
                }
            }

            var resizedImage = imageRisizer.Resize(width, height, true, ImageEncoding.Png);

            using (var ms = new MemoryStream(resizedImage))
            {
                ms.WriteTo(response.OutputStream);
            }
        }
    }
}
