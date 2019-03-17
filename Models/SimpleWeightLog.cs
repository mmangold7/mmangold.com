using System;
using System.ComponentModel.DataAnnotations;

namespace mmangold.com.Models
{
    public class SimpleWeightLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float Weight { get; set; }
        public int DayOfYear { get; set; }
    }
}