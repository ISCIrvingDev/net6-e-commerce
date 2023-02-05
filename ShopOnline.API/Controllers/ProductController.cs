using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.API.Extensions;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet("GetItems")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems() {
            try
            {
                var products = await this.productRepository.GetItems();
                var productCategories = await this.productRepository.GetCategories();

                if (products == null || productCategories == null)
                {
                    return NotFound();
                }
                
                var productsDto = products.ConvertToDto(productCategories);

                return Ok(productsDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("GetItem/{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id) {
            try
            {
                var product = await this.productRepository.GetItem(id);

                if (product == null)
                {
                    return NotFound();
                }
                
                var productCategory = await this.productRepository.GetCategory(id);

                var productDto = product.ConvertToDto(productCategory);

                return Ok(productDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
