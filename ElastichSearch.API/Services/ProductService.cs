using ElastichSearch.API.DTOs;
using ElastichSearch.API.Repository;
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
 

}
