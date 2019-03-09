﻿using System.Collections.Generic;
using Fitbit.Models;
using WaniKaniApi.Models;

namespace mmangold.com.Models
{
    public class GoalsProgressModel
    {
        public WaniKaniUserInformation UserInformation { get; set; }
        public WaniKaniLevelProgression LevelProgression { get; set; }
        public WaniKaniSrsDistribution SrsDistribution { get; set; }
        public List<WaniKaniRadicalItem> Radicals { get; set; }
        public List<WaniKaniKanjiItem> Kanji { get; set; }
        public List<WaniKaniVocabularyItem> Vocabulary { get; set; }

        public int TotalKanji { get; set; }
        public int TotalVocab { get; set; }

        public UserProfile FitBitProfileData { get; set; }
    }
}