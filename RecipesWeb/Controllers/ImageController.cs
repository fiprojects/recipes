using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RecipesCore.Services;

namespace RecipesWeb.Controllers
{
    public class ImageController : Controller
    {
        private readonly string _cacheDirectory;

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IImageService _imageService;

        public ImageController(IHostingEnvironment hostingEnvironment, IImageService imageService)
        {
            _hostingEnvironment = hostingEnvironment;
            _imageService = imageService;
            _cacheDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "Cache");
        }

        public IActionResult Get(long id)
        {
            var noImagePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "no-image.png");
            var image = _imageService.GetImage(_cacheDirectory, id) ?? System.IO.File.ReadAllBytes(noImagePath);
            return File(image, "image/png");
        }

        public IActionResult Thumbnail(long id)
        {
            var noThumbnailPath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "no-thumbnail.png");
            var image = _imageService.GetThumbnail(_cacheDirectory, id) ?? System.IO.File.ReadAllBytes(noThumbnailPath);
            return File(image, "image/png");
        }
    }
}