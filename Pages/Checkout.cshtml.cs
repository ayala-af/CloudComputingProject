using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CloudComputingProject.Pages
{
    public class CheckoutModel : PageModel
    {
        public string PaypalClientId { get; set; } = "";
        public string PaypalSecret{ get; set; } = "";
        public string PaypalUrl { get; set; } = "";


        public string DeliveryAddress { get; set; } = "";
        public string Total { get; set; } = "";
        public string ProductIdentifiers{ get; set; } = "";
        public CheckoutModel(IConfiguration configuration)
        {
            PaypalClientId = configuration["PayPalSettings:ClientId"]!;
            PaypalSecret = configuration["PayPalSettings:Secret"]!;
            PaypalUrl = configuration["PayPalSettings:Url"]!;

            
        }
        public void OnGet()
        {
            DeliveryAddress = "hapalmach 15 tel-aviv";//TempData["DeliveryAdfress"]?.ToString() ?? "";
            Total = "200";// TempData["Total"]?.ToString() ?? "";
            ProductIdentifiers = "1-2-3";// TempData["ProductIdentifiers"]?.ToString() ?? "";
            TempData.Keep();

            if(DeliveryAddress==""|Total==""|ProductIdentifiers=="")
            {
                Response.Redirect("/");
                return;
            }
        }
    }
}
