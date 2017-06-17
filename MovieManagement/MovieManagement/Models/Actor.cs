using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieManagement.Models
{
    public class Actor
    {
        public Actor()
        {
            Movies = new List<Movie>();
        }

        public int ActorId { get; set; }

        [Required]
        [Display(Name ="Actor Name")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Invalid Actor Name")]
        public string ActorName { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:MMM/dd/yyyy}")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime? Dob { get; set; }

        public string Bio { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}