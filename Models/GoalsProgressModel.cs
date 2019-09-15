using System.Collections.Generic;
using Fitbit.Models;
using WaniKaniApi.Models;

namespace mmangold.com.Models
{
    public class GoalsProgressModel
    {
        public string Level { get; set; }
        public List<GuruOrGreaterRadical> Radicals { get; set; }
        public List<GuruOrGreaterKanji> Kanji { get; set; }
        public List<GuruOrGreaterVocab> Vocabulary { get; set; }
    }
}