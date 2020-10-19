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
    public class TagsController : Controller
    {
        private readonly RecipeBoxContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public TagsController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<ActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var userTags = _db.Tags.Where(entry => entry.User.Id == currentUser.Id).ToList();
            return View(userTags);
        }

        public ActionResult Create()
        {
            ViewBag.TagId = new SelectList(_db.Tags, "TagId", "TagName");
            return View();
        }
   
        [HttpPost]
        public async Task<ActionResult> Create(Tag tag, int RecipeId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            tag.User = currentUser;
            _db.Tags.Add(tag);
            if (RecipeId != 0)
            {
                _db.RecipeTags.Add(new RecipeTag() { RecipeId = RecipeId, TagId = tag.TagId });
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult Details(int id)
        {
            Tag model = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
            return View(model); 
        }
        
        public ActionResult Delete(int id)
        {
            var thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
            return View(thisTag);
        }
    
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
            _db.Tags.Remove(thisTag);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}