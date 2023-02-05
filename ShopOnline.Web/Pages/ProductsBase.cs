using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Linq;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase : ComponentBase
    {
        // Atributos
        public IEnumerable<ProductDto> Products { get; set; }

        // Servicios
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        // Ciclos de vida
        protected override async Task OnInitializedAsync()
        {
            //return base.OnInitializedAsync(); // Se quita esta linea para que funcione el metodo de la manera que se espera

            Products = await ProductService.GetItems();

            var shoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
            var totalQty = shoppingCartItems.Sum(i => i.Qty);

            // Observer pattern 2: Se manda a llamar el "emiter"
            ShoppingCartService.RaiseEventOnShoppingChanged(totalQty);
        }

        // Metodos del componente
        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            var result = from product in Products
                         group product by product.CategoryId
                                                into prodByCatGroup
                         orderby prodByCatGroup.Key
                         select prodByCatGroup;

            return result;
        }

        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDto)
        {
            var result = groupedProductDto.FirstOrDefault(pg => pg.CategoryId == groupedProductDto.Key);

            return result.CategoryName;
        }
    }
}
