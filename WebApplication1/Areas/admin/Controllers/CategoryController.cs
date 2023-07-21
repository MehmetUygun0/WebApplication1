using Bulky.DataAcccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;


namespace WebApplication1.Areas.admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DiplayOrder.ToString())
            {
                ModelState.AddModelError("name", "İKİSİ AYYNI OLAMAZZ");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Başarıyla oluşturuldu";
                return RedirectToAction("Index", "Category");
            }
            if (obj.Name.ToLower() == "test" || obj.Name.ToLower() == "deneme")
            {
                ModelState.AddModelError("", "DENEME veya TEST yapıldı");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromunitOfWork = _unitOfWork.Category.Get(u => u.Id == id);
            //Category? categoryFromunitOfWork1 = _unitOfWork.KategoriTablosu.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFromunitOfWork2 = _unitOfWork.KategoriTablosu.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFromunitOfWork == null) { return NotFound(); }
            return View(categoryFromunitOfWork);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DiplayOrder.ToString())
            {
                ModelState.AddModelError("name", "İKİSİ AYYNI OLAMAZZ");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Başarıyla güncellendi";
                return RedirectToAction("Index", "Category");
            }
            if (obj.Name.ToLower() == "test" || obj.Name.ToLower() == "deneme")
            {
                ModelState.AddModelError("", "DENEME veya TEST yapıldı");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromunitOfWork = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromunitOfWork == null) { return NotFound(); }
            return View(categoryFromunitOfWork);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Başarıyla silidi";
            return RedirectToAction("Index", "Category");
        }
    }
}
