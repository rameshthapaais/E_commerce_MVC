using Microsoft.AspNetCore.Mvc;
using Sycamore.DataAccess;
using Sycamore.DataAccess.Repository.IRepository;
using Sycamore.Models.Models;

namespace SycamoreCommercial.Areas.Customer.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitofWork _categoryRepo;
        public CategoryController(IUnitofWork db)
        {
            _categoryRepo = db;
        }

        public IActionResult Index()
        {
            List<Category> categories = _categoryRepo.Category.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _categoryRepo.Category.Add(obj);
                    _categoryRepo.Save();
                    TempData["success"] = "Category Added Successfully";
                    return RedirectToAction("Index");

                }
                else
                {
                    TempData["error"] = $"Error occured";
                    return View();
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
                return View(obj);
            }
        }

        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }

                var category = _categoryRepo.Category.Get (r => r.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error while creating category: {ex.Message}");

                return StatusCode(500, "An error occurred while processing your request.");

            }
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    //bool duplicateExists = _categoryRepo.GetAll().Any(c =>
                    //c.Id != obj.Id &&
                    //c.CategoryName == obj.CategoryName &&
                    //c.Order == obj.Order);

                    //if (duplicateExists)
                    //{
                    //    ModelState.AddModelError(string.Empty, "A category with the same name and order already exists.");
                    //    return View(obj); // return form with error
                    //}

                    if (_categoryRepo.Category.IsDuplicateCategory(obj.CategoryName, obj.Order, obj.Id))
                    {
                        TempData["error"] = "A category with the same name and order already exists.";
                        
                        return View(obj);
                    }
                    _categoryRepo.Category.Update(obj);
                    _categoryRepo.Save();
                    TempData["success"] = "Category Edited Successfully";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while editing category: {ex.Message}");
                TempData["error"] = $"Error occured {ex.Message} ";
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _categoryRepo.Category.Get(r => r.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(Category obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _categoryRepo.Category.Remove(obj);
                    _categoryRepo.Save();
                    TempData["success"] = "Category Deleted Successfully";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch( Exception ex)
            {
                Console.WriteLine($"Error while editing category: {ex.Message}");
                TempData["error"] = $"Error occured {ex.Message} ";
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}




