using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages.Checkout
{
    public partial class Checkout
    {
        // Atributos
        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
        protected int TotalQty { get; set; }
        protected string PaymentDescription { get; set; }
        protected decimal PaymentAmount { get; set; }

        // Servicios
        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        // Ciclos de vida
        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);

                if (ShoppingCartItems != null)
                {
                    Guid orderGuid = Guid.NewGuid();

                    PaymentAmount = ShoppingCartItems.Sum(e => e.TotalPrice);
                    TotalQty = ShoppingCartItems.Sum(e => e.Qty);
                    PaymentDescription = $"O_{HardCoded.UserId}_{orderGuid}";
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // Se manda a ejecutar despues de que el componente se ha renderizado
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await Js.InvokeVoidAsync("initPayPalButton");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // Metodos - Eventos
        protected async Task Pay_Click()
        {
            try
            {
                // Implemetar logica para pagar
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
