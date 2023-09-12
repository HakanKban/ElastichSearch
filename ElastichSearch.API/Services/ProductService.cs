using ElastichSearch.API.DTOs;
using ElastichSearch.API.Models;
using ElastichSearch.API.Repository;
using Nest;
using System.Collections.Immutable;
using System.Net;

namespace ElastichSearch.API.Services;
public class ProductService
{
    private readonly ProductRepository _repository;
    private  readonly ILogger<ProductService> _logger;

    public ProductService(ProductRepository repository, ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
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
                productListDto.Add(new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature!.Height, x.Feature!.Color.ToString())));
            }
        }
        return ResponseDto<List<ProductDto>>.Success(productListDto, HttpStatusCode.OK);
    }

    public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            return ResponseDto<ProductDto>.Fail(new List<string>() { "Ürün bulunamadı" }, HttpStatusCode.NotFound);
        }

        return ResponseDto<ProductDto>.Success(product.CreateDto(), HttpStatusCode.OK);
    }

    public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto productDto)
    {
        var res = await _repository.UpdateAsync(productDto);

        if (res == false)
        {
            return ResponseDto<bool>.Fail(new List<string>() { "Güncellenemedi" }, HttpStatusCode.InternalServerError);
        }
        return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
    }

    public async Task<ResponseDto<bool>> DeleteAsync(string id)
    {
        
        var res = await _repository.DeleteAsync(id);

        if (res.IsValid == false && res.Result == Result.NotFound )
        {

            return ResponseDto<bool>.Fail(new List<string>() { "Ürün bulunamadı" }, HttpStatusCode.NotFound);
        }

        if (res.IsValid == false)
        {
            _logger.LogError(res.OriginalException, res.ServerError.Error.ToString());
            return ResponseDto<bool>.Fail(new List<string>() { "Silinemedi" }, HttpStatusCode.InternalServerError);
        }

        return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
    }
}
