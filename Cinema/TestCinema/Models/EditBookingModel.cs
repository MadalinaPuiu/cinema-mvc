using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestCinema.Models
{
    public class EditBookingModel
    {

        public int BookingId { get; set; }
        [Display(Name = "Change Program Id to: ")]
        public int ProgramId { get; set; }
        [Display(Name = "User Id: ")]
        public int UserId { get; set; }
        [Display(Name = "Program Id: ")]
        public int ProgramFirstId { get; set; }

    }
}