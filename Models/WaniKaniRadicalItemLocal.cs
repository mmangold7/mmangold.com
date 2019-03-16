using System.Collections.Generic;
using WaniKaniApi.Models;

namespace mmangold.com.Models
{
    public class WaniKaniRadicalItemLocal : WaniKaniItemLocal
    {
        public string ImageUri { get; set; }

        public static implicit operator WaniKaniRadicalItemLocal(WaniKaniRadicalItem item)
        {
            var meanings = new List<StringRecord>();
            foreach (var m in item.Meanings) meanings.Add(m);
            return new WaniKaniRadicalItemLocal
            {
                ImageUri = item.ImageUri,

                Character = item.Character,
                Level = item.Level,
                Meanings = meanings,
                UserInfo = item.UserInfo
            };
        }
    }
}