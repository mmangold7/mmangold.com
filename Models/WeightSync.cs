using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmangold.com.Models
{
    public class WeightSync
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeightSyncId { get; set; }

        public DateTime SyncDate { get; set; }

        public int NewRecords { get; set; }
    }
}