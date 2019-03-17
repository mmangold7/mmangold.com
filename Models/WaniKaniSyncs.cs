using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mmangold.com.Models
{
    public class WaniKaniSync
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WaniKaniSyncId { get; set; }

        public DateTime SyncDate { get; set; }
    }
}