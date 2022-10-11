using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TestCinema.DBModels;

namespace TestCinema.Models
{
    public class ReservationModel
    {

        public int Id { get; set; }
        
        public int MovieId { get; set; }

        [Display(Name = "Movie Name: ")]
        public string MovieName { get; set; }

        [Display(Name = "Seat Number: ")]
        public int SeatNo { get; set; }

        [Display(Name = "Year: ")]
        public int Year { get; set; }

        [Display(Name = "Month: ")]
        public int Month { get; set; }

        [Display(Name = "Day: ")]
        public int Day { get; set; }

        [Display(Name = "Hour: ")]
        public int Hour { get; set; }
        [Display(Name = "Auditorium: ")]
        public int Auditorium { get; set; }

        [Display(Name = "Price(RON): ")]
        public Nullable<int> Price { get; set; }


    }
}