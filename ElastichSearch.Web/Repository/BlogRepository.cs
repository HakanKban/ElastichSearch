using Elastic.Clients.Elasticsearch;
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


    }
}
