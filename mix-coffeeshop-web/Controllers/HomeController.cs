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
            var orders = orderRepo.List(o => !o.PaidDate.HasValue);
            return View(orders);
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult MenuManager()
        {
            var products = productRepo.GetAllProducts();
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
            var isDataCorrect = data != null && !string.IsNullOrEmpty(data.Name);
            if (!isDataCorrect) return null;

            var products = productRepo.GetAllProducts();
            data.Id = products.Count() + 1;
            productRepo.CreateNewProduct(data);
            return RedirectToAction(nameof(MenuManager));
        }

        [HttpGet]
        public IActionResult EditItem(int id)
        {
            var selectedProduct = productRepo.GetAllProducts().FirstOrDefault(it => it.Id == id);
            return View(selectedProduct);
        }

        [HttpPost]
        public IActionResult EditItem(Product data)
        {
            var products = productRepo.GetAllProducts();
            var selectedProduct = products.FirstOrDefault(it => it.Id == data.Id);
            if(selectedProduct == null) return null;

            selectedProduct.Name = data.Name;
            selectedProduct.Price = data.Price;
            selectedProduct.Desc = data.Desc;
            selectedProduct.ThumbURL = data.ThumbURL;
            productRepo.UpdateProduct(data);
            return RedirectToAction(nameof(MenuManager));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
