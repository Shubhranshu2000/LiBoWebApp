using LiBoWebApp.Data;
using LiBoWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LiBoWebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {

            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return Create();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) {
                return NotFound();
            }
            Category categoryFromDb = _db.Categories.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj); // Return the view with validation errors
            }

            // Check if the object exists in the database
            var categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == obj.Id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            // Update the properties
            categoryFromDb.Name = obj.Name;
            categoryFromDb.DisplayOrder = obj.DisplayOrder;

            // Save changes to the database
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
