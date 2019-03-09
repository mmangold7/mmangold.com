using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.OAuth2;
using mmangold.com.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WaniKaniApi;

namespace mmangold.com.Controllers
{
    public class HomeController : Controller
    {
        private readonly OAuth2Helper _authenticator;
        private readonly string _authUrl;
        private readonly FitbitAppCredentials _fitbitAppCredentials;

        public HomeController(IConfiguration configuration)
        {
            //Get third party api keys from private config file
            var apiKeys = configuration.GetSection("Keys").GetChildren();

            //Instantiate WaniKani api client using private token
            var wkApiKey = apiKeys.Single(c => c.Key == "WaniKaniApi").Value;
            _waniKaniClient = new WaniKaniClient(wkApiKey);

            //Get FitBit authorization url from private id and secret
            _fitbitAppCredentials = new FitbitAppCredentials
            {
                ClientId = apiKeys.Single(c => c.Key == "FitBitApiId").Value,
                ClientSecret = apiKeys.Single(c => c.Key == "FitBitApiSecret").Value
            };
            _authenticator = new OAuth2Helper(_fitbitAppCredentials, "https://www.mmangold.com/FitBitCallBack");
            string[] scopes = {"profile"};
            _authUrl = _authenticator.GenerateAuthUrl(scopes, null);

            //var fitBitAccessToken = new OAuth2AccessToken();
            //FitBitClient = new FitbitClient(fitbitAppCredentials, fitBitAccessToken);
        }

        private readonly WaniKaniClient _waniKaniClient;
        private FitbitClient _fitBitClient;

        public async Task<ActionResult> FitBitCallBack([FromQuery(Name = "code")] string code)
        {
            var accessToken = await _authenticator.ExchangeAuthCodeForAccessTokenAsync(code);
            _fitBitClient = new FitbitClient(_fitbitAppCredentials, accessToken);
            return Content("fitbit client initialized with access token");
        }

        public async Task<IActionResult> Index()
        {
            var goalsProgressModel = new GoalsProgressModel();

            goalsProgressModel.UserInformation = _waniKaniClient.GetUserInformation();
            goalsProgressModel.LevelProgression = _waniKaniClient.GetLevelProgression();
            goalsProgressModel.SrsDistribution = _waniKaniClient.GetSrsDistribution();
            goalsProgressModel.Radicals = _waniKaniClient.GetRadicals().FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0);
            goalsProgressModel.Kanji = _waniKaniClient.GetKanji().FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0);
            goalsProgressModel.Vocabulary = _waniKaniClient.GetVocabulary().FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0);

            goalsProgressModel.TotalKanji = _waniKaniClient.GetKanji().Count;
            goalsProgressModel.TotalVocab = _waniKaniClient.GetVocabulary().Count;

            if (_fitBitClient != null)
            {
                goalsProgressModel.FitBitProfileData = await _fitBitClient.GetUserProfileAsync();
            }

            return View(goalsProgressModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult FitBitAuth()
        {
            return Redirect(_authUrl);
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