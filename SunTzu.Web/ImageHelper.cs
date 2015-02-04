using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SunTzu.Web
{
   public class ImageHelper
   {
      private static string folder = HttpContext.Current.Server.MapPath("~/Images/Upload/");
      public static string SaveImage()
      {
         string filename = null;
         var Request = HttpContext.Current.Request;
         if (Request.Files.Count > 0 && !string.IsNullOrEmpty(Request.Files[0].FileName))
         {
            filename = Request.Files[0].FileName;
            filename = filename.Replace(filename.Substring(0, filename.LastIndexOf('.')), Guid.NewGuid().ToString());
            if (!Directory.Exists(folder))
            {
               Directory.CreateDirectory(folder);
            }
            Request.Files[0].SaveAs(folder + filename);
         }
         return filename;
      }

      public static void DeleteImage(string oldImage)
      {
         oldImage = folder + oldImage;
         if (File.Exists(oldImage))
         {
            File.Delete(oldImage);
         }
      }
   }
}
