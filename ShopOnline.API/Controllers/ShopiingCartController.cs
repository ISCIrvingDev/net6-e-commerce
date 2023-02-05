using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.API.Extensions;
using ShopOnline.API.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopiingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;

        public ShopiingCartController(
            IShoppingCartRepository shoppingCartRepository,
            IProductRepository productRepository
        )
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }

        [HttpGet("GetItems/{userId:int}")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
        {
            try
            {
                // Cart items
                var cartItems = await this.shoppingCartRepository.GetItems(userId);

                if (cartItems == null)
                {
                    return NotFound();
                }

                // Products
                var products = await this.productRepository.GetItems();

                if (products == null)
                {
                    throw new Exception("No product have been registered in the system");
                }

                // Dto
                var cartItemsDto = cartItems.ConvertToDto(products);

                // Response
                return Ok(cartItemsDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("GetItem/{id:int}")]
        public async Task<ActionResult<CartItemDto>> GetItem(int id)
        {
            try
            {
                // Cart items
                var cartItem = await this.shoppingCartRepository.GetItem(id);

                if (cartItem == null)
                {
                    return NotFound($"Id product {id} was not found");
                }

                // Products
                var product = await this.productRepository.GetItem(cartItem.ProductId);

                if (product == null)
                {
                    return NotFound($"The Id product {cartItem.ProductId} was not found");
                }

                // Dto
                var cartItemDto = cartItem.ConvertToDto(product);

                // Response
                return Ok(cartItemDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // AKA PostItem
        [HttpPost("AddItem")]
        public async Task<ActionResult<CartItemDto>> AddItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                // Cart items
                var newCartItem = await this.shoppingCartRepository.AddItem(cartItemToAddDto);

                // Products
                var product = await this.productRepository.GetItem(newCartItem.ProductId);

                if (product == null)
                {
                    return NotFound($"The Id product {newCartItem.ProductId} was not found");
                }

                // Dto
                var cartItemDto = newCartItem.ConvertToDto(product);

                // Response
                return CreatedAtAction(
                    nameof(GetItem),
                    new { id = cartItemDto.Id },
                    cartItemDto
                );
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("[action]/{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
        {
            try
            {
                // Cart item
                var cartItem = await this.shoppingCartRepository.DeleteItem(id);

                if (cartItem == null)
                {
                    return NotFound($"Id cart item {id} was not found");
                }

                // Products
                var product = await this.productRepository.GetItem(cartItem.ProductId);

                if (product == null)
                {
                    return NotFound($"The Id product {cartItem.ProductId} was not found");
                }

                // Dto
                var cartItemDto = cartItem.ConvertToDto(product);

                // Response
                return Ok(cartItemDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPatch("[action]/{id:int}")]
        public async Task<ActionResult<CartItemDto>> UpdateQty(int id, [FromBody] CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                // Cart item
                var cartItem = await this.shoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);

                if (cartItem == null)
                {
                    return NotFound($"Id cart item {id} was not found");
                }

                // Products
                var product = await this.productRepository.GetItem(cartItem.ProductId);

                if (product == null)
                {
                    return NotFound($"The Id product {cartItem.ProductId} was not found");
                }

                // Dto
                var cartItemDto = cartItem.ConvertToDto(product);

                // Response
                return Ok(cartItemDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
