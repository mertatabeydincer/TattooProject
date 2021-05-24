using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TattooProject.Models;
using TattooProject.ViewModels;

namespace TattooProject.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult UserSignup(int ID = 0)
        {
            var userModel = new UserModel();
            return View(userModel);
        }
        [HttpPost]
        public ActionResult UserSignup(User userModel)
        {
            //byte[] salt;
            //new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            if (ModelState.IsValid)
            {
                //HASHING THE PASSWORD
                //var pbkdf2 = new Rfc2898DeriveBytes(userModel.Password, salt, 1000);
                //byte[] hash = pbkdf2.GetBytes(20);
                //byte[] hashBytes = new byte[36];
                //string Salt = Convert.ToBase64String(hash);
                //userModel.Salt_Value = Salt;
                //Array.Copy(salt, 0, hashBytes, 0, 16);
                //Array.Copy(hash, 0, hashBytes, 16, 20);
                //string savedPasswordHash = Convert.ToBase64String(hashBytes);
                //userModel.Password = savedPasswordHash;
                //userModel.ConfirmPassword = savedPasswordHash;
                //HASHING THE PASSWORD

                try
                {
                    var addr = new System.Net.Mail.MailAddress(userModel.Email);
                    if (addr.Address == userModel.Email)
                    {

                        using (TattooProjectEntities dbModel = new TattooProjectEntities())
                        {
                            var isEmailAlreadyExists = dbModel.Users.Any(x => x.Email == userModel.Email);
                            if (isEmailAlreadyExists) //DOES EMAIL EXISTS IN THE DB
                            {
                                ModelState.AddModelError("Email", "User with this email already exists");
                                return View(userModel);
                            }
                            else
                            {
                                dbModel.Users.Add(userModel.MapTo<User>());
                                dbModel.SaveChanges();
                            }
                        }
                        ModelState.Clear();
                        ViewBag.SuccessMessage = "Registration Succesful";
                        return View("UserSignup", new UserModel());
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "Invalid Email Type.");
                        return View("UserSignup", userModel);
                    }
                }
                catch (Exception Ex)
                {
                    ModelState.AddModelError("Email", Ex.ToString());
                    return View();
                }
            }
            else
            {
                ViewBag.SuccessMessage = "Invalid Model";
                return View("UserSignup", userModel);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            UserLogin userLogin = new UserLogin();
            return View(userLogin);
        }

        [HttpPost]
        public ActionResult Login(UserLogin userLogin)
        {

            if (ModelState.IsValid)
            {
                using (TattooProjectEntities dbModel = new TattooProjectEntities())
                {
                    var isEmailAlreadyExists = dbModel.Users.Any(user => user.Email == userLogin.Email);
                    if (!isEmailAlreadyExists) //DOES EMAIL EXISTS IN THE DB
                    {
                        ModelState.AddModelError("Email", "Email Address is wrong.");
                        return View(userLogin);
                    }
                    else if (isEmailAlreadyExists)
                    {
                        var theUser = dbModel.Users.Where(a => a.Email == userLogin.Email).FirstOrDefault();
                        if (theUser.Password == userLogin.Password)
                        {
                            ViewBag.SuccessMessage = "Succesfully logged in.";
                            ViewBag.Message = theUser;

                            var artists = (from a in dbModel.Artists
                                           select a).ToList();

                            HttpCookie kullanici = new HttpCookie("users");
                            kullanici.Values.Add("UserId", theUser.ID.ToString());
                            kullanici.Values.Add("Name", theUser.Name.ToString());
                            kullanici.Values.Add("Surname", theUser.Surname.ToString());
                            Response.Cookies.Add(kullanici);
                            Utiilities.Name = theUser.Name.ToString();
                            Utiilities.SurName = theUser.Surname.ToString();
                            Utiilities.Id = theUser.ID;
                            return View("LandingPageUser", Tuple.Create(theUser, artists));
                        }
                        else
                        {
                            ModelState.AddModelError("Password", "Password is wrong.");
                            return View(userLogin);
                        }
                    }
                }
            }
            return View();
        }


        public ActionResult BookingList()
        {
            List<vwBooking> model;
            using (TattooProjectEntities dbModel = new TattooProjectEntities())
            {
                if (Utiilities.Artist == true)
                {
                    model = dbModel.vwBookings.Where(p => p.ArtistID == Utiilities.Id).ToList();
                }
                else
                {
                    model = dbModel.vwBookings.Where(p => p.UserId == Utiilities.Id).ToList();
                }
            }
            return View(model);
        }


        public ActionResult BookingDelete(int id)
        {
            TattooProjectEntities dbModel = new TattooProjectEntities();

            var result = dbModel.Bookings.FirstOrDefault(p => p.Id == id);
            dbModel.Bookings.Remove(result);
            dbModel.SaveChanges();
            List<vwBooking> model = dbModel.vwBookings.Where(p => p.UserId == Utiilities.Id).ToList();

            return View("BookingList", model);
        }

    }
}