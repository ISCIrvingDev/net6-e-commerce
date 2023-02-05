using ShopOnline.API.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductDto> ConvertToDto(
            this IEnumerable<Product> products,
            IEnumerable<ProductCategory> productCategories
        ) {
            var result = (
                from product in products
                join productCategory in productCategories
                on product.CategoryId equals productCategory.Id
                select new ProductDto
                {
                    Id= product.Id,
                    Name= product.Name,
                    Description= product.Description,
                    ImageUrl= product.ImageURL,
                    Price= product.Price,
                    Qty= product.Qty,
                    CategoryId= product.CategoryId,
                    CategoryName= productCategory.Name
                }
            ).ToList();

            return result;
        }

        public static ProductDto ConvertToDto(
            this Product product,
            ProductCategory productCategory
        )
        {
            var result = new ProductDto
            {
                Id= product.Id,
                Name= product.Name,
                Description= product.Description,
                ImageUrl= product.ImageURL,
                Price= product.Price,
                Qty= product.Qty,
                CategoryId= product.CategoryId,
                CategoryName= productCategory.Name
            };

            return result;
        }

        public static IEnumerable<CartItemDto> ConvertToDto(
            this IEnumerable<CartItem> cartItems,
            IEnumerable<Product> products
        )
        {
            var result = (
                from cartItem in cartItems
                join product in products
                on cartItem.ProductId equals product.Id
                select new CartItemDto
                {
                    Id= cartItem.Id,
                    ProductId= cartItem.ProductId,
                    ProductName= product.Name,
                    ProductDescription= product.Description,
                    ProductImageUrl= product.ImageURL,
                    Price= product.Price,
                    CartId= cartItem.CartId,
                    Qty= cartItem.Qty,
                    TotalPrice= (product.Price * cartItem.Qty)
                }
            ).ToList();

            return result;
        }

        public static CartItemDto ConvertToDto(
            this CartItem cartItem,
            Product product
        )
        {
            var result = new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageUrl = product.ImageURL,
                Price = product.Price,
                CartId = cartItem.CartId,
                Qty = cartItem.Qty,
                TotalPrice = (product.Price * cartItem.Qty)
            };

            return result;
        }
    }
}
