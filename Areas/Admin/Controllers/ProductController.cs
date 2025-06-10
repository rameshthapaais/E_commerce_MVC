using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sycamore.DataAccess;
using Sycamore.DataAccess.Repository.IRepository;
using Sycamore.Models.Models;
using Sycamore.Models.ViewModels;
using System.Drawing;

namespace SycamoreCommercial.Areas.Customer.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitofWork _productRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IWebHostEnvironment webHostEnvironment, IUnitofWork db)
        {
            _productRepo = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = _productRepo.Product.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {

            //Product product = new ()
            ProductVM productVM = new ProductVM()

            {
                CategoryList = _productRepo.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productVM);

            }
            else
            {

                productVM.Product = _productRepo.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }


        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile file)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null && file.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath, @"images\product");

                        if (!string.IsNullOrEmpty(productVM.Product.ProductImages))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ProductImages.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);

                        }

                        productVM.Product.ProductImages = @"\images\product\" + fileName;
                    }
                    if (productVM.Product.Id == 0)
                    {
                        _productRepo.Product.Add(productVM.Product);
                        TempData["success"] = "Product created successfully!.";

                    }
                    else                    //string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null && file.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath, @"images\product");

                        if (!string.IsNullOrEmpty(productVM.Product.ProductImages))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ProductImages.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);

                        }

                        productVM.Product.ProductImages = @"\images\product\" + fileName;
                    }
                    if (productVM.Product.Id == 0)
                    {
                        _productRepo.Product.Add(productVM.Product);
                        TempData["success"] = "Product created successfully!.";

                    }

                    else
                    {
                        _productRepo.Product.Update(productVM.Product);
                        TempData["sucess"] = "Product Edited succesfully";
                    }
                    _productRepo.Save();
                    return RedirectToAction("Index");

                }
                else
                {

                    productVM.CategoryList = _productRepo.Category.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.CategoryName,
                        Value = u.Id.ToString()
                    });

                    return View(productVM);
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can also use a logging framework)
                Console.WriteLine($"Error while creating category: {ex.Message}");

                // Optionally, show a friendly error message to the user
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");

                TempData["error"] = $"Error occured {ex.Message} ";

                // Return the view with the current data so user input is not lost
                return View(productVM);
            }
        }

        
        /*
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = _productRepo.Product.Get(r => r.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            var productVM = new ProductVM
            {
                Product = product,
                CategoryList = _productRepo.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.Id.ToString()
                })
            };

            return View(productVM);

        }

        [HttpPost]
        public IActionResult Delete(ProductVM productVM)
        {
            try
            {

                var productFromDb = _productRepo.Product.Get(u => u.Id == productVM.Product.Id);
                if (productFromDb == null)
                {
                    return NotFound();
                }

                // Step 1: Delete the image from disk if it exists
                if (!string.IsNullOrEmpty(productFromDb.ProductImages))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, productFromDb.ProductImages.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                _productRepo.Product.Remove(productFromDb);
                _productRepo.Save();
                TempData["success"] = "Product Deleted Successfully";
                return RedirectToAction("Index");

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error while editing category: {ex.Message}");
                TempData["error"] = $"Error occured {ex.Message} ";
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }
        */


        //API calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _productRepo.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = (objProductList) });
        }

        [HttpDelete]
        public IActionResult Delete(int?id)
        {
            try
            {

                var productFromDb = _productRepo.Product.Get(u => u.Id == id);
                if (productFromDb == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }

                // Step 1: Delete the image from disk if it exists
               
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, productFromDb.ProductImages.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                
                _productRepo.Product.Remove(productFromDb);
                _productRepo.Save();
                return Json(new { success = true, message = "Delete Sucessful" });

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error while editing category: {ex.Message}");
                TempData["error"] = $"Error occured {ex.Message} ";
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }



    }
}




