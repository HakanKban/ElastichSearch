using ElastichSearch.API.Models;
using Nest;

namespace ElastichSearch.API.Repository
{
    public class ProductRepository
    {
        private readonly ElasticClient _client;
        public ProductRepository(ElasticClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product product)
        {
            product.Created = DateTime.Now;

            var response = await _client.IndexAsync(product, x => x.Index("products"));//Index kaydeder. Tablo ismi products olur.
            if(!response.IsValid) return null;

            product.Id = response.Id;

            return product;
        }
    }
}
