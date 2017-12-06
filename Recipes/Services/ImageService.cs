using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using log4net;
using RecipesCore.Models;

namespace RecipesCore.Services
{
    public class ImageService : IImageService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ImageService));

        private readonly RecipesContext _db;

        public ImageService(RecipesContext db)
        {
            _db = db;
        }

        public byte[] GetImage(string directory, long id)
        {
            var cachedPath = Path.Combine(directory, $"{id}.png");
            if (!File.Exists(cachedPath) && BuildCache(directory, id) == null)
            {
                return null;
            }

            return File.ReadAllBytes(cachedPath);
        }

        public byte[] GetThumbnail(string directory, long id)
        {
            var thumbnailPath = Path.Combine(directory, $"{id}.thumbnail.png");
            if (!File.Exists(thumbnailPath) && BuildThumbnail(directory, id) == null)
            {
                return null;
            }

            return File.ReadAllBytes(thumbnailPath);
        }

        public void BuildCache(string directory) => _db.Recipes.ToList().ForEach(r => BuildCache(directory, r.Id));

        public string BuildCache(string directory, long id)
        {
            var path = Path.Combine(directory, $"{id}.png");
            Log.Debug($"Caching image to: {path}");

            var recipe = _db.Recipes.Find(id);
            if (recipe?.Image == null)
            {
                return null;
            }

            SaveImage(path, recipe.Image);
            return path;
        }

        public void BuildThumbnails(string directory) => _db.Recipes.ToList().ForEach(r => BuildThumbnail(directory, r.Id));

        public string BuildThumbnail(string directory, long id)
        {
            var path = Path.Combine(directory, $"{id}.thumbnail.png");
            Log.Debug($"Making thumbnail image to: {path}");

            var recipe = _db.Recipes.Find(id);
            if (recipe?.Image == null)
            {
                return null;
            }

            SaveImage(path, recipe.Image, thumbnail: true);
            return path;
        }

        public void SaveImage(string filename, byte[] imageBytes, ImageFormat format = null, bool thumbnail = false)
        {
            if (imageBytes == null)
            {
                return;
            }

            using (var image = Image.FromStream(new MemoryStream(imageBytes)))
            {
                if (thumbnail)
                {
                    Resize(image, 280, 158).Save(filename, format ?? ImageFormat.Png);
                    return;
                }

                image.Save(filename, format ?? ImageFormat.Png);
            }
        }

        public static Image Resize(Image original, int maxWidth, int maxHeight)
        {
            var width = maxWidth;
            var height = maxHeight;
            if (original.Width > original.Height)
            {
                height = (int) ((double) maxWidth / original.Width * original.Height);
            }
            else
            {
                width = (int)((double) maxHeight / original.Height * original.Width);
            }
            
            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(original, 0, 0, width, height);
            }
            return bitmap;
        }
    }
}