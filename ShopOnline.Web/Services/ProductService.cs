using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net.Http.Json;

namespace ShopOnline.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var product = await this.httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("api/Product/GetItems");

                return product;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en \"api/Product/GetItems\": " + e);

                throw;
            }
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                var product = await this.httpClient.GetFromJsonAsync<ProductDto>($"api/Product/GetItem/{id}");

                return product;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en \"api/Product/GetItem\": " + e);

                throw;
            }
        }
    }
}
