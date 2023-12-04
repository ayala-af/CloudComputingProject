using CloudComputingProject.Data;
using CloudComputingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.DependencyResolver;
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

        public ActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                TempData["startDate"] = startDate;
                TempData["endDate"] = endDate;
                var orderItems = _context.OrderItems;
                List<object> data = new List<object>();
                List<string> labels = new List<string>();
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
                    var ordersByDate = orders.OrderBy(order => order.OrderDate).ToList();
                    startDate=ordersByDate[0].OrderDate;
                    endDate=ordersByDate[ordersByDate.Count - 1].OrderDate;
                }

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
                // var productIds = productOrders.Select(po => po.ProductId).ToArray();
                // var totalOrders = productOrders.Select(po => po.TotalOrders).ToArray();
                // var productNames = productIds.Select(productId => GetProductName(productId)).ToArray();
                TempData["TotalOrders"] = GetTotalOrders(orders);
                TempData["ProductNames"] = GetGetProductsNames();



                var productFlavors = productOrders
                    .Select(item => item.Flavors
                            .GroupBy(flavor => flavor) // קבץ לפי שם הטעם
                            .Select(group => new
                            {
                                FlavorId = group.Key, // השם של הטעם
                                FlavorCount = group.Count() // כמות הטעמים של אותו השם
                            }).ToArray()
                            ).ToArray();
                var flavorProductNames = productFlavors.Select(fo => fo.Select(f => _context.Flavors.Where(f1 => f1.Id.ToString()==f.FlavorId).First().FlavorName).ToArray()).ToList();
                var flavorProductCount = productFlavors.Select(fo => fo.Select(f => f.FlavorCount).ToArray()).ToList();
                TempData["FlavorProductNames"] = flavorProductNames;
                TempData["FlavorProductCount"] = flavorProductCount;

                var leagalOrderItems = orders.SelectMany(order => orderItems.Where(orderItem => orderItem.OrderId == order.Id)).ToList();
                List<string> FlavorName = new List<string>();
                List<int> FlavorTotalSale = new List<int>();
                foreach (Flavor flavor in _context.Flavors)
                {
                    FlavorName.Add(flavor.FlavorName);
                    FlavorTotalSale.Add(leagalOrderItems.Sum(orderItem => orderItem.Flavors.Split(",").Count(f => f==(flavor.Id.ToString()))));

                }
                TempData["FlavorName"] = FlavorName;
                TempData["FlavorTotalSale"] = FlavorTotalSale;


                while (startDate <= endDate)
                {
                    labels.Add(startDate?.ToString("MMMM/yyyy"));
                    startDate = startDate?.AddMonths(1);
                }
                var legalOrderItems = orders.Select(order => orderItems.Where(orderItem => orderItem.OrderId == order.Id).ToList());

                var legalFlavors = legalOrderItems.Select(orderItemList => orderItemList.SelectMany(item => item.Flavors.Split(",").Select(f => new { FlavorId = f, Date = orders.Find(order => order.Id==item.OrderId).OrderDate })));
                var flavorsOnSale = legalFlavors.SelectMany(x => x).ToList();//.GroupBy(f => f.FlavorId);
                                                                             // var f = flavorsOnSale.Select(group => new { label = _context.Flavors.FirstOrDefault(flavor => flavor.Id.ToString()==group.Key).FlavorName, data = labels.Select(lab => group.Where(item => item.Date.ToString("MMMM/yyyy")==lab).Count()), hidden = true }as object).ToList();//group.GroupBy(item => item.Date.ToString("MMMM/yyyy")).Select(group1 => group1.Count()).ToList() }).ToList();
                List<object> fl = new List<object>();
                foreach (Flavor flavor in _context.Flavors)
                {
                    fl.Add(new
                    {
                        label = flavor.FlavorName,
                        data = labels.Select(lab => flavorsOnSale.Where(f1 => f1.FlavorId==flavor.Id.ToString()).Where(item => item.Date.ToString("MMMM/yyyy")==lab).Count()),
                        hidden = true
                    } as object);
                }

                TempData["Flavor1Labels"] = labels;
                TempData["Flavor1Data"] = fl;
                return View();
            }
            catch (Exception ex)
            {
                return View();
                // return View("Error");
            }
        }
        //מחזיר את  הטוטאלי של המכירות. גרף 1
        private int[] GetTotalOrders(List<Order> orders)
        {
            var productOrders = orders
    .SelectMany(order => _context.OrderItems.Where(orderItem => orderItem.OrderId == order.Id))
    .GroupBy(item => item.ProductId)
    .Select(group => new
    {
        ProductId = group.Key,
        TotalOrders = group.Count()
    })
    .ToList();

            var productIds = _context.Products.Select(product => product.Id).ToList();
            int[] totalOrders = productIds.Select(productId => productOrders.FirstOrDefault(productOrder => productOrder.ProductId==productId)?.TotalOrders??0).ToArray();
            return totalOrders;
            //if totalordes!=null

        }
        private string[] GetGetProductsNames()
        {
            var products = _context.Products;
            var productIds = _context.Products.Select(product => product.Id).ToList();
            return productIds.Select(productId => products.FirstOrDefault(product => product.Id==productId)?.Name??" ").ToArray();
        }


    }

}
