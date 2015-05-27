using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PointEx.Entities;

namespace PointEx.Web.Infrastructure.Extensions
{
    public static class HttpPostedFileBaseExtensions
    {
        public static File ToFile(this HttpPostedFileBase httpPostedFileBase)
        {
            var file = new File
            {
                FileContent = new FileContent()
            };

            file.Name = System.IO.Path.GetFileName(httpPostedFileBase.FileName);
            file.ContentType = httpPostedFileBase.ContentType;
            using (var reader = new System.IO.BinaryReader(httpPostedFileBase.InputStream))
            {
                file.FileContent.Content = reader.ReadBytes(httpPostedFileBase.ContentLength);
            }

            return file;
        }
    }
}