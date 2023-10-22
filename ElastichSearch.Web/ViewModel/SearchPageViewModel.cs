namespace ElastichSearch.Web.ViewModel
{
    public class SearchPageViewModel
    {
        public long TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public long PageLinkCount { get; set; }
        public List<ECommerceViewModel> List { get; set; }
        public EcommerceSearchViewModel  SearchViewModel { get; set; }
        public string CreatePageUrl(HttpRequest request, int page, int pageSize) 
        {
            var fullUrl = new Uri($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}").AbsolutePath;

            if(fullUrl.Contains("page", StringComparison.OrdinalIgnoreCase)) 
            {
                fullUrl = fullUrl.Replace($"Page = {Page}", $"Page={page}", StringComparison.OrdinalIgnoreCase);

                fullUrl = fullUrl.Replace($"PageSize = {PageSize}", $"Page={pageSize}", StringComparison.OrdinalIgnoreCase);
            }
            else 
            {
                fullUrl = $"{fullUrl}?Page={page}";
                fullUrl = $"{fullUrl}?PageSize={pageSize}";
            }
            return fullUrl;
        }
    }
}
