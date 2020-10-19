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
  public class RecipesController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public RecipesController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = await _userManager.FindByIdAsync(userId);
        var userRecipes = _db.Recipes.Where(entry => entry.User.Id == currentUser.Id).ToList();
        return View(userRecipes);
    }
    public ActionResult Create()
    {
        ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
        return View();
    }
   
    [HttpPost]
    public async Task<ActionResult> Create(Recipe recipe, int CategoryId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      recipe.User = currentUser;
      _db.Recipes.Add(recipe);
      if (CategoryId != 0)
      {
          _db.RecipeCategories.Add(new RecipeCategory() { CategoryId = CategoryId, RecipeId = recipe.RecipeId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Details(int id)
    {
      Recipe model = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      return View(model); 
    }
    public ActionResult AddTag(int id)
    {
      Recipe thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
      ViewBag.TagId = new SelectList(_db.Tags, "TagId", "TagName");
      return View(thisRecipe);
    }
    [HttpPost]
    public ActionResult AddTag(RecipeTag recipeTag)
    {
      if (recipeTag.TagId != 0)
      {
        if (_db.RecipeTags.Where(recipe => recipe.RecipeId == recipeTag.RecipeId && recipe.TagId == recipeTag.TagId).ToHashSet().Count == 0)
        {
          _db.RecipeTags.Add(recipeTag);
        }
        
      
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    
    public ActionResult Delete(int id)
    {
        var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
        return View(thisRecipe);
    }
    
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
        _db.Recipes.Remove(thisRecipe);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
  }
}
