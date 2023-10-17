using CloudComputingProject.Data;
using CloudComputingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CloudComputingProject.Controllers
{
    public class DataAnalysisController : Controller
    {

        private readonly ApplicationDbContext _context;
        public DataAnalysisController(ApplicationDbContext context)
        {
            _context = context;


        }
        // GET: dataAnalasisController
        public ActionResult Index()
        {
            return View();
        }

        // GET: dataAnalasisController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: dataAnalasisController/Create
        public ActionResult Create(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                TempData["startDate"] = startDate;
                TempData["endDate"] = endDate;
                List<Order> orders;
                if (startDate != null && endDate != null)
                {
                    orders = _context.Orders
                        .Where(order => order.OrderDate >= startDate && order.OrderDate <= endDate)
                        .ToList();
                }
                else
                {
                    orders = _context.Orders.ToList();
                }
                var orderItems = _context.OrderItems;
                var productOrders = orders
                    .SelectMany(order => orderItems.Where(orderItem => orderItem.OrderId == order.Id))
                    .GroupBy(item => item.ProductId)
                    .Select(group => new
                    {
                        ProductId = group.Key,
                        TotalOrders = group.Count(),
                        Flavors = group.Select(orderItem => orderItem.Flavors).ToArray()
                    })
                    .ToList();
                var productIds = productOrders.Select(po => po.ProductId).ToArray();
                var totalOrders = productOrders.Select(po => po.TotalOrders).ToArray();
                var productNames = productIds.Select(productId => GetProductName(productId)).ToArray();
                TempData["TotalOrders"] = totalOrders;
                TempData["ProductNames"] = productNames;


                var productFlavors = productOrders
                    .Select(item => item.Flavors
                            .GroupBy(flavor => flavor) // קבץ לפי שם הטעם
                            .Select(group => new
                            {
                                FlavorId = group.Key, // השם של הטעם
                                FlavorCount = group.Count() // כמות הטעמים של אותו השם
                            }).ToArray()
                            ).ToArray();
                var flavorProductNames = productFlavors.Select(fo => fo.Select(f => _context.Flavors.Where(f1=>f1.Id.ToString()==f.FlavorId).First().FlavorName).ToArray()).ToList();
                var flavorProductCount = productFlavors.Select(fo => fo.Select(f => f.FlavorCount).ToArray()).ToList();
                TempData["FlavorProductNames"] = flavorProductNames;
                TempData["FlavorProductCount"] = flavorProductCount;

                var leagalOrderItems = orders.SelectMany(order => orderItems.Where(orderItem => orderItem.OrderId == order.Id)).ToList();
                List<string> FlavorName=new List<string>();
                List<int> FlavorTotalSale=new List<int>();
                foreach (Flavor flavor in _context.Flavors)
                {
                    FlavorName.Add(flavor.FlavorName);
                    FlavorTotalSale.Add(leagalOrderItems.Sum(orderItem => orderItem.Flavors.Split(",").Count(f=>f==(flavor.Id.ToString()))));

                }
                TempData["FlavorName"] = FlavorName;
                TempData["FlavorTotalSale"] = FlavorTotalSale;

                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        private string GetProductName(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId)?.Name;
        }


        // POST: dataAnalasisController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: dataAnalasisController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: dataAnalasisController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: dataAnalasisController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: dataAnalasisController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
