using Elasticsearch.Net;
using Nest;

namespace ElastichSearch.API.Extensions;
public static class ElastichSearch
{
    public static void AddElasctic(this IServiceCollection services, IConfiguration configuration)
    {
        var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));
        var settings = new ConnectionSettings(pool);
        var client = new ElasticClient(settings);
        services.AddSingleton(client);
    }
}
