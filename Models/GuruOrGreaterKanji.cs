using System;
using System.ComponentModel.DataAnnotations;

namespace mmangold.com.Models
{
    public class GuruOrGreaterKanji
    {
        [Key]
        public string Character { get; set; }

        public DateTime UnlockedDate { get; set; }
    }
}