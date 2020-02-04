#region snippet_all

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientSample
{
    #region snippet_prod

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }

    #endregion

    class Program
    {
        #region snippet_HttpClient

        private static readonly HttpClient Client = new HttpClient();

        #endregion

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"Name: {product.Name}\tPrice: " +
                              $"{product.Price}\tCategory: {product.Category}");
        }

        #region snippet_CreateProductAsync

        static async Task<Uri> CreateProductAsync(Product product)
        {
            HttpResponseMessage response = await Client.PostAsJsonAsync(
                "api/products", product);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        #endregion

        #region snippet_GetProductAsync

        static async Task<Product> GetProductAsync(string path)
        {
            Product product = null;
            HttpResponseMessage response = await Client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsAsync<Product>();
            }

            return product;
        }

        #endregion

        #region snippet_UpdateProductAsync

        static async Task UpdateProductAsync(Product product)
        {
            HttpResponseMessage response = await Client.PutAsJsonAsync(
                $"api/products/{product.Id}", product);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            await response.Content.ReadAsAsync<Product>();
        }

        #endregion

        #region snippet_DeleteProductAsync

        static async Task<HttpStatusCode> DeleteProductAsync(string id)
        {
            HttpResponseMessage response = await Client.DeleteAsync(
                $"api/products/{id}");
            return response.StatusCode;
        }

        #endregion

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        #region snippet_run

        #region snippet5

        static async Task RunAsync()
        {
            // Update port # in the following line.
            Client.BaseAddress = new Uri("http://localhost:65316/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            #endregion

            try
            {
                // Create a new product
                Product product = new Product
                {
                    Name = "Gizmo",
                    Price = 100,
                    Category = "Widgets"
                };

                var url = await CreateProductAsync(product);
                Console.WriteLine($"Created at {url}");

                // Get the product
                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);

                // Update the product
                Console.WriteLine("Updating price...");
                product.Price = 80;
                await UpdateProductAsync(product);

                // Get the updated product
                product = await GetProductAsync(url.PathAndQuery);
                ShowProduct(product);

                // Delete the product
                var statusCode = await DeleteProductAsync(product.Id);
                Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        #endregion
    }
}

#endregion