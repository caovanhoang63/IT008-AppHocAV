using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using PexelsDotNetSDK.Api;

namespace IT008_AppHocAV.Services
{
    public class PexelsImageAPI
    {
        public static async Task<BitmapImage> GetImages(string keyword)
        {
            var pexelsClient = new PexelsClient("9A5Ec3I5Td0wKHPHUQH7P1a9tS2bp8y3blhs1DY6YEzoYVr9CXwMQMD8");
            var result = await pexelsClient.SearchPhotosAsync(keyword);
            var photoSource = result.photos[0].source.large;
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(photoSource); 
            image.EndInit();
            return image;
        }
    }
}