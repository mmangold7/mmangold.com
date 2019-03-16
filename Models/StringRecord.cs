using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace mmangold.com.Models
{
    public class StringRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int WaniKaniItemLocalId { get; set; }

        public string StringValue { get; set; }

        public static implicit operator StringRecord(string s)
        {
            return new StringRecord()
            {
                StringValue = s
            };
        }
    }
}
