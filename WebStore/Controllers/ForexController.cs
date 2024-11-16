using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace WebStore.Controllers
{
    public class ForexController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if(uploadedFile != null)
            {
                // Set Key Name
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/product", ImageName);
                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(stream);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
