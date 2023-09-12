using ElastichSearch.API.DTOs;
using ElastichSearch.API.Repository;
using System.Collections.Immutable;
using System.Net;

namespace ElastichSearch.API.Services;
public class ProductService
{
    private ProductRepository _repository;

    public ProductService(ProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
    {
        var response = await _repository.SaveAsync(request.CreateProduct());

        if (response == null)
        {
            return ResponseDto<ProductDto>.Fail(new List<string> { "Hata meydana geldi" }, HttpStatusCode.InternalServerError);
        }

        return ResponseDto<ProductDto>.Success(response.CreateDto(), HttpStatusCode.Created);
    }

    public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        var productListDto = new List<ProductDto>();

        //var productsListDTO = products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature!.Height, x.Feature!.Color))).ToList();

        foreach (var x in products)
        {
            if (x.Feature is null)
            {
                 productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, null));
            }
            else
            {
                productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature!.Height, x.Feature!.Color)));
            }
        }
        return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);
    }
}
