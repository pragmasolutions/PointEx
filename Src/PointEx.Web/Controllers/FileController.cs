using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Service;
using Simple.ImageResizer;

namespace PointEx.Web.Controllers
{
    public class FileController : BaseController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        // GET: File
        public ActionResult Image(int id, int? width, int? heigh)
        {
            var file = _fileService.GetById(id);
            if (file == null)
            {
                return HttpNotFound();
            }

            return Image(file.FileContent.Content, file.ContentType, width ?? 0, heigh ?? 0);
        }
    }

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

            var resizedImage = imageRisizer.Resize(width, height,false, ImageEncoding.Png);

            using (var ms = new MemoryStream(resizedImage))
            {
                ms.WriteTo(response.OutputStream);
            }
        }
    }
}