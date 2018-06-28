using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;
using mix_coffeeshop_web.Models;
using Moq;
using Xunit;
using System.Linq;

namespace mix_coffeeshop_web_test
{
    public class UpdateProduct
    {
        [Theory(DisplayName = "Update a product with correct data Then system return the updated prodcut")]
        [InlineData(1, "new-name1", 99, "new-des1", "new-thubm1.png")]
        public void UpdateAnExistingProduct(int id, string name, double price, string desc, string thumbURL)
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repo = mock.Create<mix_coffeeshop_web.Repositories.IProductRepository>();
            var api = new mix_coffeeshop_web.Controllers.ProductController(repo.Object);
            var allProducts = new List<Product>
            {
                new Product{ Id = 1, Name = "name01", Price = 50, Desc = "desc01", ThumbURL = "img01.png" },
            };
            repo.Setup(it => it.GetAllProducts()).Returns(() => allProducts);

            var data = new Product{ Id = id, Name = name, Price = price, Desc = desc, ThumbURL = thumbURL };
            var product = api.UpdateProduct(data);

            product.Should().NotBeNull().And.BeEquivalentTo(data);
        }

        [Theory(DisplayName = "Update a product with incorrect data Then system return no product")]
        [InlineData(99, "new-name1", 99, "new-des1", "new-thubm1.png")]
        public void UpdateNotExistingProduct(int id, string name, double price, string desc, string thumbURL)
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repo = mock.Create<mix_coffeeshop_web.Repositories.IProductRepository>();
            var api = new mix_coffeeshop_web.Controllers.ProductController(repo.Object);
            var allProducts = new List<Product>
            {
                new Product{ Id = 1, Name = "name01", Price = 50, Desc = "desc01", ThumbURL = "img01.png" },
            };
            repo.Setup(it => it.GetAllProducts()).Returns(() => allProducts);

            var data = new Product{ Id = id, Name = name, Price = price, Desc = desc, ThumbURL = thumbURL };
            var product = api.UpdateProduct(data);

            product.Should().BeNull();
        }
    }
}