using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;
using System.Text;

namespace ShopOnline.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient httpClient;

        public event Action<int> OnShoppingCartChanged;

        public ShoppingCartService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        // AKA PostItem
        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var data = await this.httpClient.PostAsJsonAsync<CartItemToAddDto>(
                    "api/ShopiingCart/AddItem",
                    cartItemToAddDto
                );

                var response = await data.Content.ReadFromJsonAsync<CartItemDto>();

                /*
                data.StatusCode; // Estado del codigo
                await data.Content.ReadAsStringAsync(); // El mensaje del error
                */

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en \"api/ShopiingCart/AddItem\": " + e);

                throw;
            }
        }

        public async Task<IEnumerable<CartItemDto>> GetItems(int userId)
        {
            try
            {
                //var response = await this.httpClient.GetFromJsonAsync<IEnumerable<CartItemDto>>($"api/ShopiingCart/GetItems/{userId}");

                var response = await this.httpClient.GetAsync($"api/ShopiingCart/GetItems/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return Enumerable.Empty<CartItemDto>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<CartItemDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();

                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en \"api/ShopiingCart/GetItems\": " + e);

                throw;
            }
        }

        public async Task<CartItemDto> DeleteItem(int id)
        {
            try
            {
                var response = await this.httpClient.DeleteAsync($"api/ShopiingCart/DeleteItem/{id}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return default(CartItemDto);
                    }

                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();

                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en \"api/ShopiingCart/GetItems\": " + e);

                throw;
            }
        }

        public async Task<CartItemDto> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                CartItemDto result = default(CartItemDto);

                var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdateDto);
                var content = new StringContent(
                    jsonRequest,
                    Encoding.UTF8,
                    "application/json-patch+json"
                );

                var response = await this.httpClient.PatchAsync(
                    $"api/ShopiingCart/UpdateQty/{id}",
                    content
                );

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadFromJsonAsync<CartItemDto>();
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en \"api/ShopiingCart/UpdateQty\": " + e);

                throw;
            }
        }

        // Observer pattern 1: Se crear el "emiter"
        public void RaiseEventOnShoppingChanged(int totalQty)
        {
            // Si el evento tiene subscriptores
            if (OnShoppingCartChanged != null)
            {
                // Envia el valor de "totalQty" a los subscriptores
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }
    }
}
