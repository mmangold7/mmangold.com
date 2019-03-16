using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WaniKaniApi.Models;

namespace mmangold.com.Models
{
    public class WaniKaniItemUserInfoLocal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public WaniKaniSrsLevel SrsLevel { get; set; }

        public int SrsStep { get; set; }

        public DateTime UnlockedDate { get; set; }

        public DateTime? ReviewDate { get; set; }

        public bool IsBurned { get; set; }

        public DateTime? BurnedDate { get; set; }

        public int MeaningCorrectAnswers { get; set; }

        public int MeaningIncorrectAnswers { get; set; }

        public int MeaningMaxStreak { get; set; }

        public int MeaningCurrentStreak { get; set; }

        public int? ReadingCorrectAnswers { get; set; }

        public int? ReadingIncorrectAnswers { get; set; }

        public int? ReadingMaxStreak { get; set; }

        public int? ReadingCurrentStreak { get; set; }

        public string MeaningNote { get; set; }

        public List<StringRecord> UserSynonyms { get; set; } = new List<StringRecord>();

        public string ReadingNote { get; set; }

        public static implicit operator WaniKaniItemUserInfoLocal(WaniKaniItemUserInfo i)
        {
            var syns = new List<StringRecord>();
            foreach (var s in i.UserSynonyms)
            {
                syns.Add(s);
            }
            return new WaniKaniItemUserInfoLocal
            {
                SrsLevel = i.SrsLevel,
                BurnedDate = i.BurnedDate,
                IsBurned = i.IsBurned,
                MeaningCorrectAnswers = i.MeaningCorrectAnswers,
                MeaningCurrentStreak = i.MeaningCurrentStreak,
                MeaningIncorrectAnswers = i.MeaningIncorrectAnswers,
                MeaningMaxStreak = i.MeaningMaxStreak,
                MeaningNote = i.MeaningNote,
                ReadingCorrectAnswers = i.ReadingCorrectAnswers,
                ReadingCurrentStreak = i.ReadingCurrentStreak,
                ReadingIncorrectAnswers = i.ReadingIncorrectAnswers,
                ReadingMaxStreak = i.ReadingMaxStreak,
                ReadingNote = i.ReadingNote,
                UnlockedDate = i.UnlockedDate,
                ReviewDate = i.ReviewDate,
                SrsStep = i.SrsStep,
                UserSynonyms = syns
            };
        }
    }
}