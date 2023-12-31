﻿using Elastic.Clients.Elasticsearch;
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
        public async Task<List<ECommerce>> WildCardQuery(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q.Wildcard(w => w.Field(f => f.CustomerFullName.Suffix("keyword")).Wildcard(customerFullName))));
            return result.Documents.ToList();
        }  
        public async Task<List<ECommerce>>  FuzzyQuery(string customerName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q.
              Fuzzy(w => w.
                Field(f => f.CustomerFirstName.
                 Suffix("keyword")).Value(customerName).
                   Fuzziness(new Fuzziness(1)))).Sort(sort=> sort.Field(f => f.TaxfulTotalPrice, new FieldSort() { Order = SortOrder.Desc})));
            return result.Documents.ToList();
        } 

        // Or layarak sorgular. Score değeri var. Hakan ya da Kaban.
        public async Task<List<ECommerce>>  MatchQueryFullText(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q.
              Match(w => w.
                Field(f => f.Category).Query(customerFullName))));
                
            return result.Documents.ToList();
        }


        public async Task<List<ECommerce>> CompoundQuery(string cityName, double taxFullTotalPrice, string categoryName , string menuFActure)
        {
            var result = await _client.SearchAsync<ECommerce>(
                s => s.Index(indexName)
                .Size(1000)
                .Query(q => q.
                Bool(b => b.
                Must(m => m.
                Term(t => t.
                Field("geoip.city_name").Value(cityName)))
                .MustNot(mn => mn.Range(r => r.NumberRange(nr => nr.Field(f => f.TaxfulTotalPrice).Lte(taxFullTotalPrice))))
                .Should(s => s.Term(t => t.Field(f => f.Category.Suffix("keywprd")).Value(categoryName)))
                .Filter(f => f.Term(t => t.Field("menufacturer.ketword").Value(menuFActure))))));
            return result.Documents.ToList();

        }

    }
}
