using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Pages
{
    public class DisplayProductsBase : ComponentBase
    {
        [Parameter, EditorRequired]
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
