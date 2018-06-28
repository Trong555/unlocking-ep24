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
    public class UnitTest1
    {
        [Fact(DisplayName = "Get products with correct data Then system return all products.")]
        public void GetAllProductSuccess()
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repo = mock.Create<mix_coffeeshop_web.Repositories.IProductRepository>();
            var api = new mix_coffeeshop_web.Controllers.ProductController(repo.Object);
            repo.Setup(it => it.GetAllProducts()).Returns(()=> new List<Product>
            {
                new Product(),
                new Product(),
                new Product(),
                new Product(),
            });
            var products = api.Get();
            products.Should().HaveCount(4);
        }

        [Fact(DisplayName = "Get products when no products in the system Then system return 0 product.")]
        public void GetAllProductWhenNoDataInTheStorage()
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repo = mock.Create<mix_coffeeshop_web.Repositories.IProductRepository>();
            var api = new mix_coffeeshop_web.Controllers.ProductController(repo.Object);
            repo.Setup(it => it.GetAllProducts()).Returns(()=> new List<Product>
            {
            });
            var products = api.Get();
            products.Should().HaveCount(0);
        }

        [Theory(DisplayName = "Get a product with correct data Then system return the selected product")]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        public void GetAnExistingProductFromId(int id, int expectedProductId)
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repo = mock.Create<mix_coffeeshop_web.Repositories.IProductRepository>();
            var api = new mix_coffeeshop_web.Controllers.ProductController(repo.Object);
            var allProducts = new List<Product>
            {
                new Product{ Id = 1, Name = "name01", Price = 50, Desc = "desc01", ThumbURL = "img01.png" },
                new Product{ Id = 2, Name = "name02", Price = 60, Desc = "desc02", ThumbURL = "img02.png" },
                new Product{ Id = 3, Name = "name03", Price = 70, Desc = "desc03", ThumbURL = "img03.png" },
            };
            repo.Setup(it => it.GetAllProducts()).Returns(() => allProducts);
            var product = api.Get(id);

            var expect = allProducts.FirstOrDefault(it => it.Id == expectedProductId);
            product.Should().NotBeNull().And.BeSameAs(expect);
        }

        [Theory(DisplayName = "Get a product with incorrect data Then system return no product")]
        [InlineData(99)]
        public void GetNotExistingProductFromId(int id)
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repo = mock.Create<mix_coffeeshop_web.Repositories.IProductRepository>();
            var api = new mix_coffeeshop_web.Controllers.ProductController(repo.Object);
            var allProducts = new List<Product>
            {
                new Product{ Id = 1, Name = "name01", Price = 50, Desc = "desc01", ThumbURL = "img01.png" },
            };
            repo.Setup(it => it.GetAllProducts()).Returns(() => allProducts);
            var product = api.Get(id);

            product.Should().BeNull();
        }
    }
}