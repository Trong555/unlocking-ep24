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
    public class AcceptOrder
    {
        [Theory(DisplayName = "ยืนยันรายการสั่งซื้อแต่ไม่ส่งรหัสรายการสั่งซื้อมา")]
        [InlineData(null, "ไม่พบรายการสั่งซื้อ")]
        [InlineData("", "ไม่พบรายการสั่งซื้อ")]
        [InlineData(" ", "ไม่พบรายการสั่งซื้อ")]
        [InlineData("99", "ไม่พบรายการสั่งซื้อ")]
        public void AcceptOrder_NoParameter(string id, string expectedMessage)
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

            try { api.AcceptOrder(id); }
            catch (Exception ex) { ex.Message.Should().Be(expectedMessage); }

            repoProduct.VerifyNoOtherCalls();
            repoOrder.Verify(dac => dac.Get(It.IsAny<Expression<Func<Order, bool>>>()), Times.AtMostOnce);
            repoOrder.VerifyNoOtherCalls();
        }

        [Theory(DisplayName = "ยืนยันรายการสั่งซื้อสำเร็จ")]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public void AcceptOrder_Success(string id)
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

            api.AcceptOrder(id);

            repoProduct.VerifyNoOtherCalls();
            repoOrder.Verify(dac => dac.Get(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once);
            Func<Order, bool> func = o =>
            {
                o.PaidDate.Should().NotBeNull();
                return true;
            };
            repoOrder.Verify(dac => dac.Update(It.Is<Order>(o => func(o))), Times.Once);
            repoOrder.VerifyNoOtherCalls();
        }
    }
}