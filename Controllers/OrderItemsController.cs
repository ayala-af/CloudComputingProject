using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CloudComputingProject.Services;
using CloudComputingProject.Data;
using CloudComputingProject.Models;


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CloudComputingProject.Controllers
{

    public class OrderItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        // private readonly ICartRepository _cartRepo;
        //  private readonly UserManager<IdentityUser> _userManager;
        //  private readonly IHttpContextAccessor _httpContextAccessor;
        //private ShoppingCart _shoppingCart;
        public OrderItemsController(ApplicationDbContext context /*,ICartRepository cartRepo*//*IHttpContextAccessor httpContextAccessor,UserManager<IdentityUser> userManager*/)
        {
            _context = context;
            //   _cartRepo = cartRepo;
            //_shoppingCart = new ShoppingCart
            //{
            //    CartDetails = new List<OrderItem> { }
            //};
        }



        // GET: OrderItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderItems.ToListAsync());
        }
        // GET: OrderItems
        public async Task<IActionResult> Cart()
        {
            var filteredItems = await _context.OrderItems
                  .Where(item => item.UserId == GetUserId())
                  .ToListAsync();


            return View(filteredItems);
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

            var orderItem = new OrderItem();
            orderItem.ProductId = (int)productId;

          

            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            var flavors = _context.Flavors.ToList().Where(p => p.Category == product.Category);

            ViewBag.Flavors = flavors;
            ViewBag.Product = product;
            return View(orderItem);
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

            // Convert the list of selected flavors to a comma-separated string
            string flavorsString = string.Join(",", flavors);

            // Assign the selected flavors to the orderItem
            orderItem.Flavors = flavorsString;
            // Get the current user's ID from the User.Identity
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                // Handle the case where the user's ID claim is not found
                return RedirectToAction("Login", "Account"); // Redirect to the login page or handle as needed
            }

            var currentUserId = userIdClaim.Value;

            // Assign the current user's ID to the orderItem
            orderItem.UserId = currentUserId;
            // Add the order item to the context
            _context.OrderItems.Add(orderItem);

            // Save the changes to the context
            await _context.SaveChangesAsync();

            return RedirectToAction("Cart");




            //ViewBag.Flavors = new MultiSelectList(_context.Flavors.ToList(), "Id", "FlavorName");
            //         ///the products for manue
            //         var productsList = _context.Products.ToList();
            //         return View(productsList);
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
        private string GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                // Handle the case where the user's ID claim is not found
                return "000"; // Redirect to the login page or handle as needed
            }

            return userIdClaim.Value;
        }
    }
}
