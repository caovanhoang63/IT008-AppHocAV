using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace IT008_AppHocAV.Util
{
    public class BitmapConverter
    {
        /// <summary>
        /// Convert a byte array to a BitmapImage object
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <returns></returns>
        public static Byte[] ConvertToByteFromBitmapImage(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using(MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }
        
        
        
        /// <summary>
        /// Convert a BitmapImage object to a byte array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static BitmapImage ToImage(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
    }
}