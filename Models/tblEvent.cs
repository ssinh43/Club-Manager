using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clubmanager.Models
{
    [Table("tblEvent")]
    public class tblEvent
    {
        [Key]
        public int EventID { get; set; }

        public string? EventName { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Location { get; set; }

        public int? MaxParticipants { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}
