using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace signWeb.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        //public IActionResult ButtonClick()
        //{
        //   // return View();
        //}
        public IActionResult check(string button)
        {
            if (!string.IsNullOrEmpty(button))
            {
                TempData["ButtonValue"] = string.Format("{0} button clicked.", button);
            }
            else
            {
                TempData["ButtonValue"] = "No button click!";
            }
            return RedirectToAction("ButtonClick");
        }
        //public void OnGet()
        //{
        //}
    }
}
