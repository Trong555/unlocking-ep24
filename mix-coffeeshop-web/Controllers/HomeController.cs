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
    public class HomeController : Controller
    {
        private IProductRepository productRepo;
        private IOrderRepository orderRepo;

        public HomeController(IProductRepository productRepo, IOrderRepository orderRepo)
        {
            this.productRepo = productRepo;
            this.orderRepo = orderRepo;
        }

        public IActionResult Index()
        {
            var api = new OrderController(productRepo, orderRepo);
            var orders = api.ListOrdering();
            return View(orders);
        }

        [HttpPost("{id}")]
        public IActionResult AcceptOrder(string id)
        {
            var api = new OrderController(productRepo, orderRepo);
            try
            {
                api.AcceptOrder(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return NotFound();
            }
        }

        public IActionResult History()
        {
            var api = new OrderController(productRepo, orderRepo);
            var orders = api.ListHistory();
            return View(orders);
        }

        public IActionResult MenuManager()
        {
            var api = new ProductController(productRepo);
            var products = api.Get();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddItem()
        {
            return View(new Product());
        }

        [HttpPost]
        public IActionResult AddItem(Product data)
        {
            var api = new ProductController(productRepo);
            api.CreateNewProduct(data);
            return RedirectToAction(nameof(MenuManager));
        }

        [HttpGet]
        public IActionResult EditItem(int id)
        {
            var api = new ProductController(productRepo);
            var selectedProduct = api.Get().FirstOrDefault(it => it.Id == id);
            return View(selectedProduct);
        }

        [HttpPost]
        public IActionResult EditItem(Product data)
        {
            var api = new ProductController(productRepo);
            var selectedProduct = api.UpdateProduct(data);
            return RedirectToAction(nameof(MenuManager));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
