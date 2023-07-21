using Bulky.DataAcccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;


namespace WebApplication1.Areas.admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
            //ViewBag.CategoryList = CategoryList;
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				}),
                Product =new Product()
            };
            if (id==null||id==0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
				return View(productVM);
			}
		}
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile? file)
        {
            deneme(productVM.Product);
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file!=null)
                {
                    string fileName=Guid.NewGuid().ToString();
                    string productPath=Path.Combine(wwwRootPath,@"images/product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //eski resmi silioruz
                        string oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using(var fileStream=new FileStream(Path.Combine(productPath,fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"/images/product/" + fileName;
                }

                if (productVM.Product.Id==0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
					_unitOfWork.Product.Update(productVM.Product);
				}
				_unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Başarıyla oluşturuldu";
                return RedirectToAction("Index", "Product");
            }
			else
			{
				productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});
				return View(productVM);
			}
        }

        
        void deneme(Product obj)
        {
            if (obj.Title == obj.Description.ToString())
            {
                ModelState.AddModelError("name", "İKİSİ AYYNI OLAMAZZ");
            }
            if (obj.Title.ToLower() == "test" || obj.Title.ToLower() == "deneme")
            {
                ModelState.AddModelError("", "DENEME veya TEST yapıldı");
            }
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
			List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data=objProductList});
        }
        [HttpDelete]
		public IActionResult Delete(int? id)
		{
            var productToBeDelete = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDelete != null)
            {
                return Json(new {success=false,message="Errror white deleting"});
            }
			string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDelete.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
			}
            _unitOfWork.Product.Remove(productToBeDelete);
            _unitOfWork.Save();
			return Json(new { success = true, message = "Delete Successful" });

		}
		#endregion
	}
}
