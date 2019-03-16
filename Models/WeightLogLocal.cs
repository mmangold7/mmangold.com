using System;
using System.ComponentModel.DataAnnotations;

namespace mmangold.com.Models
{
    public class WeightLogLocal
    {
        [Key]
        public long LogId { get; set; }

        public float Bmi { get; set; }

        public DateTime Date { get; set; }

        public DateTime Time { get; set; }

        public float Weight { get; set; }

        public DateTime DateTime
        {
            get
            {
                var date = Date;
                date = date.Date;
                return date.Add(Time.TimeOfDay);
            }
        }
    }
}