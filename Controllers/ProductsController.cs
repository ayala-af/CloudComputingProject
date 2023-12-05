using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CloudComputingProject.Data;
using CloudComputingProject.Models;
using Microsoft.AspNetCore.Authorization;
using Firebase.Auth;
using Firebase.Storage;

namespace CloudComputingProject.Controllers
{
    /// <summary>
    /// this controller managing Crud actions for product model
    /// </summary>
    /// 

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;
        private static string ApiKey = "\r\nAIzaSyC2RqgkdowSHpmrXDrDGnJcWplh_3d3xAE";
        private static string Bucket = "cloudcomputingproject-81c00.appspot.com";
        private static string AuthEmail = "ayalaaftergut@gmail.com";
        private static string AuthPassword = "15251015";

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // GET: Products
        public async Task<IActionResult> Index()
        {
            return _context.Products != null ?
                        View(await _context.Products.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }
        public async Task<IActionResult> ProductsMenu()
        {
            return _context.Products != null ?
                        View(await _context.Products.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Products'  is null.");
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Index
        public IActionResult Create()
        {

            return View();
        }

        // POST: Products/Index
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Category,Url,Price,MaxFlavorsNumber,IsAvailable")] Product product, IFormFile imageFile)
        {
            
            if (ModelState.IsValid)
            {
                // Check if an image file was uploaded
                if ((imageFile != null && imageFile.Length > 0))
                {
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                    FirebaseStorage firebaseStorage = new FirebaseStorage(
                    Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                    });


                    // Define the path where you want to store the file in Firebase Storage
                    string storagePath = "images/" + imageFile.FileName;

                    // Upload the file to Firebase Storage
                    try
                    {
                        using (var stream = imageFile.OpenReadStream())
                        {
                            var task = await firebaseStorage
                                .Child(storagePath)
                                .PutAsync(stream);


                            product.Url = task;
                        }
                        // Update the Product1 entity's ImageUrl property with the URL of the saved image

                        // Save the product to the database, including the ImageUrl
                        // ...
                        _context.Add(product);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception was thrown: {0}", ex);
                    }



                    

                }
            }
            // Redirect to the index page or another appropriate action
            return View(product);

        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Category,Url,Price,MaxFlavorsNumber,IsAvailable")] Product product,IFormFile? imageFile)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string productUrl;
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                        var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

                        FirebaseStorage firebaseStorage = new FirebaseStorage(
                            Bucket,
                            new FirebaseStorageOptions
                            {
                                AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                            });

                        // Define the path where you want to store the file in Firebase Storage
                        string storagePath = "images/" + imageFile.FileName;

                        // Upload the new image to Firebase Storage
                        using (var stream = imageFile.OpenReadStream())
                        {
                            var task = await firebaseStorage
                                .Child(storagePath)
                                .PutAsync(stream);

                             productUrl = task;
                            // Update the URL property of the product with the new image URL
                        }

                        //// Delete the old image from Firebase Storage (if needed)
                        //if (!string.IsNullOrEmpty(product.Url))
                        //{
                        //    await firebaseStorage
                        //        .Child(product.Url)
                        //        .DeleteAsync();
                        //}
                        product.Url = productUrl;
                    }
                   
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
              
            }
       
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
