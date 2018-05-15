using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities {
    public abstract class BaseEntity {
        [Required]
        [Key]
        public long Id { get; set; }
        public bool Active { get; set; }

        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
