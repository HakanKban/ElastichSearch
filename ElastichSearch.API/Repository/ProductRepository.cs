using Elastic.Clients.Elasticsearch;
using ElastichSearch.API.DTOs;
using ElastichSearch.API.Models;
using System.Collections.Immutable;

namespace ElastichSearch.API.Repository
{
    public class ProductRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "products";
        public ProductRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Product?> SaveAsync(Product product)
        {
            product.Created = DateTime.Now;

            var response = await _client.IndexAsync(product, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));
            //Index kaydeder. Tablo ismi products olur.
            if(!response.IsSuccess()) return null;

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
            if (!result.IsSuccess())
            {
                return null;
            }
            result.Source.Id = result.Id;
            return result.Source;
        }
        public async Task<bool> UpdateAsync(ProductUpdateDto productUpdateDto)
        {
            var response = await _client.UpdateAsync<Product, ProductUpdateDto>(indexName,productUpdateDto.Id,x => x.Doc(productUpdateDto));
            return response.IsSuccess();
        }

        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var res = await _client.DeleteAsync<Product>(id, x => x.Index(indexName));
            return res;
        }

    }
}
