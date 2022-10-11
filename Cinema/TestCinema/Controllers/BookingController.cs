using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TestCinema.DBModels;
using TestCinema.Models;

namespace TestCinema.Controllers
{
    public class BookingController : Controller
    {
        // GET: Booking

        CinemaDBEntities dbUsers = new CinemaDBEntities();
        CinemaDBEntities1 dbMovies = new CinemaDBEntities1();
        CinemaDBEntities3 dbBooking = new CinemaDBEntities3();
        public ActionResult Index()
        {
            if ((string)Session["Email"] != "admin@gmail.com" && Session["Email"] != null)
            {
                return View(dbMovies.Movies.ToList());
            }
            else { return RedirectToAction("Login", "Account"); }
        }

        public string Success()
        {
            return "All fine";
        }


        [HttpGet]
        public ActionResult Program(int? id)
        {
            if ((string)Session["Email"] != "admin@gmail.com" && Session["Email"] != null)
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                
                

               // if (movie == null)
               // return HttpNotFound();
                return View(dbBooking.Programs.Where(x => x.MovieId == id && x.Available=="yes").ToList());
            }

            else
            {
                return RedirectToAction("Error", "Home");

            }
        }


        [HttpGet]
        public ActionResult Book(int? id)
        {
            if ((string)Session["Email"] != "admin@gmail.com" && Session["Email"] != null)
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Program p = dbBooking.Programs.Where(x => x.ProgramId == id).FirstOrDefault();
                p.Available = "no";
                Booking bookingMovie = new Booking();
                bookingMovie.ProgramId = (int)id;
                string email = (string)Session["Email"];
                User user = new User();
                user = dbUsers.Users.Where(u => u.Email.Equals(email)).First();

                bookingMovie.UserId = user.UserId;

                
                // if (movie == null)
                // return HttpNotFound();
                return Book(bookingMovie);
            }

            else
            {
                return RedirectToAction("Error", "Home");

            }
        }


        [HttpPost]
        public ActionResult Book(Booking b)
        {
            if ((string)Session["Email"] != "admin@gmail.com" && Session["Email"] != null)
            {
                if (b == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

               

                dbBooking.Bookings.Add(b);
                dbBooking.SaveChanges();

                
                // if (movie == null)
                // return HttpNotFound();
                return RedirectToAction("Index", "Booking");
            }

            else
            {
                return RedirectToAction("Error", "Home");

            }
        }



        [HttpGet]
        public ActionResult UserReservations()
        {
            if ((string)Session["Email"] != "admin@gmail.com" && Session["Email"] != null)
            {
                string email = (string)Session["Email"];
                User user = new User();
                user = dbUsers.Users.Where(u => u.Email.Equals(email)).First();

                List<Booking> b = dbBooking.Bookings.Where(x => x.UserId == user.UserId).ToList();
                List<ReservationModel> rm = new List<ReservationModel>();

                foreach (Booking item in b) {
                    ReservationModel tmp = new ReservationModel();

                    Program p = dbBooking.Programs.Where(x => x.ProgramId == item.ProgramId).First();
                    Movy m=dbMovies.Movies.Find(p.MovieId);
                    tmp.MovieId = p.MovieId;
                    tmp.MovieName = m.Name;
                    tmp.SeatNo = p.SeatNo;
                    tmp.Year = p.Year;
                    tmp.Month = p.Month;
                    tmp.Day = p.Day;
                    tmp.Hour = p.Hour;
                    tmp.Auditorium = p.Auditorium;
                    tmp.Price = p.Price;
                    rm.Add(tmp);
                }


                // if (movie == null)
                // return HttpNotFound();
                return View(rm);
            }

            else
            {
                return RedirectToAction("Error", "Home");

            }
        }


    }
}