﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CloudComputingProject.Data;
using CloudComputingProject.Models;
using System.Security.Claims;

namespace CloudComputingProject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            string userId = GetUserId();

            return _context.Orders != null ? 
                          View(await _context.Orders.Where(o=>o.UserId==userId).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Order'  is null.");
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
            string userId = GetUserId();
            var orderItems = await _context.OrderItems
                              .Where(item => item.UserId == GetUserId())
                              .ToListAsync();
            Order order = new Order();
            //order.TotalPrice = orderItems.Sum(p => p.Price);
            //order.OrderDate = DateTime.UtcNow;
            //order.UserId = userId;
            //order.OrderDay = DateTime.UtcNow.DayOfWeek;
            //order.Items = orderItems;
            ViewBag.products = await _context.Products.ToListAsync();
            ViewBag.Flavors = await _context.Flavors.ToListAsync();

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientFirstName,ClientLastName,PhoneNumber,Email,City,Street,House")] Order order)
        {
            
                // Add the order items to the order
                order.Items = await _context.OrderItems
                                           .Where(item => item.UserId == GetUserId())
                                           .ToListAsync();

                // Set the order properties
                order.TotalPrice = order.Items.Sum(p => p.Price);
                order.OrderDate = DateTime.UtcNow;
                order.UserId = GetUserId();
                order.OrderDay = DateTime.UtcNow.DayOfWeek;

                // Add the order to the database
                _context.Add(order);
            foreach(var item in _context.OrderItems)
            {
                if(item.UserId == GetUserId())
                    _context.Remove(item);
            }
                await _context.SaveChangesAsync();
         
                // Redirect to the index page
                return RedirectToAction(nameof(Index));
            

            
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
	}
}
