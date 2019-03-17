using System;
using System.ComponentModel.DataAnnotations;

namespace mmangold.com.Models
{
    public class GuruOrGreaterRadical
    {
        [Key]
        public string ImageUri { get; set; }

        public DateTime UnlockedDate { get; set; }
    }
}