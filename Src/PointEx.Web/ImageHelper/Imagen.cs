using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using DESNEA.Web.MVC.Imagenes;

namespace Imagenes
{
    public class ImageResult : ActionResult
    {
        public string ImagenNombreArchivo { get; set; }
        public MemoryStream ImagenStream { get; set; }
        public string ContentType { get; set; }

        //Dimensiones
        public int Alto { get; set; }
        public int Ancho { get; set; }
        public int ResolucionDpi { get; set; }

        public DateTime FechaModificacion { get; set; }

        public ImageResult(string imagenNombreArchivo)
        {
            ImagenNombreArchivo = imagenNombreArchivo;
            ContentType = "image/png"; //FileTypeHelper.GetContentType(ImagenNombreArchivo);
        }

        public ImageResult(MemoryStream imagenStream, string contentType, DateTime fechaModificacion)
        {
            ImagenStream = imagenStream;
            ContentType = contentType;
            this.FechaModificacion = fechaModificacion;


        }

        public ImageResult(byte[] imagen, int ancho, int alto, string contentType, bool mantenerAspecto,
                           DateTime fechaModificacion, int resolucionDpi)
        {
            ConstructorGeneral(imagen, ancho, alto, contentType, mantenerAspecto, fechaModificacion, resolucionDpi);
        }

        private void ConstructorGeneral(byte[] imagen, int ancho, int alto, string contentType, bool mantenerAspecto,
                                        DateTime fechaModificacion, int resolucionDpi)
        {
            this.Ancho = ancho;
            this.Alto = alto;
            this.FechaModificacion = fechaModificacion;
            this.ResolucionDpi = resolucionDpi;


            //HttpContext.Current.Response.ClearHeaders();
            //CacheImagen(600);

            ContentType = contentType;


            ImagenStream = ObtenerMiniaturaFromImage(ConvertirBytesEnImagen(imagen),
                                                     this.Ancho,
                                                     this.Alto,
                                                     FormatoImagen(contentType),
                                                     mantenerAspecto);
        }

        public ImageResult(byte[] imagen, int ancho, int alto, string contentType, bool mantenerAspecto,
                           DateTime fechaModificacion)
        {
            ConstructorGeneral(imagen, ancho, alto, contentType, mantenerAspecto, fechaModificacion, 72);

        }

        public ImageResult(byte[] imagen, int ancho, int alto, string contentType, DateTime fechaModificacion)
        {
            ConstructorGeneral(imagen, ancho, alto, contentType, true, fechaModificacion, 72);
        }

        //public void CacheImagen(int duracion)
        //{

        //    if (duracion <= 0)
        //    {
        //        return;
        //    }
        //    HttpCachePolicy cache = HttpContext.Current.Response.Cache;
        //    TimeSpan cacheDuration = TimeSpan.FromSeconds(duracion);

        //    cache.SetCacheability(HttpCacheability.Public);
        //    cache.SetExpires(DateTime.Now.Add(cacheDuration));
        //    cache.SetMaxAge(cacheDuration);
        //    //cache.AppendCacheExtension("must-revalidate, proxy-revalidate");

        //}

        public static MemoryStream ImagenNoDiponible(int ancho, int alto, bool mantenerAspecto)
        {

            Bitmap imagenNoDisponible =
                new Bitmap(HttpContext.Current.Server.MapPath(@"\Content\Brujula_v1\imagenes\imagenNoDisponible.png"));
            MemoryStream ms = new MemoryStream();
            imagenNoDisponible.Save(ms, ImageFormat.Png);
            byte[] imagen = ms.ToArray();
            return ObtenerMiniaturaFromImage(imagen, ancho, alto, "image/png", mantenerAspecto);
        }

        public static MemoryStream ImagenNoDiponible(int ancho, int alto)
        {
            bool mantenerAspecto = true;
            return ImagenNoDiponible(ancho, alto, mantenerAspecto);
        }

        public static MemoryStream ObtenerMiniaturaFromImage(byte[] input, int ancho, int alto, string contentType,
                                                             bool mantenerAspecto)
        {
            return ObtenerMiniaturaFromImage(ConvertirBytesEnImagen(input), ancho, alto,
                                             FormatoImagenEstatico(contentType));
        }

        public static MemoryStream ObtenerMiniaturaFromImage(Image input, int ancho, int alto, string contentType)
        {
            return ObtenerMiniaturaFromImage(input, ancho, alto, FormatoImagenEstatico(contentType));
        }

        public static MemoryStream ObtenerMiniaturaFromImage(Image imagenOriginal, int ancho, int alto,
                                                             ImageFormat formato, bool mantenerAspecto)
        {
            //Mantener relacion de aspecto
            if (mantenerAspecto)
            {
                if (imagenOriginal.Width > imagenOriginal.Height)
                    alto = imagenOriginal.Height*ancho/imagenOriginal.Width;
                else
                    ancho = imagenOriginal.Width*alto/imagenOriginal.Height;
            }

            //TODO: Optimizar el render de JPG

            var bmp = new Bitmap(imagenOriginal, ancho, alto);
            var ms = new MemoryStream();
            bmp.Save(ms, formato); //
            ms.Position = 0;
            return ms;
        }

        public static MemoryStream ObtenerMiniaturaFromImage(Image input, int ancho, int alto, ImageFormat formato)
        {
            bool mantenerAspecto = true;
            return ObtenerMiniaturaFromImage(input, ancho, alto, formato, mantenerAspecto);
        }

        /// <summary>
        /// Convertir array de bytes en imagen
        /// </summary>
        /// <param name="imagenArray">Array de bytes que representa la imagen</param>
        /// <returns>Imagen</returns>
        public static Image ConvertirBytesEnImagen(byte[] imagenArray)
        {
            System.Drawing.Image imagenNueva;
            using (MemoryStream ms = new MemoryStream(imagenArray, 0, imagenArray.Length))
            {
                ms.Write(imagenArray, 0, imagenArray.Length);
                imagenNueva = Image.FromStream(ms, true);
            }
            return imagenNueva;
        }

        /// <summary>
        /// Convertir array de bytes en MemoryStream
        /// </summary>
        /// <param name="imagenArray">Array de bytes que representa la imagen</param>
        /// <returns>Stream de la imagen</returns>
        public static MemoryStream ConvertirBytesEnStream(byte[] imagenArray)
        {
            return new MemoryStream(imagenArray, 0, imagenArray.Length);
        }

        /// <summary>
        /// Formato de la imagen a partir del ContentType. 
        /// </summary>
        /// <param name="contentType">Cadena contentType</param>
        /// <returns>Formato de la imagen</returns>
        public static ImageFormat FormatoImagenEstatico(string contentType)
        {
            ImageFormat formato;
            switch (contentType)
            {
                case "image/png":
                    formato = ImageFormat.Png;
                    break;

                case "image/jpeg":
                default:
                    formato = ImageFormat.Jpeg;
                    break;
            }

            return formato;
        }

        public ImageFormat FormatoImagen(string contentType)
        {
            return FormatoImagenEstatico(contentType);
        }

        /// <summary>
        /// Formato de la imagen.
        /// </summary>
        /// <returns>Formato de la imagen</returns>
        public ImageFormat FormatoImagen()
        {
            return FormatoImagen(this.ContentType);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var res = context.HttpContext.Response;

            //Duracion de la cache
            //double cacheDurationValue = string.IsNullOrEmpty() ?  : Convert.ToDouble(appsettingValue);
            TimeSpan cacheDuracion = TimeSpan.FromHours(8);

            res.AppendHeader("Last-Modified", FechaModificacion.ToString("r"));
            res.AppendHeader("Connection", "keep-alive");
            res.Cache.SetExpires(DateTime.Now.Add(cacheDuracion));
            res.Cache.SetCacheability(HttpCacheability.Public);
            res.Cache.SetMaxAge(cacheDuracion);
            res.Cache.SetValidUntilExpires(true);

            //res.SuppressContent = true;
            //res.StatusCode = 304;
            //res.StatusDescription = "Not Modified";
            // Explicitly set the Content-Length header so the client doesn't wait for
            // content but keeps the connection open for other requests
            //res.AddHeader("Content-Length", "0");

            res.ContentType = ContentType;

            //Modificando imagen

            if (ImagenStream != null)
            {
                ImagenStream.WriteTo(res.OutputStream);
            }
            else
            {
                res.TransmitFile(ImagenNombreArchivo);
            }

        }
    }
}