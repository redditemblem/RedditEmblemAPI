using RedditEmblemAPI.Models.Configuration;
using RedditEmblemAPI.Models.Exceptions.Query;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace RedditEmblemAPI.Services.Helpers
{
    public static class MapImageBuilder
    {
        public static byte[] Generate(JSONConfiguration config)
        {
            //Grab the first matrix row from the MapControls query. Data here should only ever be in one row/column.
            IList<object> values = config.Map.MapControls.Query.Data.First();

            //Validate the map is turned on
            if ((values.ElementAtOrDefault(config.Map.MapControls.MapSwitch) ?? "Off").ToString() != "On")
                throw new MapDataLockedException();

            //Validate we have a map image
            string mapImageURL = (values.ElementAtOrDefault(config.Map.MapControls.MapImageURL) ?? string.Empty).ToString();
            if (string.IsNullOrEmpty(mapImageURL))
                throw new MapImageURLNotFoundException(config.Map.MapControls.Query.Sheet);

            //Load map image data from URL
            byte[] mapImageData = new WebClient().DownloadData(mapImageURL);
            
            //Create image canvas using map image as background
            using (MemoryStream imgStream = new MemoryStream(mapImageData))
            using (SKManagedStream inputStream = new SKManagedStream(imgStream))
            using (SKBitmap img = SKBitmap.Decode(inputStream))
            using (SKSurface surface = SKSurface.Create(new SKImageInfo(img.Width, img.Height)))
            {
                RenderBytesToCanvas(mapImageData, surface.Canvas, 0, 0);

                byte[] sprite = new WebClient().DownloadData("https://cdn.discordapp.com/attachments/783904931064709141/873386449213485116/SequoiaPromo.gif");
                RenderBytesToCanvas(sprite, surface.Canvas, 30, 30);

                return surface.Snapshot().Encode().ToArray();
            }
        }

        private static void RenderBytesToCanvas(byte[] imageData, SKCanvas canvas, float x, float y)
        {
            using (MemoryStream imgStream = new MemoryStream(imageData))
            using (SKManagedStream inputStream = new SKManagedStream(imgStream))
            using (SKBitmap img = SKBitmap.Decode(inputStream))
            {
                canvas.DrawBitmap(img, x, y);
            }
        }
    }
}
