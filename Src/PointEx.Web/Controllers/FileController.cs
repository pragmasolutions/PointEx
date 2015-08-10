using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PointEx.Service;
using Imagenes;

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
        public ActionResult Image(int id, int? width, int? height)
        {
            var file = _fileService.GetById(id);
            if (file == null)
            {
                return HttpNotFound();
            }

            //return Image(file.FileContent.Content, file.ContentType, width, height);
            return new ImageResult(file.FileContent.Content, width??100, height??100, file.ContentType, false, file.ModifiedDate??DateTime.Now, 100);
        }
    }
}