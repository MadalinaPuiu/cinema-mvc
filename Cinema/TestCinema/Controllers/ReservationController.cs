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
    public class ReservationController : Controller
    {
        // GET: Reservation
        CinemaDBEntities dbUsers = new CinemaDBEntities();
        CinemaDBEntities1 dbMovies = new CinemaDBEntities1();
        //CinemaDBEntities2 dbReservations = new CinemaDBEntities2();
        public ActionResult Index()
        {
            if ((string)Session["Email"] != "admin@gmail.com" && Session["Email"]!=null)
            {
                return View(dbMovies.Movies.ToList());
            } 
            else { return RedirectToAction("Login", "Account"); }
        }

     /*   [HttpGet]
        public ActionResult Book(int? id)
        { 
            if ((string)Session["Email"] != "admin@gmail.com" && Session["Email"] != null)
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                ReservationModel reservation = new ReservationModel();
                MovieSchedule ms = new MovieSchedule();
                Movy movie = dbMovies.Movies.Find(id);
                string email = (string)Session["Email"];
                User user = new User();

                ms = dbReservations.MovieSchedules.Where(u => u.MovieId == movie.MovieId).FirstOrDefault();
                user = dbUsers.Users.Where(u=> u.Email.Equals(email)).First();
                //TempData["MaxSeatNr"] = ms.MaxSeatNr;
                reservation.UserId = user.UserId;
                reservation.MovieId = movie.MovieId;
                reservation.Email = user.Email;
                reservation.MovieName = movie.Name;
                reservation.Price = ms.Price;
                reservation.MaxSeatNr = (int)ms.MaxSeatNr;
                List<MovieProgram> lis = new List<MovieProgram>();
                lis= dbReservations.MoviePrograms.Where(u => u.MovieId == movie.MovieId).ToList();
                reservation.Program = lis;

                if (movie == null)
                    return HttpNotFound();
                return View(reservation);
            }

            else { return RedirectToAction("Error", "Home");
                
            }
        }


        [HttpPost]
        public ActionResult Book(ReservationModel model)
        {
            if ((string)Session["Email"] != "admin@gmail.com" && Session["Email"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (!dbReservations.Reservations.Any(m => m.UserId == model.UserId && m.MovieId == model.MovieId && m.ReservationDate == model.ReservationDate && m.StartHour == model.StartHour && m.SeatNr == model.SeatNr && m.PhoneNr == model.PhoneNr))
                        {
                           // if (dbReservations.MovieSchedules.Any(x =>x.MovieId == model.MovieId))
                           // {

                                //&& DateTime.Compare(x.StartDate, model.ReservationDate) <= 0 && DateTime.Compare(x.EndDate, model.ReservationDate) >= 0
                                //if (dbReservations.MoviePrograms.Any(x => x.MovieId == model.MovieId && x.StartHour == model.StartHour))
                               // {
                                    Reservation reserv = new Reservation();

                                    reserv.UserId = model.UserId;
                                    reserv.PhoneNr = model.PhoneNr;
                                    reserv.MovieId = model.MovieId;
                                    reserv.ReservationDate = model.ReservationDate;
                                    reserv.StartHour = model.StartHour;
                                    reserv.SeatNr = model.SeatNr;


                                    dbReservations.Reservations.Add(reserv);
                                    dbReservations.SaveChanges();


                                    model = new ReservationModel();

                                    return RedirectToAction("Index");
                               // }
                               // else
                                //{
                                 //   ModelState.AddModelError("Error", "Sorry, the hour is not valid...");
                                 //   return View(model);
                               // }
                            //}
                           // else
                           // {
                           //     ModelState.AddModelError("Error", "Sorry, the date is not valid...");
                           //     return View(model);
                           // }
                        }
                        else
                        {
                            ModelState.AddModelError("Error", "Sorry, this ticket was booked...");
                            return View(model);
                        }



                    }
                    return View(model);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("Error", "errrrrrrrrrr..."+ex);
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }

        }*/


    }
}