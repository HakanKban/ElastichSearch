using ElastichSearch.API.DTOs;
using ElastichSearch.API.Models;
using Nest;
using System.Collections.Immutable;

namespace ElastichSearch.API.Repository
{
    public class ProductRepository
    {
        private readonly ElasticClient _client;
        private const string indexName = "products";
        public ProductRepository(ElasticClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product product)
        {
            product.Created = DateTime.Now;

            var response = await _client.IndexAsync(product, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));
            //Index kaydeder. Tablo ismi products olur.
            if(!response.IsValid) return null;

            product.Id = response.Id;

            return product;
        }

        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            var result = await _client.SearchAsync<Product>(s => s.Index(indexName).Query(q => q.MatchAll()));
            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToImmutableList();
        }
        public async Task<Product?> GetByIdAsync(string id)
        {
            var result = await _client.GetAsync<Product>(id, x => x.Index(indexName));
            if (!result.IsValid)
            {
                return null;
            }
            result.Source.Id = result.Id;
            return result.Source;
        }
        public async Task<bool> UpdateAsync(ProductUpdateDto productUpdateDto)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>(productUpdateDto.Id, x =>
            x.Index(indexName).Doc(productUpdateDto));
            return response.IsValid;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var res = await _client.DeleteAsync<Product>(id, x => x.Index(indexName));
            return res.IsValid;
        }

    }
}
