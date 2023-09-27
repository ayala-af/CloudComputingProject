using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using CloudComputingProject.Data;
using CloudComputingProject.Models;
using CloudComputingProject.Services;

namespace CloudComputingProject.Controllers
{
    
    public class OrderItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        


        public OrderItemsController(ApplicationDbContext context)
        {
            _context = context;
        }
      
        // GET: OrderItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderItems.ToListAsync());
        }
        /// <summary>
        /// This function called when user choose product to add to order
        /// 
        /// </summary>
        /// <param name="productId">This is the ID of the product the user choose to add</param>
        /// <returns>open view of createOrderItem to create orderItem based on the productID parameter</returns>
		public async Task<IActionResult> CreateOrderItem(int? productId)
		{
			if (productId == null || _context.Products == null)
			{
				return NotFound();
			}
			var orderItem = new OrderItem();//create new orderItem for this product
			orderItem.ProductId = (int)productId;//assign the productID value 
			var product = _context.Products.FirstOrDefault(p => p.Id == productId);//find this product in the exist products, in order to know the category
            var flavors = _context.Flavors.ToList().Where(p => p.Category == product.Category);//make list of flavors with correct category
        /// Create a MultiSelectList object for contain a dropdown list with flavors.
        /// The 'flavors' collection contains the available flavor options.
        /// The 'Id' is what is name of the key of flavor model
        /// The 'FlavorName' is what i want to desplay

            ViewBag.Flavors = new MultiSelectList(flavors, "Id", "FlavorName");

			return View(orderItem);//send the orderItem to the view of CreateOrderItem
        }
        /// <summary>
        /// Save the new orderItem 
        /// </summary>
        /// <param name="orderItem"></param>
        /// <param name="flavors"></param>
        /// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateOrderItem(OrderItem orderItem, List<int> flavors)
		{
			if (ModelState.IsValid)
			{
				// Convert the list of selected flavors to a comma-separated string
				string flavorsString = string.Join(",", flavors);

				// Assign the selected flavors to the orderItem
				orderItem.Flavors = flavorsString;

				// Add the order item to the context
				_context.OrderItems.Add(orderItem);

				// Save the changes to the context
				await _context.SaveChangesAsync();

				return RedirectToAction(nameof(Index));

			}
            
          

			ViewBag.Flavors = new MultiSelectList(_context.Flavors.ToList(), "Id", "FlavorName");
            ///the products for manue
            var productsList = _context.Products.ToList();
            return View(productsList);
		}

		// GET: OrderItems/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderItems == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }
        public IActionResult Create()
        {
            // Replace this with code that fetches the actual flavors from your database or another data source
            var flavors = _context.Flavors.ToList();
            var products = _context.Products.ToList();
            // Create a MultiSelectList with the fetched flavors

            ViewBag.Flavors = new MultiSelectList(flavors, "Id", "FlavorName");
            ViewBag.Products = new SelectList(products, "Id", "Name");

            return View();
        }


    

        // POST: OrderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        // Create action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderItem orderItem, List<int> flavors)
        {
            if (ModelState.IsValid)
            {
                // Convert the list of selected flavors to a comma-separated string
                string flavorsString = string.Join(",", flavors);

                // Assign the selected flavors to the orderItem
                orderItem.Flavors = flavorsString;

                // Add the order item to the context
                _context.OrderItems.Add(orderItem);

                // Save the changes to the context
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Repopulate ViewBag.Products and ViewBag.Flavors in case of model validation errors
            var flavorsList = _context.Flavors.ToList();
            var productsList = _context.Products.ToList();

            ViewBag.Products = new SelectList(productsList, "Id", "Name");
            ViewBag.Flavors = new MultiSelectList(flavorsList, "Id", "FlavorName");

            return View(orderItem);
        }





        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderItems == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            // Fetch the products and flavors for the dropdowns
            var flavors = _context.Flavors.ToList();
            var products = _context.Products.ToList();

            // Create a MultiSelectList with the fetched flavors
            ViewBag.Flavors = new MultiSelectList(flavors, "Id", "FlavorName");

            // Create a SelectList with the fetched products
            ViewBag.Products = new SelectList(products, "Id", "Name");

            // Set the selected values for the product and flavors dropdowns based on the orderItem
            ViewBag.SelectedFlavors = orderItem.Flavors.Split(',').Select(int.Parse).ToList();

            return View(orderItem);
        }


        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Flavors,OrderId")] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderItems == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.OrderItems'  is null.");
            }
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderItemExists(int id)
        {
          return (_context.OrderItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
