using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clubmanager.Models
{
    [Table("tblEventRegistration")]
    public class tblEventRegistration
    {
        [Key]
        public int RegistrationID { get; set; }

        public int EventID { get; set; }

        public int UserID { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string? Status { get; set; }
    }
}