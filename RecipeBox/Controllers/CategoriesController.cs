using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
namespace RecipeBox.Controllers
{
    [Authorize]
  public class CategoriesController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public CategoriesController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }
  
    public async Task<ActionResult> Index()
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = await _userManager.FindByIdAsync(userId);
        var userCategories = _db.Categories.Where(entry => entry.User.Id == currentUser.Id).ToList();
        return View(userCategories);
    }
    public ActionResult Create()
    {
        ViewBag.RecipeId = new SelectList(_db.Recipes, "RecipeId", "Name");
        return View();
    }
   
    [HttpPost]
    public async Task<ActionResult> Create(Category category, int RecipeId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      category.User = currentUser;
      _db.Categories.Add(category);
      if (RecipeId != 0)
      {
          _db.RecipeCategories.Add(new RecipeCategory() { RecipeId = RecipeId, CategoryId = category.CategoryId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Category model = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
      return View(model); 
    }
    public ActionResult Delete(int id)
    {
        var thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
        return View(thisCategory);
    }
    
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
        _db.Categories.Remove(thisCategory);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }


  }
}