using System.Diagnostics;
using System.Linq;
using mmangold.com.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WaniKaniApi;

namespace mmangold.com.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration Configuration { get; set; }

        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            var goalsProgressModel = new GoalsProgressModel();

            var wkApiKey = Configuration.GetSection("Keys").GetChildren().Single(c => c.Key == "WaniKaniApi").Value;
            var wkClient = new WaniKaniClient(wkApiKey);


            return View(goalsProgressModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}