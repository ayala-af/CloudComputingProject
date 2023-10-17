using CloudComputingProject.Data;
using CloudComputingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CloudComputingProject.Controllers
{
    public class dataAnalasisController : Controller
    {

        private readonly ApplicationDbContext _context;
        public dataAnalasisController(ApplicationDbContext context)
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

                var flavorCount = productFlavors.Select(fo => fo.Select(f => f.FlavorCount).ToArray()).ToList();

                TempData["ProductIds"] = productIds;
                TempData["TotalOrders"] = totalOrders;
                TempData["ProductNames"] = productNames;

                TempData["FlavorId"] = flavorProductNames;
                TempData["FlavorCount"] = flavorCount;





                TempData["startDate"] = startDate;
                TempData["endDate"] = endDate;


                var flavorIds = _context.Flavors.Select(flavor => flavor.Id).ToArray();

                // קבל את השמות של כל הטעמים
                var flavorNames = _context.Flavors.Select(flavor => flavor.FlavorName).ToArray();


                var legalorderitrms = orders.SelectMany(order => orderItems.Where(orderItem => orderItem.OrderId == order.Id)).ToList();
                List<string> FlavorId1=new List<string>();
                List<int> TotalSales1=new List<int>();
                foreach (Flavor flavor in _context.Flavors)
                {
                    FlavorId1.Add(flavor.Id.ToString());
                    TotalSales1.Add(legalorderitrms.Sum(orderItem => orderItem.Flavors.Split(",").Count(f=>f==(flavor.Id.ToString()))));

                }
           
                TempData["FlavorId1"] = FlavorId1;
                TempData["TotalSales1"] = TotalSales1;
                return View();
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות כאן, לדוגמה, לשלוח את השגיאה ללוגים או להציג הודעה למשתמש
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
