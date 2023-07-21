using BulkyRazor_Temp.Data;
using BulkyRazor_Temp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyRazor_Temp.Pages.Categories
{
	[BindProperties]
    public class CreateModel : PageModel
    {
		private readonly ApplicationDbContext _db;
		public Category Category{ get; set; }
		public CreateModel(ApplicationDbContext db)
		{
			_db = db;
		}
		public void OnGet()
        {
        }
		public IActionResult OnPost(Category obj)
		{
			_db.KategoriTablosu.Add(Category);
			_db.SaveChanges();
			TempData["success"] = "BAÞARIYLA EKLENDÝ ADAMIM";
			return RedirectToPage("Index");
		}
    }
}
