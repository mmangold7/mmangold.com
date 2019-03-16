using System.Collections.Generic;
using WaniKaniApi.Models;

namespace mmangold.com.Models
{
    public class WaniKaniVocabularyItemLocal : WaniKaniItemLocal
    {
        public List<StringRecord> KanaReadings { get; set; }

        public static implicit operator WaniKaniVocabularyItemLocal(WaniKaniVocabularyItem i)
        {
            var readings = new List<StringRecord>();
            foreach (var r in i.KanaReadings) readings.Add(r);
            var meanings = new List<StringRecord>();
            foreach (var m in i.Meanings) meanings.Add(m);
            return new WaniKaniVocabularyItemLocal
            {
                KanaReadings = readings,

                UserInfo = i.UserInfo,
                Character = i.Character,
                Level = i.Level,
                Meanings = meanings
            };
        }
    }
}