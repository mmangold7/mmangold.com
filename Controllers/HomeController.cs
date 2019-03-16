using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.OAuth2;
using Fitbit.Models;
using mmangold.com.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WaniKaniApi;
using Activity = System.Diagnostics.Activity;

namespace mmangold.com.Controllers
{
    public class HomeController : Controller
    {
        private SiteDbContext _context;
        
        private readonly FitbitAppCredentials _fitbitAppCredentials;
        private readonly FitbitClient _fitBitClient;

        private readonly WaniKaniClient _waniKaniClient;
        private OAuth2Helper _authenticator;
        private string _authUrl;

        public HomeController(IConfiguration configuration, SiteDbContext context)
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
            var token = apiKeys.Single(c => c.Key == "FitBitApiToken").Value;
            var accessToken = new OAuth2AccessToken();
            accessToken.Token = token;
            _fitBitClient = new FitbitClient(_fitbitAppCredentials, accessToken);
        }

        public async Task<IActionResult> Index()
        {
            var goalsProgressModel = new GoalsProgressModel();

            //WaniKani data
            try
            {
                goalsProgressModel.UserInformation = _waniKaniClient.GetUserInformation();
                //goalsProgressModel.LevelProgression = _waniKaniClient.GetLevelProgression();
                //goalsProgressModel.SrsDistribution = _waniKaniClient.GetSrsDistribution();
                goalsProgressModel.Radicals = _waniKaniClient.GetRadicals()
                    .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0);
                goalsProgressModel.Kanji = _waniKaniClient.GetKanji()
                    .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0);
                goalsProgressModel.Vocabulary = _waniKaniClient.GetVocabulary()
                    .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0);

                goalsProgressModel.TotalKanji = _waniKaniClient.GetKanji().Count;
                goalsProgressModel.TotalVocab = _waniKaniClient.GetVocabulary().Count;
            }
            catch (Exception e)
            {
            }

            //var aria = await _fitBitClient.GetDevicesAsync();
            
            //var simpleWeights = new List<SimpleWeightLog>();
            //var yearIterator = new DateTime(2019, 1, 31);
            //while (yearIterator.Month <= DateTime.Now.Day)
            //{
            //    var fitbitWeight = await _fitBitClient.GetWeightAsync(yearIterator, DateRangePeriod.OneMonth);
            //    foreach (var weight in fitbitWeight.Weights)
            //        simpleWeights.Add(new SimpleWeightLog
            //        {
            //            //js months are zero indexed. sigh
            //            Date = weight.Date.AddMonths(-1),
            //            Weight = (float)(weight.Weight * 2.20462),
            //            DayOfYear = weight.Date.DayOfYear
            //        });
            //    yearIterator = yearIterator.AddMonths(1);
            //}
 
            //goalsProgressModel.SimpleWeights = simpleWeights;

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