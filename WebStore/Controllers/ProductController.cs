using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStore.Data;
using WebStore.Models;
using WebStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product;

            foreach (var obj in objList)
            {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            }

            return View(objList);
        }
        // Get - Upsert
		public IActionResult Upsert(int? id)
		{
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            //ViewBag.CategoryDropDown = CategoryDropDown;
            //ViewData["CategoryDropDown"] = CategoryDropDown;

            //Product product = new Product();

            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if(id==null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);
                if(productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);

            }
		}

		// POST - Upsert
		[HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult Upsert(ProductVM productVM)
		{
            //var rec = Request.Form.Files; // Получить список переданных файлов
            productVM.Product.Image = productVM.imageF.FileName;
            productVM.Product.CategoryId = productVM.categoryVM;

            var category_ = _db.Category.Find(productVM.Product.CategoryId);
            productVM.Product.Category = category_;

            var f = true;
            if (f/*ModelState.IsValid*/)

            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;  // F:\!Coding\C#\Store2\WebStore\wwwroot



                if (productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _db.Product.Add(productVM.Product);
                }
                else
                {
                    //Updating
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                    if(files.Count > 0) // значит файл уже добавлен
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        // Удалить старый файл
                        var oldFile = Path.Combine(upload, objFromDb.Image); // Ссылка на старое фото
                        if(System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile); // Удалить старый файл
                        }
                        
                        // Добавить новый файл
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else // Если файл для загрузки не менялся, но были обновлены другие свойства
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _db.Product.Update(productVM.Product);
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(productVM);

        }


        // Get - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _db.Product.Include(u => u.Category).FirstOrDefault(u => u.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        // POST - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);

            if (obj == null) return NotFound();

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;

            // Удалить старый файл
            var oldFile = Path.Combine(upload, obj.Image); // Ссылка на старое фото
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile); // Удалить старый файл
            }

            _db.Product.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
        }
    }
}
