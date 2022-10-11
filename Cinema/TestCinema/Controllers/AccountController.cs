using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TestCinema.DBModels;
using TestCinema.Models;
using System.IO;
using System.Text;
using System.Data;
using System.Security.Cryptography;

namespace TestCinema.Controllers
{

    public class AccountController : Controller
    {
        CinemaDBEntities objUserDBEntities = new CinemaDBEntities(); 
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public string En(string s)
        {
            
            return s + "length=" + s.Length;
        }

        public ActionResult Register()
        {
            UserModel objUserModel = new UserModel();
            return View(objUserModel);
        }

        [HttpPost]
        public ActionResult Register(UserModel objUserModel)
        {
            if (ModelState.IsValid)
            {
                if (!objUserDBEntities.Users.Any(m => m.Email == objUserModel.Email))
                {
                    User objUser = new DBModels.User();
                    objUser.CreatedOn = DateTime.Now;
                    objUser.Email = objUserModel.Email;
                    objUser.FirstName = objUserModel.FirstName;
                    objUser.LastName = objUserModel.LastName;
                    objUser.Password = Encrypt(objUserModel.Password);

                    objUserDBEntities.Users.Add(objUser);
                    objUserDBEntities.SaveChanges();
                    objUserModel = new UserModel();
                    objUserModel.SuccessMessage = "User Successfully added";
                    return RedirectToAction("Index", "Home");
                }
                else {
                    ModelState.AddModelError("Error","Email already exists");
                    return View();
                }
            }
            return View();
        }

        public ActionResult Login()
        {
            LoginModel objLoginModel = new LoginModel();
            return View(objLoginModel);
        }

        [HttpPost]
        public ActionResult Login(LoginModel objLoginModel)
        {
            try {
           // User u = new User();
            //u = objUserDBEntities.Users.Where(m => m.Email == objLoginModel.Email).FirstOrDefault();

            //string pas1= u.Password;
            
                string pas = Encrypt(objLoginModel.Password);
                if (ModelState.IsValid)
                {
                    if (objUserDBEntities.Users.Where(m => m.Email == objLoginModel.Email && m.Password.Equals(pas)).FirstOrDefault() == null)
                    {

                        ModelState.AddModelError("Error", "Invalid Email or Password");
                        return View();
                    }
                    else
                    {

                        Session["Email"] = objLoginModel.Email;
                        if (objLoginModel.Email != "admin@gmail.com")
                            return RedirectToAction("Index", "Home");
                        else return RedirectToAction("Index", "Admin");
                    }
                }
                }
                catch {

                ModelState.AddModelError("Error", "Invalid Email or Password");
                return View();

            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
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