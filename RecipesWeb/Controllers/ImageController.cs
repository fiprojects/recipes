﻿using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RecipesCore.Services;

namespace RecipesWeb.Controllers
{
    public class ImageController : BaseController
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

        public IActionResult Get(long id) => Get(id, Path.Combine(_hostingEnvironment.WebRootPath, "Images", "no-image.png"));
        
        public IActionResult Thumbnail(long id) => Get(id, Path.Combine(_hostingEnvironment.WebRootPath, "Images", "no-thumbnail.png"));

        private IActionResult Get(long id, string fallbackPath) {
            var image = _imageService.GetImage(_cacheDirectory, id) ?? System.IO.File.ReadAllBytes(fallbackPath);
            return File(image, "image/png");
        }
    }
}