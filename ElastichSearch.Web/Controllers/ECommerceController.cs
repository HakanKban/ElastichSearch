using ElastichSearch.Web.Services;
using ElastichSearch.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ElastichSearch.Web.Controllers
{
    public class ECommerceController : Controller
    {
        private readonly ECommerceService _commerceService;

        public ECommerceController(ECommerceService commerceService)
        {
            _commerceService = commerceService;
        }

        public async Task<ActionResult> Search([FromQuery] SearchPageViewModel searchPageViewModel)
        {

            var (eCommerceList, totalCount,pageLinkCount) = await _commerceService.SearcAsync(searchPageViewModel.SearchViewModel, searchPageViewModel.Page, searchPageViewModel.PageSize);

            searchPageViewModel.TotalCount = totalCount;
            searchPageViewModel.List = eCommerceList;
            searchPageViewModel.PageLinkCount = pageLinkCount;


            return View(searchPageViewModel);
        }
    }
}
