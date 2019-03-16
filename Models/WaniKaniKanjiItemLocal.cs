using System.Collections.Generic;
using WaniKaniApi.Models;

namespace mmangold.com.Models
{
    public class WaniKaniKanjiItemLocal : WaniKaniItemLocal
    {
        public List<StringRecord> OnYomi { get; set; }

        public List<StringRecord> KunYomi { get; set; }

        public List<StringRecord> Nanori { get; set; }

        public WaniKaniReadingType ImportantReading { get; set; }

        public static implicit operator WaniKaniKanjiItemLocal(WaniKaniKanjiItem i)
        {
            var kunyomi = new List<StringRecord>();
            foreach (var k in i.KunYomi) kunyomi.Add(k);
            var onyomi = new List<StringRecord>();
            foreach (var o in i.OnYomi) onyomi.Add(o);
            var nanori = new List<StringRecord>();
            foreach (var n in i.Nanori) nanori.Add(n);
            var meanings = new List<StringRecord>();
            foreach (var m in i.Meanings) meanings.Add(m);
            return new WaniKaniKanjiItemLocal
            {
                ImportantReading = i.ImportantReading,
                KunYomi = kunyomi,
                Nanori = onyomi,
                OnYomi = nanori,
                UserInfo = i.UserInfo,
                Character = i.Character,
                Level = i.Level,
                Meanings = meanings
            };
        }
    }
}