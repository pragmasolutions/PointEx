using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace DESNEA.Web.MVC.Imagenes
{
   public class ImagenUtil 
   {


       public static Image Redimensionar(Image imagen, int ancho, int alto, string bg, float resolucionDpi)
       {
           //Image imagen = Image.FromStream(new MemoryStream(imageFile));
           //int targetH, targetW;
           //if (imagen.Height > imagen.Width)
           //{
           //    targetH = targetSize;
           //    targetW = (int)(imagen.Width * ((float)targetSize / (float)imagen.Height));
           //}
           //else
           //{
           //    targetW = targetSize;
           //    targetH = (int)(imagen.Height * ((float)targetSize / (float)imagen.Width));
           //}


           //Mantener relacion de aspecto
           if (imagen.Width > imagen.Height)
           {
               alto = imagen.Height * ancho / imagen.Width;
           }
           else
           {
               ancho = imagen.Width * alto / imagen.Height;
           }

           Image imagenPhoto = imagen;

           // Create a new blank canvas.  The resized image will be drawn on this canvas.
           Bitmap bmPhoto = new Bitmap(ancho, alto, PixelFormat.Format48bppRgb);
           bmPhoto.SetResolution(resolucionDpi, resolucionDpi);
           Graphics grPhoto = Graphics.FromImage(bmPhoto);

           ColorConverter conv = new ColorConverter();
           Color color = (Color)conv.ConvertFromString(bg);
           grPhoto.Clear(color);

           grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
           grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
           grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
           grPhoto.DrawImage(imagenPhoto, new Rectangle(0, 0, ancho, alto), 0, 0, imagen.Width, imagen.Height - 2, GraphicsUnit.Pixel);
           
           
           // Save out to memory and then to a file.  We dispose of all objects to make sure the files don't stay locked.
           MemoryStream mm = new MemoryStream();
           bmPhoto.Save(mm, ImageFormat.Jpeg);
           
           imagen.Dispose();
           imagenPhoto.Dispose();
           bmPhoto.Dispose();
           grPhoto.Dispose();
           return ByteArrayToImage(mm.GetBuffer());
       }

       public static Image Redimensionar(Image imagen, int ancho, int alto) {
           return Redimensionar(imagen, ancho, alto, "white", 72);
       }

       public static Image Redimensionar(Image imagen, int ancho, int alto, string bg)
       {
           return Redimensionar(imagen, ancho, alto, bg, 72);
       }




       //public static Image FixedSize(Image imgPhoto, int Width, int Height)
       //{
       //    int sourceWidth = imgPhoto.Width;
       //    int sourceHeight = imgPhoto.Height;
       //    int sourceX = 0;
       //    int sourceY = 0;
       //    int destX = 0;
       //    int destY = 0;

       //    float nPercent = 0;
       //    float nPercentW = 0;
       //    float nPercentH = 0;

       //    nPercentW = ((float)Width / (float)sourceWidth);
       //    nPercentH = ((float)Height / (float)sourceHeight);
       //    if (nPercentH < nPercentW)
       //    {
       //        nPercent = nPercentH;
       //        destX = System.Convert.ToInt16((Width -
       //                      (sourceWidth * nPercent)) / 2);
       //    }
       //    else
       //    {
       //        nPercent = nPercentW;
       //        destY = System.Convert.ToInt16((Height -
       //                      (sourceHeight * nPercent)) / 2);
       //    }

       //    int destWidth = (int)(sourceWidth * nPercent);
       //    int destHeight = (int)(sourceHeight * nPercent);

       //    Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
       //    bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

       //    Graphics grPhoto = Graphics.FromImage(bmPhoto);
       //    grPhoto.Clear(Color.Red);
       //    grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

       //    grPhoto.DrawImage(imgPhoto,
       //        new Rectangle(destX, destY, destWidth, destHeight),
       //        new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
       //        GraphicsUnit.Pixel);

       //    grPhoto.Dispose();
       //    return bmPhoto;
       //}



       public static Image Recortar(Image imagen, int ancho, int alto, int posicionX, int posicionY, string bg, float resolucionDpi)
       {
           //Image imagen = Image.FromStream(new MemoryStream(imagenArchivo));
           Bitmap bmPhoto = new Bitmap(ancho, alto, PixelFormat.Format48bppRgb);
           bmPhoto.SetResolution(resolucionDpi, resolucionDpi);
           Graphics grPhoto = Graphics.FromImage(bmPhoto);

           ColorConverter conv = new ColorConverter();
           Color color = (Color)conv.ConvertFromString(bg);
           grPhoto.Clear(color);
           grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
           grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
           grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
           grPhoto.DrawImage(imagen, new Rectangle(0, 0, ancho, alto), posicionX, posicionY, ancho, alto, GraphicsUnit.Pixel);

           // Save out to memory and then to a file.  We dispose of all objects to make sure the files don't stay locked.
           MemoryStream mm = new MemoryStream();
           bmPhoto.Save(mm, ImageFormat.Jpeg);
           imagen.Dispose();
           bmPhoto.Dispose();
           grPhoto.Dispose();
           return ByteArrayToImage(mm.GetBuffer());
       }

       public static Image Recortar(Image imagen, int ancho, int alto, int posicionX, int posicionY, string bg)
       {
           return Recortar(imagen, ancho, alto, posicionX, posicionY, bg, 72);
       }

       public static Image Recortar(Image imagen, int ancho, int alto, int posicionX, int posicionY)
       {
           return Recortar(imagen, ancho, alto, posicionX, posicionY, "White", 72);
       }


    


       /// <summary>
       /// Ajustar imagen a un ancho y alto especifico. Cortando los excedentes y centrando la imagen al punto medio
       /// </summary>
       /// <param name="imagen"></param>
       /// <param name="ancho"></param>
       /// <param name="alto"></param>
       /// <param name="puntoMedioX"></param>
       /// <param name="puntoMedioY"></param>
       /// <param name="bg"></param>
       /// <param name="resolucionDpi"></param>
       /// <returns></returns>
       public static Image Fit(Image imagen, int ancho, int alto, decimal? puntoMedioX, decimal? puntoMedioY, string bg, float resolucionDpi)
       {

           decimal? x = 0;
           decimal? y = 0;

           x = puntoMedioX ?? imagen.Width / 2;
           y = puntoMedioY ?? imagen.Height / 2;


           int anchoRedimiensionar = ancho;
           int altoRedimensionar = alto;

           //ancho y alto de la imagen real
           int imagenAncho = imagen.Width;
           int imagenAlto = imagen.Height;

           //Mantener relacion de aspecto
           if (imagen.Width > imagen.Height)
           {
               //CALCULO UN ANCHO TAL QUE EL ALTO PROPORCIONAL SEA EL REQUERIDO PARA CORTAR
               anchoRedimiensionar = imagen.Width * alto / imagen.Height;

               //Si el ancho es el menos al que ya tenemos dejamos el que tenemos
               if (anchoRedimiensionar < ancho)
               {
                   anchoRedimiensionar = ancho;
               }
           }
           else
           {
               //CALCULO UN ANCHO TAL QUE EL ANCHO PROPORCIONAL SEA EL REQUERIDO PARA CORTAR
               altoRedimensionar = imagen.Height * ancho / imagen.Width;

               if (altoRedimensionar < alto)
               {
                   altoRedimensionar = alto;
               }
           }


           //Redimensiona la imagen
           Image imagenRedimensionada = Redimensionar(imagen, anchoRedimiensionar, altoRedimensionar, bg, resolucionDpi);

           //redimensiona el punto medio
           int nuevoX = (int)(imagenRedimensionada.Width * x / imagenAncho);
           int nuevoY = (int)(imagenRedimensionada.Height * y / imagenAlto);


           //Calculo del vertice x para cortar la imagen
           if (imagenRedimensionada.Width <= ancho)
           {
               nuevoX = 0;
           }
           else
           {
               int PuntoMedioX = (ancho / 2);
               if (nuevoX > PuntoMedioX)
               {
                   nuevoX = nuevoX - PuntoMedioX;
               }
               else
               {
                   nuevoX = 0;
               }

               if ((nuevoX + ancho) >= imagenRedimensionada.Width)
               {
                   // nuevoX = nuevoX + ancho - imagenRedimensionada.Width;

                   nuevoX = imagenRedimensionada.Width - ancho;
               }

           }


           //calculo del vertice y para cortar la imagen
           if (imagenRedimensionada.Height <= alto)
           {
               nuevoY = 0;
           }
           else
           {
               int PuntoMedioY = (alto / 2);
               if (nuevoY > PuntoMedioY)
               {
                   nuevoY = nuevoY - PuntoMedioY;
               }
               else
               {
                   nuevoY = 0;
               }


               if ((nuevoY + alto) >= imagenRedimensionada.Height)
               {
                   // nuevoY = nuevoY + alto - imagenRedimensionada.Height;
                   nuevoY = imagenRedimensionada.Height - alto;
               }

           }

           Image imagenRecortada = Recortar(imagenRedimensionada, ancho, alto, nuevoX, nuevoY, bg, resolucionDpi);

           return imagenRecortada;

       }
       
       public static Image Fit(Image imagen, int ancho, int alto, decimal? puntoMedioX, decimal? puntoMedioY, string bg)
       {
           return Fit(imagen, ancho, alto, puntoMedioX, puntoMedioY, bg, 72);
       } 

       public static Image Fit(Image imagen, int ancho, int alto, decimal? puntoMedioX, decimal? puntoMedioY)
       {
           return Fit(imagen, ancho, alto, puntoMedioX, puntoMedioY, "White", 72);           
       }

       /// <summary>
       /// Ajustar imagen a un ancho y alto especifico. Cortando los excedentes y centrando la imagen al punto medio
       /// </summary>
       /// <param name="imagenEnBytes"></param>
       /// <param name="ancho"></param>
       /// <param name="alto"></param>
       /// <param name="puntoMedioX"></param>
       /// <param name="puntoMedioY"></param>
       /// <param name="bg"></param>
       /// <param name="resolucionDpi"></param>
       /// <returns></returns>
       public static Image Fit(byte[] imagenEnBytes, int ancho, int alto, decimal? puntoMedioX, decimal? puntoMedioY, string bg, float resolucionDpi)
       {
           Image imagen = ByteArrayToImage(imagenEnBytes);
           return Fit(imagen, ancho, alto, puntoMedioX, puntoMedioY, bg, resolucionDpi);
       }

       public static Image Fit(byte[] imagenEnBytes, int ancho, int alto, decimal? puntoMedioX, decimal? puntoMedioY, string bg)
       {
           Image imagen = ByteArrayToImage(imagenEnBytes);
           return Fit(imagen, ancho, alto, puntoMedioX, puntoMedioY, bg);
       }

       public static Image Fit(byte[] imagenEnBytes, int ancho, int alto, decimal? puntoMedioX, decimal? puntoMedioY)
       {
           Image imagen = ByteArrayToImage(imagenEnBytes);
           return Fit(imagen, ancho, alto, puntoMedioX, puntoMedioY);
       }



       public static Image ByteArrayToImage(byte[] arregloDeBytes)
       {
           MemoryStream ms = new MemoryStream(arregloDeBytes);
           Image imagen = Image.FromStream(ms);
           return imagen;
       }


       public static byte[] ImageToByteArray(System.Drawing.Image imagen, ImageFormat formato)
       {
           MemoryStream ms = new MemoryStream();
           if (imagen!=null)
           {
               imagen.Save(ms, formato); 
           }
           return ms.ToArray();
       }


   }
  
}
