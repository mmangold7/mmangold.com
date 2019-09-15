using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaniKaniApi.Models;

namespace mmangold.com.Models
{
    public class LevelProgress
    {
        [Key]
        public int Id { get; set; }
        public string Level { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}