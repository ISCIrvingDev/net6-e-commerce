using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages.ShoppingCart
{
    public partial class ShoppingCart
    {
        // Atributos
        public IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }
        protected string TotalPrice { get; set; }
        protected int TotalQty { get; set; }

        // Servicios
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IJSRuntime Js { get; set; }

        // Ciclos de vida
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Js.InvokeVoidAsync("unaFuncioDeEjemplo", 6, 666);

                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);

                CartChanged();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        // Metodos - Eventos
        protected async Task DeleteItem_Click(int id)
        {
            try
            {
                var cartItemDto = await ShoppingCartService.DeleteItem(id);

                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);

                CartChanged();
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = id,
                        Qty = qty
                    };

                    var returnedUpdateItemDto = await this.ShoppingCartService.UpdateQty(id, updateItemDto);

                    ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);

                    CartChanged();
                }
                else
                {
                    var item = this.ShoppingCartItems.FirstOrDefault(i => i.Id == id);

                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice= item.Price;
                    }
                }
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
        }

        // Metodos - Computados
        private void CartChanged()
        {
            // Set totals - CalculateCartSummaryTotals()
            this.TotalPrice = this.ShoppingCartItems.Sum(i => i.TotalPrice).ToString("C");
            this.TotalQty = this.ShoppingCartItems.Sum(i => i.Qty);

            // Observer pattern 2: Se manda a llamar el "emiter"
            ShoppingCartService.RaiseEventOnShoppingChanged(this.TotalQty);
        }
    }
}
