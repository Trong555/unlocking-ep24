using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;
using mix_coffeeshop_web.Models;
using Moq;
using Xunit;
using System.Linq;
using mix_coffeeshop_web.Repositories;
using mix_coffeeshop_web.Controllers;
using System.Linq.Expressions;

namespace mix_coffeeshop_web_test
{
    public class GetOrderByReferenceCode
    {
        public static IEnumerable<object[]> GetOrderByReferenceCodeTestData = new List<object[]>{
            new object[] { "1", new Order{ Id = "1", Username = "name01@gmail.com", OrderDate = new DateTime(2018,6,29), ReferenceCode = "1" }, },
            new object[] { "2", new Order{ Id = "2", Username = "name02@gmail.com", OrderDate = new DateTime(2018,6,29), ReferenceCode = "2" }, },
            new object[] { "3", new Order{ Id = "3", Username = "name03@gmail.com", OrderDate = new DateTime(2018,6,29), ReferenceCode = "3" }, },
            new object[] { null, null, },
            new object[] { "", null, },
            new object[] { " ", null, },
            new object[] { "4", null, },
        };
        [Theory(DisplayName = "ขอรายการสั่งซื้อจากรหัสอ้างอิง")]
        [MemberData(nameof(GetOrderByReferenceCodeTestData))]
        public void GetOrderByReferenceCodeTest(string referenceCode, Order expected)
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repoProduct = mock.Create<IProductRepository>();
            var repoOrder = mock.Create<IOrderRepository>();
            var api = new OrderController(repoProduct.Object, repoOrder.Object);
            var allOrders = new List<Order>
            {
                new Order{ Id = "1", Username = "name01@gmail.com", OrderDate = new DateTime(2018,6,29), ReferenceCode = "1" },
                new Order{ Id = "2", Username = "name02@gmail.com", OrderDate = new DateTime(2018,6,29), ReferenceCode = "2" },
                new Order{ Id = "3", Username = "name03@gmail.com", OrderDate = new DateTime(2018,6,29), ReferenceCode = "3" },
            };
            repoOrder.Setup(it => it.Get(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns<Expression<Func<Order, bool>>>((expression) => allOrders.FirstOrDefault(expression.Compile()));

            var response = api.GetByReferenceCode(referenceCode);

            response.Should().BeEquivalentTo(expected);
            repoProduct.VerifyNoOtherCalls();
            repoOrder.Verify(dac => dac.Get(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once);
            repoOrder.VerifyNoOtherCalls();
        }
    }
}