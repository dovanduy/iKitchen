using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SunTzu.Web
{
    public class ImageHelper
    {
        private static string folder = HttpContext.Current.Server.MapPath("~/Images/Upload/");
        static ImageHelper()
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

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

        public static List<string> SaveImageMultiple()
        {
            var result = new List<string>();
            var Request = HttpContext.Current.Request;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                string filename = null;
                filename = Request.Files[i].FileName;
                if (string.IsNullOrEmpty(filename))
                {
                    continue;
                }
                filename = filename.Replace(filename.Substring(0, filename.LastIndexOf('.')), Guid.NewGuid().ToString());
                Request.Files[i].SaveAs(folder + filename);
                result.Add(filename);
            }
            return result;
        }

        public static void DeleteImage(string oldImage)
        {
            oldImage = folder + oldImage;
            if (File.Exists(oldImage))
            {
                File.Delete(oldImage);
            }
        }

        public static void CompressImage(string sourceFilename, int dWidth, int dHeight)
        {
            var sourceFilePath = folder + sourceFilename;
            var destFolder = string.Format("{0}{1}x{2}/", folder, dWidth, dHeight);
            if(!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            var destFilePath = destFolder + sourceFilename;
            CompressImage(sourceFilePath, destFilePath, dHeight, dWidth, 100);
        }

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth">宽度</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>
        public static bool CompressImage(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }

        }
    }
}
