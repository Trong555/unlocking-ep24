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
    public class ListOrderByUsername
    {
        public static IEnumerable<object[]> ListOrderByUsernameTestData = new List<object[]>{
            new object[] { "name01@gmail.com", new List<Order>{ new Order{ Username = "name01@gmail.com", }, }, },
            new object[] { "name02@gmail.com", new List<Order>{ new Order{ Username = "name02@gmail.com", }, new Order{ Username = "name02@gmail.com", },  }, },
            new object[] { "name03@gmail.com", new List<Order>{ new Order{ Username = "name03@gmail.com", }, new Order{ Username = "name03@gmail.com", }, new Order{ Username = "name03@gmail.com", }, }, },
            new object[] { "name04@gmail.com", new List<Order>{ new Order{ Username = "name04@gmail.com", }, new Order{ Username = "name04@gmail.com", }, new Order{ Username = "name04@gmail.com", }, new Order{ Username = "name04@gmail.com", }, }, },
            new object[] { null, new List<Order>{ }, },
            new object[] { "", new List<Order>{ }, },
            new object[] { " ", new List<Order>{ }, },
            new object[] { "name05@gmail.com", new List<Order>{ }, },
        };
        [Theory(DisplayName = "ขอรายการสั่งซื้อจากรหัสอ้างอิง")]
        [MemberData(nameof(ListOrderByUsernameTestData))]
        public void ListOrderByUsernameTest(string referenceCode, IEnumerable<Order> expected)
        {
            var mock = new MockRepository(MockBehavior.Default);
            var repoProduct = mock.Create<IProductRepository>();
            var repoOrder = mock.Create<IOrderRepository>();
            var api = new OrderController(repoProduct.Object, repoOrder.Object);
            var allOrders = new List<Order>
            {
                new Order{ Username = "name01@gmail.com", },
                new Order{ Username = "name02@gmail.com", },
                new Order{ Username = "name02@gmail.com", },
                new Order{ Username = "name03@gmail.com", },
                new Order{ Username = "name03@gmail.com", },
                new Order{ Username = "name03@gmail.com", },
                new Order{ Username = "name04@gmail.com", },
                new Order{ Username = "name04@gmail.com", },
                new Order{ Username = "name04@gmail.com", },
                new Order{ Username = "name04@gmail.com", },
            };
            repoOrder.Setup(it => it.List(It.IsAny<Expression<Func<Order, bool>>>()))
                .Returns<Expression<Func<Order, bool>>>((expression) => allOrders.Where(expression.Compile()).ToList());

            var response = api.ListByUsername(referenceCode);

            response.Should().BeEquivalentTo(expected);
            repoProduct.VerifyNoOtherCalls();
            repoOrder.Verify(dac => dac.List(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once);
            repoOrder.VerifyNoOtherCalls();
        }
    }
}