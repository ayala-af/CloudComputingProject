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
using System.Net;
using System.Security.Policy;

namespace CloudComputingProject.Controllers
{
    /// <summary>
    /// This is the regular controller which generated when using mvc entity framework controller
    /// based on the model: flavor
    /// </summary>
    /// 

   // [Authorize(Roles = "Admin")]
    public class FlavorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;
        private static string ApiKey = "\r\nAIzaSyC2RqgkdowSHpmrXDrDGnJcWplh_3d3xAE";
        private static string Bucket = "cloudcomputingproject-81c00.appspot.com";
        private static string AuthEmail = "ayalaaftergut@gmail.com";
        private static string AuthPassword = "15251015";
        public FlavorsController(ApplicationDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Flavors
        public async Task<IActionResult> Index()
        {
            return _context.Flavors != null ?
                        View(await _context.Flavors.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Flavors'  is null.");
        }

        // GET: Flavors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Flavors == null)
            {
                return NotFound();
            }

            var flavor = await _context.Flavors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flavor == null)
            {
                return NotFound();
            }

            return View(flavor);
        }

        // GET: Flavors/Index
        public IActionResult Create()
        {

            ViewBag.Categories = GetCategories();
            return View();
        }

        // POST: Flavors/Index
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FlavorName,Category,IsAvailable,FlavorUrl")] Flavor flavor, IFormFile imageFile)
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

							using (var httpClient = new HttpClient())
							{
                                string doubleEncodedURL = WebUtility.UrlEncode(task);
                                var apiUrl = @$"http://localhost:5258/ImaggaDal?url={doubleEncodedURL}&category={null}";
								var response = await httpClient.GetAsync(apiUrl);

								if (response.IsSuccessStatusCode)
								{
									var responseContent = await response.Content.ReadAsStringAsync();
									// Handle the response content here.
									if (responseContent == "true")
									{
										TempData["isImageConfirm"] = true;
										flavor.FlavorUrl = task;
									}
									else
										TempData["isImageConfirm"] = false;
									// downloadUrl כאן יכיל את ה-URL לתמונה שהועלתה
								}
								else
								{
									// Handle the case when the request is not successful.
									// You can check the response status code and take appropriate action.
								}
							}
							//gateway api call
							
                            //var responseJson = new WebClient().DownloadString(apiUrl);

                            
                        }
                        _context.Add(flavor);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        // טיפול בשגיאה במידה וההעלאה נכשלה
                        Console.WriteLine("Exception was thrown: {0}", ex);
                    }
                }
            }
            return View(flavor);
        }

        // GET: Flavors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Flavors == null)
            {
                return NotFound();
            }
            ViewBag.Categories = GetCategories();
            var flavor = await _context.Flavors.FindAsync(id);
            if (flavor == null)
            {
                return NotFound();
            }
            return View(flavor);
        }

        // POST: Flavors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FlavorName,Category,IsAvailable,FlavorUrl")] Flavor flavor, IFormFile? imageFile)
        {
            if (id != flavor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string flavortUrl;
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

                            flavortUrl = task;
                            // Update the URL property of the product with the new image URL
                        }
                        flavor.FlavorUrl = flavortUrl;
                    }
                        _context.Update(flavor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlavorExists(flavor.Id))
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
            return View(flavor);
        }

        // GET: Flavors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Flavors == null)
            {
                return NotFound();
            }

            var flavor = await _context.Flavors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flavor == null)
            {
                return NotFound();
            }

            return View(flavor);
        }

        // POST: Flavors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Flavors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Flavors'  is null.");
            }
            var flavor = await _context.Flavors.FindAsync(id);
            if (flavor != null)
            {
                _context.Flavors.Remove(flavor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private List<SelectListItem> GetCategories()
        {
            var categories = Enum.GetValues(typeof(CloudComputingProject.Models.Category))
                 .Cast<CloudComputingProject.Models.Category>()
                 .Select(c => new SelectListItem
                 {
                     Text = c.ToString(),
                     Value = ((int)c).ToString()
                 })
                 .ToList();
            return categories;
        }
        private bool FlavorExists(int id)
        {
            return (_context.Flavors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
