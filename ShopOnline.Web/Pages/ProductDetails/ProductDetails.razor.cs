using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages.ProductDetails
{
    public partial class ProductDetails// : ComponentBase
    {
        // Parametros
        [Parameter, EditorRequired]
        public int Id { get; set; }

        // Atributos
        public ProductDto Product { get; set; }
        public string ErrorMessage { get; set; }

        // Servicios
        [Inject]
        public IProductService ProductService { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        // Ciclos de vida
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Product = await ProductService.GetItem(Id);
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        // Metodos
        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);

                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
