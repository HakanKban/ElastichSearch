using Elastic.Clients.Elasticsearch;
using ElastichSearch.API.Extensions;
using ElastichSearch.API.Models;

namespace ElastichSearch.API.Repository
{
    public class EcommerceRepository
    {
        private readonly ElasticsearchClient _client;

        public EcommerceRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        private const string indexName = "kibana_sample_data_ecommerce";

        public async Task<List<ECommerce>> TermQuery(string customerFirstName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword")
            .Value(customerFirstName))));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToList();
        }
 
    }
}
