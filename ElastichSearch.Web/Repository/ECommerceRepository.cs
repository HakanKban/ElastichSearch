using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElastichSearch.Web.Models;
using ElastichSearch.Web.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ElastichSearch.Web.Repository
{
    public class ECommerceRepository
    {
        private readonly ElasticsearchClient _elasticsearchClient;

        public ECommerceRepository(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }

        private const string indexName = "kibana_sample_data_ecommerce";

        public async Task<(List<ECommerce> list , long count)> SearhAsync(EcommerceSearchViewModel viewModel, int page, int pageSize)
        {
            List<Action<QueryDescriptor<ECommerce>>> listQuery = new();
            if(!string.IsNullOrEmpty(viewModel.Category)) 
            {
                //Action<QueryDescriptor<ECommerce>> matchcontext = (q) => q.Match(f => f.Field(m => m.Category).Query(viewModel.Category));
                listQuery.Add((q) => q.Match(f => f.Field(m => m.Category).Query(viewModel.Category)));
            }        
            if(!string.IsNullOrEmpty(viewModel.CustomerFullName)) 
            {
                listQuery.Add((q) => q.Match(f => f.Field(m => m.Category).Query(viewModel.CustomerFullName)));
            }       
            if(viewModel.OrderDateStart != null) 
            {
                listQuery.Add((q) => q
                      .Range(r => r
                         .DateRange(dr => dr
                          .Field(f => f.OrderDate)
                          .Gte(viewModel.OrderDateStart.Value))));
            }       
            if(viewModel.OrderDateEnd != null) 
            {
                listQuery.Add((q) => q
                      .Range(r => r
                         .DateRange(dr => dr
                          .Field(f => f.OrderDate)
                          .Lte(viewModel.OrderDateEnd.Value))));
            }
            if (string.IsNullOrEmpty(viewModel.Gender)) 
            {
                listQuery.Add((q) => q.Term(f => f.Field(m => m.Gender).Value(viewModel.Gender)));
            }
            var pageFrom = (page - 1) * pageSize;
            var result = await _elasticsearchClient.SearchAsync<ECommerce>(s => s.Index(indexName)
           .Size(pageSize).From(pageFrom).Query(q => q
              .Bool(b => b
                .Must(listQuery.ToArray()))));

            foreach (var item in result.Hits)
            {
                item.Source.Id = item.Id;
            }
            return (result.Documents.ToList(), result.Total);
        }
    }
}
