using ArticleProject.ServiceLayer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace ArticleProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IToastNotification toastNotification;

        public CategoryController(ICategoryService categoryService, IToastNotification toastNotification)
        {
            this.categoryService = categoryService;
            this.toastNotification = toastNotification;
        }
        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            var result = await categoryService.GetAllCategoriesForApprove();
            return View(result);
        }
        public async Task<IActionResult> ActiveCategory(Guid CategoryId)
        {
            await categoryService.ActiveCategory(CategoryId);
            toastNotification.AddSuccessToastMessage("Kategori Onaylanmıştır.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("CategoryList", "Category");
        }
        public async Task<IActionResult> PassiveCategory(Guid CategoryId)
        {
            await categoryService.PassiveCategory(CategoryId);
            toastNotification.AddInfoToastMessage("Kategori Pasife Alınmıştır.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("CategoryList", "Category");
        }
        public async Task<IActionResult> DeleteCategory(Guid CategoryId)
        {
            await categoryService.DeleteCategory(CategoryId);
            toastNotification.AddErrorToastMessage("Kategori Silinmiştir.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("CategoryList", "Category");
        }
    }
}
