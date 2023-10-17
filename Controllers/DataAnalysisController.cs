using CloudComputingProject.Data;
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
        public IActionResult Index()
        {
            //DateTime startDate = new DateTime(2023, 10, 10); // התאריך התחלה
           // DateTime endDate = new DateTime(2023, 10, 20); // התאריך סיום

            var orders = _context.Orders // המקום שאתה מביא את ההזמנות ממנו
          //      .Where(order => order.OrderDate >= startDate && order.OrderDate <= endDate)
                .ToList();
            var orderItems = _context.OrderItems;
            var productOrders = orders
                .SelectMany(order => orderItems.Where(orderItem=> orderItem.OrderId==order.Id) )// מרכז את כל הפריטים בהזמנה
                .GroupBy(item => item.ProductId) // קובץ לפי ProductId
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalOrders = group.Count() // מוצא את מספר ההזמנות עבור כל מוצר
                })
                .ToList();

            var productIds = productOrders.Select(po => po.ProductId).ToArray();
            var totalOrders = productOrders.Select(po => po.TotalOrders).ToArray();

            var productNames = new string[productIds.Length];

            for (int i = 0; i < productIds.Length; i++)
            {
                // שאילתת LINQ לשאילתת השם מבסיס הנתונים
                var productName = _context.Products.FirstOrDefault(p => p.Id == productIds[i])?.Name;
                productNames[i] = productName;
            }

            // שמירת השמות במערך ב-TempData
            TempData["ProductNames"] = productNames;

            // שמירת המערכים עם ה-ProductIds וה-TotalOrders ב-TempData
            TempData["ProductIds"] = productIds;
            TempData["TotalOrders"] = totalOrders;
            
            return View();
        }
        [HttpPost]
        public IActionResult UpdateData(DateTime startDate, DateTime endDate)
        {
            var orders = _context.Orders
                .Where(order => order.OrderDate >= startDate && order.OrderDate <= endDate)
                .ToList();

            var orderItems = _context.OrderItems;
            var productOrders = orders
                .SelectMany(order => orderItems.Where(orderItem => orderItem.OrderId == order.Id))
                .GroupBy(item => item.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalOrders = group.Count()
                })
                .ToList();

            var productIds = productOrders.Select(po => po.ProductId).ToArray();
            var totalOrders = productOrders.Select(po => po.TotalOrders).ToArray();

            var productNames = new string[productIds.Length];

            for (int i = 0; i < productIds.Length; i++)
            {
                var productName = _context.Products.FirstOrDefault(p => p.Id == productIds[i])?.Name;
                productNames[i] = productName;
            }

            TempData["ProductNames"] = productNames;
            TempData["ProductIds"] = productIds;
            TempData["TotalOrders"] = totalOrders;


            return Json(new
            {
                success = true,
                productNames = productNames,
                totalOrders = totalOrders
            });
        }


    }
}
