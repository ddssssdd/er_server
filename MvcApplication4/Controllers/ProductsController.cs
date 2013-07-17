using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MvcApplication4.Models;
using MvcApplication4.Expense;
namespace MvcApplication4.Controllers
{
    public class ProductsController : ApiController
    {
        Product[] products = new Product[]{
            new Product{Id =1, Name= "Tomato Soup", Category = "Groceries", Price = 1 }, 
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M }, 
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M } 
        };
        public IEnumerable<Product> GetAllProducts(){
            return products;
        }
        [HttpGet]
        public Product index(int id)
        {
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return product;
        }
        [HttpGet]
        public IEnumerable<Users> list(String username,String password)
        {
            ExpenseDB db = new ExpenseDB();
            return db.Users.SqlQuery("select * from users where username=@p0 and dbo.clrOrionStauthDecryptString(password)=@p1", new object[] {username,password });
        }
        [HttpGet]
        public IEnumerable<Product> find(String category)
        {
            return products.Where((p) => String.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }
        [HttpGet]
        public IEnumerable<Product> find2([FromUri]Product product)
        {
            //return this.ControllerContext.Request.RequestUri.Host.ToString();
            return products.Where((p) => String.Equals(p.Category, product.Category, StringComparison.OrdinalIgnoreCase));
        }

        public Product GetProductById(int id){
            var product = products.FirstOrDefault((p)=>p.Id==id);
            if (product==null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return product;
        }
        public IEnumerable<Product> GetProductsByCategory(String category){
            return products.Where( (p)=>String.Equals(p.Category,category,StringComparison.OrdinalIgnoreCase));
        }
    }


}
