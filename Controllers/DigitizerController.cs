using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mmangold.com.Models;

namespace mmangold.com.Controllers
{
    public class DigitizerController : Controller
    {
        private readonly SiteContext _context;
        //private DigitRecognizer _digitRecognizer = new DigitRecognizer();

        public DigitizerController(SiteContext context)
        {
            _context = context;
        }

        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Digitize(int image)
        {
            //List<DigitResult> results = _digitRecognizer.Evaluate(GetHandWrittenImage());
            return Json(new { cat = 5 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveResult(LabeledUserImage image)
        {
            bool success = false;
            if (ModelState.IsValid)
            {
                _context.Add(image);
                await _context.SaveChangesAsync();
                success = true;
            }
            return Json(new
            {
                success
            });
        }
    }
}
