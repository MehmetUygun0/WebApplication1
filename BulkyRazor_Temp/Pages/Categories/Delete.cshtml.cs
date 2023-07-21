using BulkyRazor_Temp.Data;
using BulkyRazor_Temp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyRazor_Temp.Pages.Categories
{
	[BindProperties]
	public class DeleteModel : PageModel
	{
		private readonly ApplicationDbContext _db;
		public Category Category { get; set; }
		public DeleteModel(ApplicationDbContext db)
		{
			_db = db;
		}
		public void OnGet(int? id)
		{
			if (id != null && id != 0)
			{
				Category = _db.KategoriTablosu.Find(id);
			}
		}
		public IActionResult OnPost()
		{
			Category? obj = _db.KategoriTablosu.Find(Category.Id);
			if (obj == null)
			{
				return NotFound();
			}
			_db.KategoriTablosu.Remove(obj);
			_db.SaveChanges();
			TempData["success"] = "BAÞARIYLA SÝLÝNDÝ ADAMIM";
			return RedirectToPage("Index");
		}
	}
}
