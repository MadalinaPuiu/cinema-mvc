using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestCinema.DBModels;
using System.IO;
using System.Text;
using System.Data;
using System.Security.Cryptography;

namespace TestCinema.Controllers
{
    public class HomeController : Controller
    {

        CinemaDBEntities1 dbMovies = new CinemaDBEntities1();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }



        public ActionResult AllMovies(string sortBy, string orderBy, string SearchName = "")
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
                    if (sortBy == "Name")
                    {
                        if (orderBy == "Ascending")
                        {
                            return View(dbMovies.Movies.Where(b => b.Name.Contains(SearchName)).OrderBy(i => i.Name).ToList());
                        }
                        else
                        {
                            return View(dbMovies.Movies.Where(b => b.Name.Contains(SearchName)).OrderByDescending(i => i.Name).ToList());
                        }
                    }
                    else if (sortBy == "Rating")
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

        public string Error()
        {
            return "No Permission";
        }

        public string Encrypt()
        {
            String a = "ad1";
            String b = Encrypt(a);
            return b+ "length="+b.Length;
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