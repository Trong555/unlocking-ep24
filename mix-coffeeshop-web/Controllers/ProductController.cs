using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mix_coffeeshop_web.Models;
using mix_coffeeshop_web.Repositories;

namespace mix_coffeeshop_web.Controllers
{
    [Route("[controller]/[action]")]
    public class ProductController : Controller
    {
        private IProductRepository repo;

        public ProductController(IProductRepository repo)
        {
            this.repo = repo;    
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var products = repo.GetAllProducts();
            return products;
        }

        [HttpGet("{id}")]
        public Product Get(int id)
        {
            var products = repo.GetAllProducts();
            return products.FirstOrDefault(it => it.Id == id);
        }

        [HttpPost]
        public Product CreateNewProduct([FromBody]Product product)
        {
            var isDataCorrect = product != null && !string.IsNullOrEmpty(product.Name);
            if(!isDataCorrect) return null;

            var products = repo.GetAllProducts();
            product.Id = products.Count() + 1;
            repo.CreateNewProduct(product);
            return product;
        }

        [HttpPut]
        public Product UpdateProduct([FromBody]Product product)
        {
            var selectedProduct = Get(product.Id);
            if(selectedProduct == null) return null;

            selectedProduct.Name = product.Name;
            selectedProduct.Price = product.Price;
            selectedProduct.Desc = product.Desc;
            selectedProduct.ThumbURL = product.ThumbURL;
            repo.UpdateProduct(product);
            return selectedProduct;
        }
    }
}
