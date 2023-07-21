using BulkyRazor_Temp.Data;
using BulkyRazor_Temp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyRazor_Temp.Pages.Categories
{
	[BindProperties]
    public class EditModel : PageModel
    {
		private readonly ApplicationDbContext _db;
		public Category Category { get; set; }
		public EditModel(ApplicationDbContext db)
		{
			_db = db;
		}
		public void OnGet(int? id)
		{
			if (id!=null && id!=0)
			{
				Category = _db.KategoriTablosu.Find(id);
			}
		}
		public IActionResult OnPost()
		{
			if (ModelState.IsValid)
			{
				_db.KategoriTablosu.Update(Category);
				_db.SaveChanges();
				TempData["success"] = "BAÞARIYLA GÜNCELLENDÝ ADAMIM";
				return RedirectToPage("Index");
			}
			return Page();
		}
	}
}
