using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestCinema.Models
{
    public class MovieModel
    {   
        public int MovieId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is Required")]
        [Display(Name = "Movie Name: ")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is Required")]
        [Display(Name = "Description: ")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Duration is Required")]
        [Display(Name = "Duration: ")]
        public string Duration { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Release year is Required")]
        [Display(Name = "Year: ")]
        public string Year { get; set; }

        
        [Display(Name="Upload File:")]
        public string ImagePath { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Rating is Required")]
        [Display(Name = "Rating: ")]
        public Nullable<float> Rating { get; set; }
    }
}