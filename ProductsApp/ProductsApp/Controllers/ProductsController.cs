//using ProductsApp.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;

//namespace ProductsApp.Controllers
//{
//    public class ProductsController : ApiController
//    {
//        Product[] products = new Product[]
//        {
//            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
//            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
//            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }
//        };

//        public IEnumerable<Product> GetAllProducts()
//        {
//            return products;
//        }

//        public IHttpActionResult GetProduct(int id)
//        {
//            var product = products.FirstOrDefault((p) => p.Id == id);
//            if (product == null)
//            {
//                return NotFound();
//            }
//            return Ok(product);
//        }
//    }
//}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductsApp.Models;

namespace ProductsApp.Controllers
{
    public class ProductsController : ApiController
    {
        static readonly IProductRepository repository = new ProductRepository();

        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAll();
        }

        public Product GetProduct(int id)
        {
            Product item = repository.Get(id);
            if (item==null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return repository.GetAll().Where(p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        public HttpResponseMessage PostProduct(Product item)
        {
            item = repository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);
            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public void PutProduct(int id, Product product)
        {
            product.Id = id;
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public void DeleteProduct(int id)
        {
            repository.Remove(id);
        }

    }
}
