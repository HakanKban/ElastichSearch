using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
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
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword")
            //.Value(customerFirstName))));

            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.CustomerFirstName.Suffix("keyword"), customerFirstName)));

            var termQuery = new TermQuery("customer_first_name.keyword")
            {
                Value = customerFirstName,
                CaseInsensitive = true
            };
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termQuery));

            foreach (var hit in result.Hits) hit.Source.Id = hit.Id;
            return result.Documents.ToList();
        }
        public async Task<List<ECommerce>> TermsQuery(List<string> customerFirsNameList)
        {
            List<FieldValue> terms = new List<FieldValue>();
            customerFirsNameList.ForEach(x =>
            {
                terms.Add(x);
            });
            var termsQuery = new TermsQuery()
            {
                Field = "customer_first_name.keyword",
                Terms = new TermsQueryField(terms.AsReadOnly())
            };

            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(termsQuery));
            return result.Documents.ToList();
        }
        public async Task<List<ECommerce>> PrefixQuery(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q.Prefix(t => t.Field(x => x.CustomerFullName.Suffix("keyword")).Value(customerFullName))));

            return result.Documents.ToList();
        }  
        public async Task<List<ECommerce>> RangeQuery(double fromPrice, double toPrice)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q
             .Range(r => r
              .NumberRange(nr => nr
               .Field(f => f.TaxfulTotalPrice)
                .Gte(fromPrice).Lte(toPrice)))));

            return result.Documents.ToList();
        }
        public async Task<List<ECommerce>> MatchAllQuery()
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(10).Query(q => q.MatchAll()));
            return result.Documents.ToList();
        } 
        
        public async Task<List<ECommerce>> PaginationQuery(int page, int pageSize)
        {
            var pageFrom  = (page - 1) * pageSize; 
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Size(10).From(pageFrom)
            .Query(q => q.MatchAll()));
            return result.Documents.ToList();
        }
    }
}
