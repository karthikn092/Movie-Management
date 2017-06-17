using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieManagement.Models
{
    public class Movie
    {
        public Movie()
        {
            Actors = new List<Actor>();
            Producer = new Producer();
            ProducerCollection = new List<object>();
            ActorsCollection = new List<CheckBoxModel>();
            YearCollection = new List<object>();
        }

        public int MovieId { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Invalid Movie Name")]
        [Display(Name = "Movie Name")]
        public string MovieName { get; set; }

        [Display(Name = "Year Of Release")]
        public string YearOfRelease { get; set; }

        public string Plot { get; set; }


        public byte[] Poster { get; set; }

        public virtual ICollection<Actor> Actors { get; set; }

        public virtual Producer Producer { get; set; }

        [NotMapped]
        [Display(Name ="Producer")]
        public string ProducerString { get; set; }

        [NotMapped]
        public List<object> ProducerCollection { get; set; }
        [NotMapped]
        public List<CheckBoxModel> ActorsCollection { get; set; }
        [NotMapped]
        public List<object> YearCollection { get; set; }
    }
}