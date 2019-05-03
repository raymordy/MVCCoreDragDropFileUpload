using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCFileUploader.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MVCFileUploader.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _environment;
        public HomeController(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    string folderRoot = Path.Combine(_environment.ContentRootPath, "Uploads");
                    string filePath = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    filePath = Path.Combine(folderRoot, filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                return Ok(new { success = true, message = "File Uploaded" });
            }
            catch (Exception)
            {
                return BadRequest(new { success = false, message = "Error file failed to upload" });
            }
        }
    }
}

