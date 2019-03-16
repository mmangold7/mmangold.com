using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmangold.com.Models
{
    public abstract class WaniKaniItemLocal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Character { get; set; }

        public List<StringRecord> Meanings { get; set; }

        public int Level { get; set; }

        public WaniKaniItemUserInfoLocal UserInfo { get; set; }
    }
}