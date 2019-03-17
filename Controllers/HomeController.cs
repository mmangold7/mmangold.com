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
        private readonly FitbitAppCredentials _fitbitAppCredentials;
        private readonly FitbitClient _fitBitClient;

        private readonly WaniKaniClient _waniKaniClient;
        private OAuth2Helper _authenticator;
        private string _authUrl;
        private SiteDbContext _context;

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

        //public async void SaveInitialWaniKaniData()
        //{
        //    await _context.RadicalItems.AddRangeAsync(_waniKaniClient.GetRadicals()
        //        .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0).Cast<WaniKaniRadicalItemLocal>());
        //    await _context.KanjiItems.AddRangeAsync(_waniKaniClient.GetKanji()
        //        .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0).Cast<WaniKaniKanjiItemLocal>());
        //    await _context.VocabularyItems.AddRangeAsync(_waniKaniClient.GetVocabulary()
        //        .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0).Cast<WaniKaniVocabularyItemLocal>());
        //    await _context.SaveChangesAsync();
        //}

        public async void GetAllGuruOrGreaterItems()
        {
            var guruRadicals = new List<GuruOrGreaterRadical>();
            _waniKaniClient.GetRadicals()
                .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0).ForEach(r =>
                    guruRadicals.Add(new GuruOrGreaterRadical
                    {
                        ImageUri = r.ImageUri,
                        UnlockedDate = r.UserInfo.UnlockedDate
                    }));
            await _context.GuruOrGreaterRadicals.AddRangeAsync(guruRadicals);

            var guruKanjis = new List<GuruOrGreaterKanji>();
            _waniKaniClient.GetKanji()
                .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0).ForEach(r =>
                    guruKanjis.Add(new GuruOrGreaterKanji
                    {
                        Character = r.Character,
                        UnlockedDate = r.UserInfo.UnlockedDate
                    }));
            await _context.GuruOrGreaterKanjis.AddRangeAsync(guruKanjis);

            var guruVocabs = new List<GuruOrGreaterVocab>();
            _waniKaniClient.GetVocabulary()
                .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0).ForEach(r =>
                    guruVocabs.Add(new GuruOrGreaterVocab
                    {
                        Character = r.Character,
                        UnlockedDate = r.UserInfo.UnlockedDate
                    }));
            await _context.GuruOrGreaterVocabs.AddRangeAsync(guruVocabs);

            await _context.WaniKaniSyncs.AddAsync(new WaniKaniSync()
            {
                SyncDate = DateTime.Now
            });

            await _context.SaveChangesAsync();
        }

        public async Task<IActionResult> Index()
        {
            var goalsProgressModel = new GoalsProgressModel();

            //Initial load WK data if none exists in db or no init load has been done yet this day
            if (!_context.GuruOrGreaterRadicals.Any() || !_context.WaniKaniSyncs.Any(s => s.SyncDate.Day == DateTime.Now.Day))
            {
                GetAllGuruOrGreaterItems();
            }
            //Retrieve deltas


            goalsProgressModel.UserInformation = _waniKaniClient.GetUserInformation();
            goalsProgressModel.Radicals = _context.GuruOrGreaterRadicals.OrderBy(r => r.UnlockedDate).ToList();
            goalsProgressModel.Kanji = _context.GuruOrGreaterKanjis.OrderBy(k => k.UnlockedDate).ToList();
            goalsProgressModel.Vocabulary = _context.GuruOrGreaterVocabs.OrderBy(v => v.UnlockedDate).ToList();

            //These two count calls can take 5 seconds total... don't do them every time
            //goalsProgressModel.TotalKanji = _waniKaniClient.GetKanji().Count();
            //goalsProgressModel.TotalVocab = _waniKaniClient.GetVocabulary().Count();

            //goalsProgressModel.UserInformation = _waniKaniClient.GetUserInformation();
            //goalsProgressModel.LevelProgression = _waniKaniClient.GetLevelProgression();
            //goalsProgressModel.SrsDistribution = _waniKaniClient.GetSrsDistribution();
            //goalsProgressModel.Radicals = _waniKaniClient.GetRadicals()
            //    .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0);
            //goalsProgressModel.Kanji = _waniKaniClient.GetKanji()
            //    .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0);
            //goalsProgressModel.Vocabulary = _waniKaniClient.GetVocabulary()
            //    .FindAll(r => r.UserInfo != null && r.UserInfo.SrsLevel > 0);

            //goalsProgressModel.TotalKanji = _waniKaniClient.GetKanji().Count;
            //goalsProgressModel.TotalVocab = _waniKaniClient.GetVocabulary().Count;



            //initial load vs deltas
            //if (!_context.WeightSyncs.Any())
            //    await GetInitialFitBitWeightData(new DateTime(2019, 1, 1), DateTime.Now.AddMonths(2));
            //else
            //    await GetInitialFitBitWeightData(_context.WeightSyncs.Max(s => s.SyncDate), DateTime.Now.AddDays(7));


            goalsProgressModel.SimpleWeights = _context.SimpleWeightLogs.OrderBy(l => l.DayOfYear).ToList();

            return View(goalsProgressModel);
        }

        //fix this method or passed paramters. updated weights are returning all weights in the past week from the startdate
        public async Task GetInitialFitBitWeightData(DateTime startDate, DateTime endDate)
        {
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
            catch (Exception e)
            {
                foreach (var l in simpleWeights)
                {
                    await _context.SimpleWeightLogs.AddAsync(l);
                }
            }
            catch
            {
                
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