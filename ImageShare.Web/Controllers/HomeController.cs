using ImageShare.Data;
using ImageShare.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace ImageShare.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ImageRepository _imageRepo = new(@"Data Source=.\sqlexpress; Initial Catalog=WebDevPractice; Integrated Security=true;");

        //private readonly IWebHostEnvironment _hostEnvironment;
        //public HomeController(IWebHostEnvironment hostEnvironment)
        //{
        //    _hostEnvironment = hostEnvironment;
        //}
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile imageFile, string password)
        {
            var fileName = imageFile.Upload("uploads");
            var id = _imageRepo.Insert(new()
            {
                Name = fileName,
                Password = password
            });
            return View(new UploadViewModel { Image = _imageRepo.GetImage(id) });
        }
        public IActionResult ViewImage(int id)
        {
            var ids = HttpContext.Session.Get<List<int>>("ids");
            bool unlocked = ids != null && ids.Contains(id);
            if (unlocked)
            {
                _imageRepo.IncrementImageViews(id);
            }
            return View(new UploadViewModel
            {
                Image = _imageRepo.GetImage(id),
                Unlocked = unlocked,
                InvalidPassword = (string)TempData["incorrect"]
            });
        }

        [HttpPost]
        public IActionResult ViewImage(string password, int id)
        {
            if (password == _imageRepo.GetImage(id).Password)
            {
                var ids = HttpContext.Session.Get<List<int>>("ids") ?? new();
                ids.Add(id);
                HttpContext.Session.Set("ids", ids);
            }
            else
            {
                TempData["incorrect"] = $"Incorrect password : {password}";
            }
            return Redirect($"/home/viewimage?id={id}");
        }

    }
}