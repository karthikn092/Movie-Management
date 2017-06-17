using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieManagement.Models
{
    public class Producer
    {
        public int ProducerId { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Invalid Name")]
        [Display(Name ="Producer Name")]
        public string ProducerName { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:MMM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime? Dob { get; set; }

        public string Bio { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}