using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CloudComputingProject.Data;
using CloudComputingProject.Models;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNet.Identity;
using System.IO;

namespace CloudComputingProject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        public OrdersController(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            string userId = GetUserId();

            return _context.Orders != null ?
                          View(await _context.Orders.Where(o => o.UserId == userId).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Order'  is null.");
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }
            var products = await _context.Products.ToListAsync();
            var flavors = await _context.Flavors.ToListAsync();

            ViewData["Products"] = products;
            ViewData["Flavors"] = flavors;
            var order = await _context.Orders.
                FirstOrDefaultAsync(m => m.Id == id);
            order.Items = _context.OrderItems.Where(i => i.OrderId == id).ToList();
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Index
        public async Task<IActionResult> Create(decimal? orderTotal)
        {

            string userId = GetUserId();
            var items = await _context.OrderItems.Where(item => item.UserId == userId && item.OrderId == 0).ToListAsync();

            Order order = new();
            //order.TotalPrice = orderItems.Sum(p => p.Price);
            //order.OrderDate = DateTime.UtcNow;
            order.UserId = userId;
            //order.OrderDay = DateTime.UtcNow.DayOfWeek;
            //order.Items = orderItems;
            var user = await _userManager.FindByIdAsync(userId);


            
            order.ClientFirstName = user.FirstName;
            order.ClientLastName = user.LastName;
            order.PhoneNumber = user.PhoneNumber;
            order.Email = user.Email;

            ////gateway api call to verify location existence
            //var apiUrl = $"http://www.apigateway.somee.com/Address?city={user.City}&street={user.Street}";
            
            //    // Create an instance of HttpClient
            //    using (var httpClient = new HttpClient())
                //{
            //        // Send a GET request to the other project's endpoint
            //        var response = await httpClient.GetAsync(apiUrl);

            //        // Check if the request was successful
            //        if (response.IsSuccessStatusCode)
            //        {
            //            // Parse the response content as a boolean value
            //            bool isValidAddress = bool.Parse(await response.Content.ReadAsStringAsync());

            //            if (isValidAddress)
            //            {
            //                order.City = user.City;
            //                order.Street = user.Street;
            //                order.House = user.HouseNumber ?? 0;
            //            }
            //        }
            //        else
            //        {
            //            //
            //        }




                ViewBag.OrderItems = items;
                    ViewBag.products = await _context.Products.ToListAsync();
                    ViewBag.Flavors = await _context.Flavors.ToListAsync();
                    TempData["Price"] = items.Sum(item => item.Price);
                    TempData["IsPayed"] = false;
                    TempData["OrderItems"] = items;
                    return View(order);
                
            
            }

        // POST: Orders/Index
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientFirstName,ClientLastName,PhoneNumber,Email,City,Street,House,TotalPrice,UserId")] Order order)
        {
            bool isvalidAddress = await IsValidAddress(order.City, order.Street, "0");

			if (ModelState.IsValid && isvalidAddress)
			{

                // Add the order items to the order
                order.Items = await _context.OrderItems.
                Where(item => item.UserId == GetUserId() && item.OrderId == 0)
                .ToListAsync();

                // Set the order properties
                order.TotalPrice = order.Items.Sum(p => p.Price);
                order.OrderDate = DateTime.Now;
				order.IsHoliday = await IsHoliday();
                Weather weather = await GetWeather(order.City);
                order.Humidity = weather.humidity;
                order.Temperature = weather.temp;
                order.FeelsLike = weather.feels_like;
				order.OrderDay = DateTime.UtcNow.DayOfWeek;

				
               

					// Add the order to the database
					_context.Add(order);
                //foreach(var item in _context.OrderItems)
                //{
                //    if(item.UserId == GetUserId())
                //        _context.Remove(item);
                //}
                await _context.SaveChangesAsync();
                foreach (OrderItem item in order.Items)
                {
                    item.OrderId = order.Id;
                    _context.Update(item);
                }
                await _context.SaveChangesAsync();
              
                var emailSender = new emailsender();
                emailSender.SendEmail(User.FindFirst(ClaimTypes.Email).ToString(), "Order Confirmation","Thank you for your order");

                // Redirect to the index page
                return RedirectToAction(nameof(Details), new { Id = order.Id });
            }

            string userId = GetUserId();
            var items = await _context.OrderItems.Where(item => item.UserId == userId && item.OrderId == 0).ToListAsync();


            //order.TotalPrice = orderItems.Sum(p => p.Price);
            //order.OrderDate = DateTime.UtcNow;
            //order.UserId = userId;
            //order.OrderDay = DateTime.UtcNow.DayOfWeek;
            //order.Items = orderItems;
            TempData["IsPayed"] = true;
            ViewBag.OrderItems = items;
            ViewBag.products = await _context.Products.ToListAsync();
            ViewBag.Flavors = await _context.Flavors.ToListAsync();
            TempData["Price"] = items.Sum(item => item.Price);
            return View(order);

        }

        public async Task<bool> IsValidAddress(string city, string street, string? num)
        {
			//gateway api call to verify location existence
			var apiUrl = $"http://www.apigateway.somee.com/Address?city={city}&street={street}";

			// Create an instance of HttpClient
			using (var httpClient = new HttpClient())
			{
				// Send a GET request to the other project's endpoint
				var response = await httpClient.GetAsync(apiUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content as a boolean value
                    bool isValidAddress = bool.Parse(await response.Content.ReadAsStringAsync());

                    return isValidAddress;
                }
                return false;
			}
		}

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientFirstName,ClientLastName,Price,PhoneNumber,OrderDate,Email,City,Street,House,FeelsLike,Humidity,Temperature,IsHoliday,OrderDay,TotalPrice")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Order'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> IsHoliday()
        {
			//gateway api call to verify location existence
			var apiUrl = $"http://www.apigateway.somee.com/HebrewDate/IsHebrewHoliday";

			// Create an instance of HttpClient
			using (var httpClient = new HttpClient())
			{
                    // Send a GET request to the other project's endpoint
                    var response = await httpClient.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content as a boolean value
                        bool isHoliday = bool.Parse(await response.Content.ReadAsStringAsync());

                        return isHoliday;
                    }
				else
				{
					// Throw an exception if the request was not successful
					throw new Exception($"Failed to fetch data from the API. Status Code: {response.StatusCode}");
				}
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">English location</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
		public async Task<Weather> GetWeather(string location)
		{
			//gateway api call to verify location existence
			var apiUrl = $"http://www.apigateway.somee.com/WeatherForecast?location={location}";

			// Create an instance of HttpClient
			using (var httpClient = new HttpClient())
			{
				// Send a GET request to the other project's endpoint
				var response = await httpClient.GetAsync(apiUrl);

				// Check if the request was successful
				if (response.IsSuccessStatusCode)
				{
					// Parse the response content as a boolean value
					var jsonContent = response.Content.ReadAsStringAsync().Result;
                    var weather = JsonConvert.DeserializeObject<Weather>(jsonContent);
					return weather;
				}
				else
				{
					// Throw an exception if the request was not successful
					throw new Exception($"Failed to fetch data from the API. Status Code: {response.StatusCode}");
				}
			}
		}
	}
}