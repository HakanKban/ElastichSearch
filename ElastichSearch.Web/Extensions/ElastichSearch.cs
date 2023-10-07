using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace ElastichSearch.Web.Extensions;
public static class ElastichSearch
{
    public static void AddElasctic(this IServiceCollection services, IConfiguration configuration)
    {
        var userName = (configuration.GetSection("Elastic")["UserName"])!;
        var password = (configuration.GetSection("Elastic")["Password"])!;
        var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!))
                      .Authentication(new BasicAuthentication(userName,password));
        var client = new ElasticsearchClient(settings);
 
        services.AddSingleton(client);
    }
}
