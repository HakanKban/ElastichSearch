using ElastichSearch.Web.Models;
using ElastichSearch.Web.Services;
using ElastichSearch.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ElastichSearch.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }
        public async Task<IActionResult> Search()
        {
            return View(await _blogService.SearchAsync(string.Empty));
        }

        public IActionResult Save()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateViewModel model)
        {
            var result = await _blogService.SaveAsync(model);
            if(!result)
            {
                TempData["result"] = "Kayıt Başarısız";
                return RedirectToAction(nameof(BlogController.Save));
            }
            TempData["result"] = "Kayıt Başarılı";
            return RedirectToAction(nameof(BlogController.Save));
        }   
        
        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            ViewBag.searchText = searchText;
            var blogList = await _blogService.SearchAsync(searchText);
            return View(blogList);
        }
    }
}
