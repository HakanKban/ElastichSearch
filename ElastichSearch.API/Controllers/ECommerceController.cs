using ElastichSearch.API.Models;
using ElastichSearch.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElastichSearch.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ECommerceController : BaseController
    {
        private readonly EcommerceRepository _repo; //Servis işlemleri ile vakita kaybedilmemek için repo kullanılmıştır.

        public ECommerceController(EcommerceRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> TermQuery(string firstName) 
        {
            
          return Ok (await _repo.TermQuery(firstName));
        
        }

    }
}
