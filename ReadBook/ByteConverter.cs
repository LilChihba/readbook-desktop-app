using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace ReadBook
{
    public class ByteConverter
    {
        public static BitmapImage convertByteToBitmapImage(Byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            MemoryStream mem = new MemoryStream(bytes);
            mem.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = mem;
            image.EndInit();
            return image;
        }
    }
}
