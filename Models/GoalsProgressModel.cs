using System.Collections.Generic;
using Fitbit.Models;
using WaniKaniApi.Models;

namespace mmangold.com.Models
{
    public class GoalsProgressModel
    {
        public WaniKaniUserInformation UserInformation { get; set; }
        public WaniKaniLevelProgression LevelProgression { get; set; }
        public WaniKaniSrsDistribution SrsDistribution { get; set; }
        public List<GuruOrGreaterRadical> Radicals { get; set; }
        public List<GuruOrGreaterKanji> Kanji { get; set; }
        public List<GuruOrGreaterVocab> Vocabulary { get; set; }

        public int TotalKanji { get; set; }
        public int TotalVocab { get; set; }

        public List<WeightLog> AriaWeights { get; set; }
        public List<SimpleWeightLog> SimpleWeights { get; set; }
    }
}