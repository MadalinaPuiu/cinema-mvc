using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TestCinema.DBModels;
using TestCinema.Models;
using System.Text;
using System.Data;
using System.Security.Cryptography;

namespace TestCinema.Controllers
{
    public class AdminController : Controller
    {
      
        CinemaDBEntities dbUsers = new CinemaDBEntities();//users
        CinemaDBEntities1 dbMovies = new CinemaDBEntities1();//movies
        CinemaDBEntities3 dbBoking = new CinemaDBEntities3();//booking

        public ActionResult Index()
        {
            if ((string)Session["Email"] == "admin@gmail.com") { return View(); }

            else { return RedirectToAction("Error", "Home"); }
        }

        public ActionResult About()
        {

            if ((string)Session["Email"] == "admin@gmail.com") { return View(); }

            else { return RedirectToAction("Error", "Home"); }
        }

        public ActionResult Contact()
        {
            if ((string)Session["Email"] == "admin@gmail.com") { return View(); }

            else { return RedirectToAction("Error", "Home"); }
        }

        public ActionResult AllMovies(string sortBy, string orderBy, string SearchName = "")
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {

                if (SearchName == "")
                {
                    if (sortBy == "Year")
                    {
                        if (orderBy == "Ascending")
                        {
                            return View(dbMovies.Movies.OrderBy(i => i.Year).ToList());
                        }
                        else
                        {
                            return View(dbMovies.Movies.OrderByDescending(i => i.Year).ToList());
                        }

                    }
                    else if (sortBy == "Rating")
                    {
                        if (orderBy == "Ascending")
                        {
                            return View(dbMovies.Movies.OrderBy(i => i.Rating).ToList());
                        }
                        else
                        {
                            return View(dbMovies.Movies.OrderByDescending(i => i.Rating).ToList());
                        }
                    }

                    else
                    {
                        if (orderBy == "Ascending")
                        {
                            return View(dbMovies.Movies.OrderBy(i => i.Name).ToList());
                        }
                        else
                        {
                            return View(dbMovies.Movies.OrderByDescending(i => i.Name).ToList());
                        }
                    }
                }
                else
                {
                    if (sortBy == "Name") {
                        if (orderBy == "Ascending")
                        {
                            return View(dbMovies.Movies.Where(b => b.Name.Contains(SearchName)).OrderBy(i => i.Name).ToList());
                        }
                        else
                        {
                            return View(dbMovies.Movies.Where(b => b.Name.Contains(SearchName)).OrderByDescending(i => i.Name).ToList());
                        }
                    }else if (sortBy == "Rating")
                    {
                        if (orderBy == "Ascending")
                        {
                            return View(dbMovies.Movies.Where(b => b.Name.Contains(SearchName)).OrderBy(i => i.Rating).ToList());
                        }
                        else
                        {
                            return View(dbMovies.Movies.Where(b => b.Name.Contains(SearchName)).OrderByDescending(i => i.Rating).ToList());
                        }
                    }
                    else
                    {
                        if (orderBy == "Ascending")
                        {
                            return View(dbMovies.Movies.Where(b => b.Name.Contains(SearchName)).OrderBy(i => i.Year).ToList());
                        }
                        else
                        {
                            return View(dbMovies.Movies.Where(b => b.Name.Contains(SearchName)).OrderByDescending(i => i.Year).ToList());
                        }
                    }
                    
                }


            }

            else { return RedirectToAction("Error", "Admin"); }
        }

        public string Error()
        {
            if ((string)Session["Email"] == "admin@gmail.com") { return "smth went wrong"; }

            else { return "STOP! LOG IN AS ADMIN FIRST!"; }
            
        }

        public ActionResult Users()
        {
            if ((string)Session["Email"] == "admin@gmail.com") { return View(dbUsers.Users.ToList()); }

            else { return RedirectToAction("Error", "Home"); }
            
        }

        public ActionResult Movies()
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            { return View(dbMovies.Movies.ToList()); }

            else { return RedirectToAction("Error", "Home"); }
            
        }

        public ActionResult Programs()
        {
            if ((string)Session["Email"] == "admin@gmail.com") { return View(dbBoking.Programs.OrderBy(i => i.MovieId).ToList()); }

            else { return RedirectToAction("Error", "Home"); }

        }

        public ActionResult Bookings()
        {
            if ((string)Session["Email"] == "admin@gmail.com") { return View(dbBoking.Bookings.ToList()); }

            else { return RedirectToAction("Error", "Home"); }

        }

        //CRUD FOR USERS---------------------------------------------------------------------
        [HttpGet]
        public ActionResult CreateUser()
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                UserModel objUserModel = new UserModel();
                return View(objUserModel);
            }

            else { return RedirectToAction("Error", "Home"); }

        }

        [HttpPost]
        public ActionResult CreateUser(UserModel objUserModel)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                if (ModelState.IsValid)
                {

                    if (!dbUsers.Users.Any(m => m.Email == objUserModel.Email))
                    {
                        User objUser = new User();
                        objUser.CreatedOn = DateTime.Now;
                        objUser.Email = objUserModel.Email;
                        objUser.FirstName = objUserModel.FirstName;
                        objUser.LastName = objUserModel.LastName;
                        objUser.Password = Encrypt(objUserModel.Password);

                        dbUsers.Users.Add(objUser);
                        dbUsers.SaveChanges();
                        objUserModel = new UserModel();
                        objUserModel.SuccessMessage = "User Successfully added";
                        return RedirectToAction("Users", "Admin");
                    }
                    else
                    {

                        ModelState.AddModelError("Error", "Email already exists");
                        return View();
                    }
                }
                return View();
            }

            else { return RedirectToAction("Error", "Home"); }


        }

        [HttpGet]
        public ActionResult EditUser(int? id)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                User user = dbUsers.Users.Find(id);

                if (user == null)
                    return HttpNotFound();
                return View(user);
            }

            else { return RedirectToAction("Error", "Home"); }


        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        dbUsers.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        dbUsers.SaveChanges();
                        return RedirectToAction("Users");
                    }

                    return View(user);

                }
                catch
                {
                    return View();
                }
            }

            else { return RedirectToAction("Error", "Home"); }



        }

        [HttpGet]
        public ActionResult DeleteUser(int? id)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                User user = dbUsers.Users.Find(id);
                if (user == null)
                    return HttpNotFound();
                return View(user);
            }

            else { return RedirectToAction("Error", "Home"); }


        }

        [HttpPost]
        public ActionResult DeleteUser(int? id, User bo)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                try
                {
                    User user = new User();
                    if (ModelState.IsValid)
                    {
                        if (id == null)
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        user = dbUsers.Users.Find(id);
                        if (user == null)
                            return HttpNotFound();
                        dbUsers.Users.Remove(user);
                        dbUsers.SaveChanges();
                        return RedirectToAction("Users");
                    }

                    return View(user);

                }
                catch
                {
                    return View();
                }
            }

            else { return RedirectToAction("Error", "Home"); }


        }

        
        public ActionResult DetailsUser(int? id)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                User user = dbUsers.Users.Find(id);
                if (user == null)
                    return HttpNotFound();
                return View(user);
            }

            else { return RedirectToAction("Error", "Home"); }


        }

        //CRUD FOR MOVIES---------------------------------------------------------------------
  
        [HttpGet]
        public ActionResult CreateMovie()
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                MovieModel objUserModel = new MovieModel();
                return View(objUserModel);
            }

            else { return RedirectToAction("Error", "Home"); }

        }

        [HttpPost]
        public ActionResult CreateMovie(MovieModel objMovieModel)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                if (ModelState.IsValid)
                {

                    if (!dbMovies.Movies.Any(m => m.Name == objMovieModel.Name && m.Year == objMovieModel.Year))
                    {
                        Movy objMovie = new DBModels.Movy();
                        //objMovie.CreatedOn = DateTime.Now;

                        string fileName, extention;
                        fileName = Path.GetFileNameWithoutExtension(objMovieModel.ImageFile.FileName);
                        extention = Path.GetExtension(objMovieModel.ImageFile.FileName);
                        fileName = objMovieModel.Name + DateTime.Now.ToString("yymmssff") + extention;//to avoid duplicate
                        objMovieModel.ImagePath = "~/Images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        objMovieModel.ImageFile.SaveAs(fileName);

                        objMovie.Name = objMovieModel.Name;
                        objMovie.Description = objMovieModel.Description;
                        objMovie.Duration = objMovieModel.Duration;
                        objMovie.Year = objMovieModel.Year;
                        objMovie.Rating = objMovieModel.Rating;
                        objMovie.ImagePath = objMovieModel.ImagePath;

                        dbMovies.Movies.Add(objMovie);
                        dbMovies.SaveChanges();
                        objMovieModel = new MovieModel();

                        return RedirectToAction("Movies", "Admin");
                    }
                    else
                    {

                        ModelState.AddModelError("Error", "Email already exists");
                        return View();
                    }
                }
                return View();
                //return RedirectToAction("Error", "Admin");
            }

            else
            {
                return RedirectToAction("Error", "Home");
            }


        }

        [HttpGet]
        public ActionResult EditMovie(int? id)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Movy movie = dbMovies.Movies.Find(id);

                if (movie == null)
                    return HttpNotFound();
                return View(movie);
            }

            else { return RedirectToAction("Error", "Home"); }


        }

        [HttpPost]
        public ActionResult EditMovie(Movy movie)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                try
                {
                    if (ModelState.IsValid)
                    {

                        string fileName, extention;
                        fileName = Path.GetFileNameWithoutExtension(movie.ImageFile.FileName);
                        extention = Path.GetExtension(movie.ImageFile.FileName);
                        fileName = movie.Name + DateTime.Now.ToString("yymmssff") + extention;//to avoid duplicate
                        movie.ImagePath = "~/Images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        movie.ImageFile.SaveAs(fileName);


                        dbMovies.Entry(movie).State = System.Data.Entity.EntityState.Modified;
                        dbMovies.SaveChanges();
                        return RedirectToAction("Movies");
                    }

                    return View(movie);

                }
                catch
                {
                    return View();
                }
            }

            else { return RedirectToAction("Error", "Home"); }

        }

        [HttpGet]
        public ActionResult DeleteMovie(int? id)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Movy movie = dbMovies.Movies.Find(id);
                if (movie == null)
                    return HttpNotFound();
                return View(movie);
            }

            else { return RedirectToAction("Error", "Home"); }


        }

        [HttpPost]
        public ActionResult DeleteMovie(int? id, Movy bo)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                try
                {
                    Movy movie = new Movy();
                    if (ModelState.IsValid)
                    {
                        if (id == null)
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        movie = dbMovies.Movies.Find(id);
                        if (movie == null)
                            return HttpNotFound();
                        dbMovies.Movies.Remove(movie);
                        dbMovies.SaveChanges();
                        return RedirectToAction("Movies");
                    }

                    return View(movie);

                }
                catch
                {
                    return View();
                }
            }

            else { return RedirectToAction("Error", "Home"); }


        }

        
        public ActionResult DetailsMovie(int? id)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {

                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Movy movie = dbMovies.Movies.Find(id);
                if (movie == null)
                    return HttpNotFound();
                return View(movie);
            }

            else { return RedirectToAction("Error", "Home"); }

        }
    


        //CRUD FOR Schedule Model---------------------------------------------------------------------

        [HttpGet]
        public ActionResult CreateProgram()
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                Program p = new Program();
                return View(p);
            }

            else { return RedirectToAction("Error", "Home"); }

        }

        [HttpPost]
        public ActionResult CreateProgram(Program p)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                if (ModelState.IsValid)
                {

                    if (!dbBoking.Programs.Any(m => m.MovieId == p.MovieId && m.Year == p.Year && m.Month == p.Month && m.Day == p.Day && m.Hour == p.Hour && m.SeatNo == p.SeatNo && m.Auditorium == p.Auditorium))
                    {

                        if (dbMovies.Movies.Any(m => m.MovieId == p.MovieId))
                        {
                            p.Available = "yes";
                            //objMovie.CreatedOn = DateTime.Now;

                            dbBoking.Programs.Add(p);
                            dbBoking.SaveChanges();
                            p = new Program();

                            return RedirectToAction("Programs", "Admin");
                        }
                        else
                        {

                            ModelState.AddModelError("Error", "Movie does not exist");
                            return View();
                        }
                    }
                    else
                    {

                        ModelState.AddModelError("Error", "Program already exists");
                        return View();
                    }
                }
                return View();
                //return RedirectToAction("Error", "Admin");
            }

            else
            {
                return RedirectToAction("Error", "Home");
            }


        }

        [HttpGet]
        public ActionResult EditProgram(int? id)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Program p = dbBoking.Programs.Find(id);

                if (p == null)
                    return HttpNotFound();
                return View(p);
            }

            else { return RedirectToAction("Error", "Home"); }


        }

        [HttpPost]
        public ActionResult EditProgram(Program p)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (!dbBoking.Programs.Any(m => m.MovieId == p.MovieId && m.Year == p.Year && m.Month == p.Month && m.Day == p.Day && m.Hour == p.Hour && m.SeatNo == p.SeatNo && m.Auditorium == p.Auditorium && m.Available==p.Available && m.Price==p.Price))
                        {

                            if (dbMovies.Movies.Any(m => m.MovieId == p.MovieId))
                            {

                                dbBoking.Entry(p).State = System.Data.Entity.EntityState.Modified;
                                dbBoking.SaveChanges();
                                return RedirectToAction("Programs");
                            }
                            else
                            {

                                ModelState.AddModelError("Error", "Movie does not exist");
                                return View();
                            }
                        }
                        else
                        {

                            ModelState.AddModelError("Error", "Program already exists");
                            return View();
                        }
                    }

                    return View(p);

                }
                catch
                {
                    return View();
                }
            }

            else { return RedirectToAction("Error", "Home"); }

        }

        [HttpGet]
        public ActionResult DeleteProgram(int? id)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    Program p = dbBoking.Programs.Find(id);

                    if (p == null)
                        return HttpNotFound();
                    return View(p);
                }

            else { return RedirectToAction("Error", "Home"); }


        }

        [HttpPost]
        public ActionResult DeleteProgram(int? id, Program bo)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {
                try
                {
                    Program p = new Program();
                    if (ModelState.IsValid)
                    {
                        if (id == null)
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        p = dbBoking.Programs.Find(id);
                        if (p == null)
                            return HttpNotFound();
                        dbBoking.Programs.Remove(p);
                        dbBoking.SaveChanges();
                        return RedirectToAction("Programs");
                    }

                    return View(p);

                }
                catch
                {
                    return View();
                }
            }

            else { return RedirectToAction("Error", "Home"); }


        }


        public ActionResult DetailsProgram(int? id)
        {
            if ((string)Session["Email"] == "admin@gmail.com")
            {

                if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    Program p = dbBoking.Programs.Find(id);

                    if (p == null)
                        return HttpNotFound();
                    return View(p);
                }

            else { return RedirectToAction("Error", "Home"); }

        }


        //CRUD FOR Booking Model---------------------------------------------------------------------

        [HttpGet]
        public ActionResult CreateBooking()
            {
                if ((string)Session["Email"] == "admin@gmail.com")
                {
                    Booking p = new Booking();
                    return View(p);
                }

                else { return RedirectToAction("Error", "Home"); }

            }

        [HttpPost]
        public ActionResult CreateBooking(Booking p)
            {
                if ((string)Session["Email"] == "admin@gmail.com")
                {
                    if (ModelState.IsValid)
                    {

                        if (!dbBoking.Bookings.Any(m => m.ProgramId == p.ProgramId && m.UserId == p.UserId))
                        {


                        if (dbBoking.Programs.Any(m => m.ProgramId == p.ProgramId) && dbUsers.Users.Any(m => m.UserId == p.UserId))
                        {

                            Program p1 = dbBoking.Programs.Where(x => x.ProgramId == p.ProgramId).FirstOrDefault();
                            p1.Available = "no";
                            dbBoking.Bookings.Add(p);
                            dbBoking.SaveChanges();
                            p = new Booking();

                            return RedirectToAction("Bookings", "Admin");
                        }else
                        {
                            ModelState.AddModelError("Error", "User or schedule does not exist ");
                            return View();
                        }
                        }
                        else
                        {

                            ModelState.AddModelError("Error", "Schedule already exists");
                            return View();
                        }
                    }
                    return View();
                    //return RedirectToAction("Error", "Admin");
                }

                else
                {
                    return RedirectToAction("Error", "Home");
                }


            }

        [HttpGet]
        public ActionResult EditBooking(int? id)
            {
                if ((string)Session["Email"] == "admin@gmail.com")
                {
                    if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    Booking p = dbBoking.Bookings.Find(id);

                    EditBookingModel ebm = new EditBookingModel();
                    ebm.BookingId = p.BookingId;
                    ebm.ProgramFirstId = p.ProgramId;
                    ebm.ProgramId= p.ProgramId;
                    ebm.UserId = p.UserId;

               
                    


                if (p == null)
                            return HttpNotFound();
                        return View(ebm);
                }

                else { return RedirectToAction("Error", "Home"); }


            }

        [HttpPost]
        public ActionResult EditBooking(EditBookingModel p)
            {
                if ((string)Session["Email"] == "admin@gmail.com")
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {


                    if (!dbBoking.Bookings.Any(m => m.ProgramId == p.ProgramId && m.UserId == p.UserId))
                    {


                        if (dbBoking.Programs.Any(m => m.ProgramId == p.ProgramId) && dbUsers.Users.Any(m => m.UserId == p.UserId))
                        {

                            Booking b = new Booking();
                            b.BookingId = p.BookingId;
                            b.ProgramId = p.ProgramId;
                            b.UserId = p.UserId;

                            Program p1 = dbBoking.Programs.Find(p.ProgramFirstId);
                            p1.Available = "yes";


                            Program p2 = dbBoking.Programs.Find(p.ProgramId);
                            p2.Available = "no";

                            dbBoking.Entry(b).State = System.Data.Entity.EntityState.Modified;

                            dbBoking.SaveChanges();

                            return RedirectToAction("Bookings");
                            }else
                        {
                            ModelState.AddModelError("Error", "User or schedule does not exist ");
                            return View();
                        }
                        }
                        else
                        {

                            ModelState.AddModelError("Error", "Schedule already exists");
                            return View();
                        }
                        }

                        return View(p);

                    }
                    catch
                    {
                        return View();
                    }
                }

                else { return RedirectToAction("Error", "Home"); }

            }

        [HttpGet]
        public ActionResult DeleteBooking(int? id)
            {
                if ((string)Session["Email"] == "admin@gmail.com")
                {
                    if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    Booking p = dbBoking.Bookings.Find(id);

                    if (p == null)
                        return HttpNotFound();
                    return View(p);
                }

                else { return RedirectToAction("Error", "Home"); }


            }

        [HttpPost]
        public ActionResult DeleteBooking(int? id, Booking bo)
            {
                if ((string)Session["Email"] == "admin@gmail.com")
                {
                    try
                    {
                        Booking p = new Booking();
                        if (ModelState.IsValid)
                        {
                            if (id == null)
                                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                            p = dbBoking.Bookings.Find(id);
                            if (p == null)
                                return HttpNotFound();

                        Program p1 = dbBoking.Programs.Find(p.ProgramId);
                        p1.Available = "yes";


                        dbBoking.Bookings.Remove(p);
                            dbBoking.SaveChanges();
                            return RedirectToAction("Bookings");
                        }

                        return View(p);

                    }
                    catch
                    {
                        return View();
                    }
                }

                else { return RedirectToAction("Error", "Home"); }


            }

        [HttpGet]
        public ActionResult DetailsBooking(int? id)
            {
                if ((string)Session["Email"] == "admin@gmail.com")
                {

                    if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    Booking p = dbBoking.Bookings.Find(id);

                    if (p == null)
                        return HttpNotFound();
                    return View(p);
                }

                else { return RedirectToAction("Error", "Home"); }

            }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }

}



