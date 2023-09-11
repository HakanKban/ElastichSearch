using ElastichSearch.API.Models;

namespace ElastichSearch.API.DTOs
{
    public record ProductFeatureDto(int Width,int Height, EColor Color)
    {
    }
}
