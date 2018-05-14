﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities {
    public abstract class BaseEntity {
        [Required]
        public long Id { get; set; }
        public bool Active { get; set; }

        [Required]
        public long CreatedBy { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        public long? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
