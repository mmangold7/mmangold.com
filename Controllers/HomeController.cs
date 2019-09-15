using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitbit.Api.Portable;
using Fitbit.Api.Portable.OAuth2;
using Fitbit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mmangold.com.Models;
using WaniKaniApi;
using Activity = System.Diagnostics.Activity;

namespace mmangold.com.Controllers
{
    public class HomeController : Controller
    {
        private readonly SiteDbContext _context;
        private readonly FitbitAppCredentials _fitbitAppCredentials;
        private readonly FitbitClient _fitBitClient;

        private readonly WaniKaniClient _waniKaniClient;
        //private OAuth2Helper _authenticator;
        //private string _authUrl;

        public HomeController(IConfiguration configuration, SiteDbContext context)
        {
            _context = context;
            //Get third party api keys from private config file
            var apiKeys = configuration.GetSection("Keys").GetChildren();

            //Instantiate WaniKani api client using private token
            var apiKeysList = apiKeys.ToList();
            var wkApiKey = apiKeysList.Single(c => c.Key == "WaniKaniApi").Value;
            _waniKaniClient = new WaniKaniClient(wkApiKey);

            //Get FitBit authorization url from private id and secret
            _fitbitAppCredentials = new FitbitAppCredentials
            {
                ClientId = apiKeysList.Single(c => c.Key == "FitBitApiId").Value,
                ClientSecret = apiKeysList.Single(c => c.Key == "FitBitApiSecret").Value
            };
            var token = apiKeysList.Single(c => c.Key == "FitBitApiToken").Value;
            var accessToken = new OAuth2AccessToken();
            accessToken.Token = token;
            _fitBitClient = new FitbitClient(_fitbitAppCredentials, accessToken);
        }

        public async Task<JsonResult> GetProgressModel()
        {
            //Initial load WK data if none exists in db or no init load has been done yet this day
            if (!_context.GuruOrGreaterRadicals.Any() ||
                !_context.WaniKaniSyncs.Any(s => s.SyncDate.DayOfYear == DateTime.Now.DayOfYear))
            {
                _context.GuruOrGreaterRadicals.RemoveRange(_context.GuruOrGreaterRadicals);
                _context.GuruOrGreaterKanjis.RemoveRange(_context.GuruOrGreaterKanjis);
                _context.GuruOrGreaterVocabs.RemoveRange(_context.GuruOrGreaterVocabs);
                _context.SaveChanges();

                var guruRadicals = new List<GuruOrGreaterRadical>();
                var guruKanjis = new List<GuruOrGreaterKanji>();
                var guruVocabs = new List<GuruOrGreaterVocab>();

                _waniKaniClient.GetRadicals()
                    .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0).ForEach(r =>
                        guruRadicals.Add(new GuruOrGreaterRadical
                        {
                            ImageUri = r.ImageUri,
                            UnlockedDate = r.UserInfo.UnlockedDate
                        }));
                _waniKaniClient.GetKanji()
                    .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0).ForEach(r =>
                        guruKanjis.Add(new GuruOrGreaterKanji
                        {
                            Character = r.Character,
                            UnlockedDate = r.UserInfo.UnlockedDate
                        }));
                _waniKaniClient.GetVocabulary()
                    .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0).ForEach(r =>
                        guruVocabs.Add(new GuruOrGreaterVocab
                        {
                            Character = r.Character,
                            UnlockedDate = r.UserInfo.UnlockedDate
                        }));

                await _context.GuruOrGreaterRadicals.AddRangeAsync(guruRadicals);
                await _context.GuruOrGreaterKanjis.AddRangeAsync(guruKanjis);
                await _context.GuruOrGreaterVocabs.AddRangeAsync(guruVocabs);
                await _context.LevelProgresses.AddAsync(new LevelProgress
                    {Level = _waniKaniClient.GetUserInformation().Level.ToString(), CreatedDateTime = DateTime.Now});

                await _context.WaniKaniSyncs.AddAsync(new WaniKaniSync
                {
                    SyncDate = DateTime.Now
                });

                _context.SaveChanges();
            }

            return Json(new GoalsProgressModel
            {
                Radicals = _context.GuruOrGreaterRadicals.OrderBy(i => i.UnlockedDate).ToList(),
                Kanji = _context.GuruOrGreaterKanjis.OrderBy(i => i.UnlockedDate).ToList(),
                Vocabulary = _context.GuruOrGreaterVocabs.OrderBy(i => i.UnlockedDate).ToList(),
                Level = _context.LevelProgresses.OrderByDescending(mi => mi.CreatedDateTime).First().Level
            });
        }

        public IActionResult Index()
        {
            return View();
        }

        //fix this method or passed paramters. updated weights are returning all weights in the past week from the startdate
        public async Task GetInitialFitBitWeightData(DateTime startDate, DateTime endDate)
        {
            _context.SimpleWeightLogs.RemoveRange(_context.SimpleWeightLogs);
            _context.SaveChanges();

            var simpleWeights = new List<SimpleWeightLog>();
            while (startDate <= endDate)
            {
                var simpleWeightsForDateRange = new List<SimpleWeightLog>();
                var weight = await _fitBitClient.GetWeightAsync(endDate, DateRangePeriod.OneWeek);
                weight.Weights.ForEach(w => simpleWeightsForDateRange.Add(new SimpleWeightLog
                {
                    Weight = w.Weight,
                    Date = w.DateTime,
                    DayOfYear = w.DateTime.DayOfYear
                }));
                simpleWeights.AddRange(simpleWeightsForDateRange);
                endDate = endDate.AddDays(-7);
            }

            try
            {
                await _context.SimpleWeightLogs.AddRangeAsync(simpleWeights);
            }
            catch (Exception)
            {
                foreach (var l in simpleWeights) await _context.SimpleWeightLogs.AddAsync(l);
            }

            await _context.WeightSyncs.AddAsync(new WeightSync
            {
                NewRecords = simpleWeights.Count,
                SyncDate = DateTime.Now
            });

            await _context.SaveChangesAsync();
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