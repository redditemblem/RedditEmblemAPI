using SkiaSharp;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RedditEmblemAPI.Models.Output.Map
{
    #region Interface

    /// <inheritdoc cref="ImageLoader"/>
    public interface IImageLoader
    {
        /// <inheritdoc cref="ImageLoader.GetImageDimensionsByUrl(string, out int, out int)"/>
        void GetImageDimensionsByUrl(string imageUrl, out int imageHeightInPixels, out int imageWidthInPixels);
    }

    #endregion Interface

    /// <summary>
    /// Helper class that assists with loading images via HTTP requests.
    /// </summary>
    public class ImageLoader : IImageLoader
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ImageLoader() { }

        /// <summary>
        /// Executes an HTTP request to query the <paramref name="imageUrl"/> and load the image. Outputs the image's <paramref name="imageHeightInPixels"/> and <paramref name="imageWidthInPixels"/>.
        /// </summary>
        public void GetImageDimensionsByUrl(string imageUrl, out int imageHeightInPixels, out int imageWidthInPixels)
        {
            if(string.IsNullOrEmpty(imageUrl))
            {
                imageHeightInPixels = 0;
                imageWidthInPixels = 0;
                return;
            }

            using (HttpClient httpClient = new HttpClient())
            {
                Task<byte[]> imageBytes = httpClient.GetByteArrayAsync(imageUrl);
                imageBytes.Wait();

                using (MemoryStream imgStream = new MemoryStream(imageBytes.Result))
                using (SKManagedStream inputStream = new SKManagedStream(imgStream))
                using (SKBitmap img = SKBitmap.Decode(inputStream))
                {
                    imageHeightInPixels = img.Height;
                    imageWidthInPixels = img.Width;
                }
            }
        }
    }
}