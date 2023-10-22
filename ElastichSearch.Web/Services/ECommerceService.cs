using ElastichSearch.Web.Repository;
using ElastichSearch.Web.ViewModel;

namespace ElastichSearch.Web.Services
{
    public class ECommerceService
    {
        private readonly ECommerceRepository _repository;

        public ECommerceService(ECommerceRepository repository)
        {
            _repository = repository;
        }

        public async  Task<(List<ECommerceViewModel>, long totalCount, long pageLinkCount)> SearcAsync(EcommerceSearchViewModel viewModel, int page, int pageSize)
        {
            var (list, totalCount) = await _repository.SearhAsync(viewModel, page, pageSize);
            var pageLinkCount = totalCount % pageSize;
            if (pageLinkCount == 0)
            {
                pageLinkCount = totalCount / pageSize;
            }
            else 
            { pageLinkCount = totalCount / pageSize +1;}

            var eCommerceList = list.Select(x => new ECommerceViewModel()
            {
                Category = String.Join(",", x.Category),
                CustomerFirstName = x.CustomerFirstName,
                CustomerLastName = x.CustomerLastName,
                CustomerFullName = x.CustomerFullName,
                OrderDate = x.OrderDate.ToShortDateString(),
                Gender = x.Gender,
                Id = x.Id,
                OrderId = x.OrderId,
                TaxfulTotalPrice = x.TaxfulTotalPrice
            }).ToList();
            return (eCommerceList, totalCount, pageLinkCount);
        }
    }
}
