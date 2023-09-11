using ElastichSearch.API.Models;

namespace ElastichSearch.API.DTOs;
public record ProductDto(string Id, string Name, decimal Price, int Stock, ProductFeatureDto? Feature)
{

}
