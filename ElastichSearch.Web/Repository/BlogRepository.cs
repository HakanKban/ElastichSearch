﻿using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElastichSearch.Web.Models;

namespace ElastichSearch.Web.Repository
{

    public class BlogRepository
    {
        private readonly ElasticsearchClient _elasticsearchClient;
        private const string indexName = "blog";

        public BlogRepository(ElasticsearchClient elasticsearchClient)
        {
            _elasticsearchClient = elasticsearchClient;
        }


        public async Task<Blog?> SaveAsync(Blog blog)
        {
            blog.Created = DateTime.Now;

            var response = await _elasticsearchClient.IndexAsync(blog, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));
            //Index kaydeder. Tablo ismi products olur.
            if (!response.IsValidResponse) return null;

            blog.Id = response.Id;

            return blog;
        }

        public async Task<List<Blog>> SearcAsync(string searcText) 
        {
            List<Action<QueryDescriptor<Blog>>> listQuery = new();

            Action<QueryDescriptor<Blog>> matchAll = (q) => q.MatchAll(); 
            Action<QueryDescriptor<Blog>> matchcontext = (q) => q.Match(f => f.Field(m => m.Content).Query(searcText)); 
            Action<QueryDescriptor<Blog>> titleMatchBoolPrefix = (q) => q.MatchBoolPrefix(f => f.Field(m => m.Content).Query(searcText)); 

            if(string.IsNullOrEmpty(searcText)) 
            {
               listQuery.Add(matchAll);
            }
            else
            {
                 listQuery.Add(matchcontext);
                 listQuery.Add(titleMatchBoolPrefix);
            }


            var result = await _elasticsearchClient.SearchAsync<Blog>(s => s.Index(indexName)
            .Size(1000).Query(q => q
               .Bool(b => b
                 .Should(listQuery.ToArray()))));

            foreach (var item in result.Hits)
            {
               item.Source.Id = item.Id;
            }
          return result.Documents.ToList();
        }


    }
}
