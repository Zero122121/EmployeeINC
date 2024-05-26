using System.IO;
using System.Windows.Media.Imaging;

namespace DB.Utils
{
    public static class ImageConverter
    {
        public static byte[] ConvertBitmapImageToByteArray(this BitmapImage bitmapImage)
        {
            byte[] result = null;

            if (bitmapImage != null)
            {
                using MemoryStream memoryStream = new MemoryStream();
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);

                result = memoryStream.ToArray();
            }

            return result;
        }

        public static BitmapImage ConvertByteArrayToBitmapImage(this byte[] byteArray)
        {
            BitmapImage bitmapImage = new BitmapImage();

            if (byteArray != null)
            {
                using MemoryStream memoryStream = new MemoryStream(byteArray);
                memoryStream.Position = 0;

                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }
    }
}