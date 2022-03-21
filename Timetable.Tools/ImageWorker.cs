using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Timetable.Tools
{
    public static class ImageWorker
    {
        private static Dictionary<string, List<byte[]>> headDic = new Dictionary<string, List<byte[]>>
        {
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                }
            }, {
                ".jpg", new List<byte[]>{
                    new byte[] { 0xFF, 0xD8, 0xFF ,0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            }, {
                ".png",new List<byte[]>{
                    new byte[]{ 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
                }
            }, {
                ".bmp",new List<byte[]>{
                    new byte[]{ 0x42, 0x4D }
                }
            }, {
                ".gif",new List<byte[]>{
                    new byte[]{   0x47, 0x49, 0x46, 0x38 }
                }
            }
        };
        /// <summary>
        ///     验证图片文件并获得图片类型
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ImageFormat ValidateGetImageFormat(string filename, Stream stream)
        {

            ImageFormat imageFormat = null;
            string[] permittedExtensions = { ".jpg", ".png", ".jpeg", ".bmp", ".gif" };
            var ext = Path.GetExtension(filename).ToLowerInvariant();
            if (!string.IsNullOrEmpty(ext) && permittedExtensions.Contains(ext))
            {
                bool isValid = false;
                BinaryReader reader = new BinaryReader(stream);
                var heads = headDic[ext];
                var headerBytes = reader.ReadBytes(heads.Max(s => s.Length));
                isValid = heads.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));

                if (isValid)
                {
                    switch (ext)
                    {
                        case ".jpeg":
                            imageFormat = ImageFormat.Jpeg;
                            break;
                        case ".png":
                            imageFormat = ImageFormat.Png;
                            break;
                        case ".gif":
                            imageFormat = ImageFormat.Gif;
                            break;
                        case ".jpg":
                            imageFormat = ImageFormat.Jpeg;
                            break;
                        case "bmp":
                            imageFormat = ImageFormat.Bmp;
                            break;
                    }
                }
            }
            return imageFormat;
        }

        #region 图片缩放，多种指定方式生成图片
        /// <summary>
        /// 图片缩放
        /// </summary>
        /// <param name="postedFile">图片文件</param>
        /// <param name="thumbnailPath">生成缩略图图片路径，如：c:\\images\\2.gif</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="mode">EQU：指定最大高宽等比例缩放；HW：//指定高宽缩放（可能变形）；W:指定宽，高按比例；H:指定高，宽按比例；Cut：指定高宽裁减（不变形）</param>
        public static void MakeThumbnail(Stream postedFileStream, string thumbnailPath, int width, int height, string mode, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            postedFileStream.Position = 0;
            System.Drawing.Image originalImage = System.Drawing.Image.FromStream(postedFileStream);
            int towidth = originalImage.Width;
            int toheight = originalImage.Height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            if (ow > width || oh > height)
            {
                towidth = width;
                toheight = height;
                if (mode == "EQU")//指定最大高宽，等比例缩放
                {
                    //if(height/oh>width/ow),如果高比例多，按照宽来缩放；如果宽的比例多，按照高来缩放
                    if (height * ow > width * oh)
                    {
                        mode = "W";
                    }
                    else
                    {
                        mode = "H";
                    }
                }
                switch (mode)
                {
                    case "HW"://指定高宽缩放（可能变形）                   
                        break;
                    case "W"://指定宽，高按比例                       
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case "H"://指定高，宽按比例   
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case "Cut"://指定高宽裁减（不变形）                   
                        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * height / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    default:
                        break;
                }
            }

            //新建一个bmp图片   
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板   
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法   
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            //设置高质量,低速度呈现平滑程度   
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充   
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分   
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);
            try
            {
                // ImageCodecInfo tiffEncoder = GetEncoder(ImageFormat.Tiff);
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
                ImageCodecInfo encoder = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.FormatID == imageFormat.Guid)
                    {
                        encoder = codec;
                    }
                }

                var encoderParams = new EncoderParameters(1);
                // encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)88);
                //encoderParams.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                // encoderParams.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 1);
                //encoderParams.Param[4] = new EncoderParameter(System.Drawing.Imaging.Encoder.Version, (long)EncoderValue.VersionGif87);
                bitmap.Save(thumbnailPath, encoder, encoderParams);

            }
            catch (System.Exception e)
            {

            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }

        }
        #endregion
    }
}
