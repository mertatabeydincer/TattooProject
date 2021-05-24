using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TattooProject.Models;
using TattooProject.ViewModels;

namespace TattooProject.Controllers
{
    public class ArtistController : BaseController
    {

        TattooProjectEntities db = new TattooProjectEntities();


        [HttpGet]
        public ActionResult Login()
        {
            ArtistLogin Login = new ArtistLogin();
            return View(Login);
        }

        [HttpPost]
        public ActionResult Login(UserLogin userLogin)
        {

            if (ModelState.IsValid)
            {
                using (TattooProjectEntities dbModel = new TattooProjectEntities())
                {
                    var isEmailAlreadyExists = dbModel.Artists.Any(user => user.Email == userLogin.Email);
                    if (!isEmailAlreadyExists) //DOES EMAIL EXISTS IN THE DB
                    {
                        ModelState.AddModelError("Email", "Email Address is wrong.");
                        return View(userLogin);
                    }
                    else if (isEmailAlreadyExists)
                    {
                        var theUser = dbModel.Artists.Where(a => a.Email == userLogin.Email).FirstOrDefault();
                        if (theUser.PasswordArtist == userLogin.Password)
                        {
                            ViewBag.SuccessMessage = "Succesfully logged in.";
                            ViewBag.Message = theUser;


                            HttpCookie kullanici = new HttpCookie("users");
                            kullanici.Values.Add("UserId", theUser.ArtistID.ToString());
                            kullanici.Values.Add("Name", theUser.Name.ToString());
                            kullanici.Values.Add("SurName", theUser.Surname.ToString());
                            kullanici.Values.Add("Artist", "OK");
                            Response.Cookies.Add(kullanici);
                            Utiilities.Name = theUser.Name;
                            Utiilities.SurName = theUser.Surname;
                            Utiilities.Id = theUser.ArtistID;
                            Utiilities.Artist= true;
                            return RedirectToAction("ArtistProfile");
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

        public ActionResult ArtistSignup()
        {
            var artistModel = new ArtistModel();
            return View(artistModel);
        }

        [HttpPost]
        public ActionResult ArtistSignup(ArtistModel artistModel)
        {
            if (ModelState.IsValid)
            {
                //try
                //{
                var addr = new System.Net.Mail.MailAddress(artistModel.Email);
                if (addr.Address == artistModel.Email)
                {
                    using (TattooProjectEntities dbModel = new TattooProjectEntities())
                    {
                        var isEmailAlreadyExists = dbModel.Artists.Any(ex => ex.Email == artistModel.Email);
                        if (isEmailAlreadyExists) //DOES EMAIL EXISTS IN THE DB
                        {
                            ModelState.AddModelError("Email", "Artist with this email already exists");
                            return View(artistModel);
                        }
                        else
                        {
                            dbModel.Artists.Add(artistModel.MapTo<Artist>());
                            dbModel.SaveChanges();
                        }
                    }
                    ModelState.Clear();
                    ViewBag.SuccessMessage = "Registration Succesful";
                    return View("ArtistSignup", new ArtistModel());
                }
                else
                {
                    return View("ArtistSignup", artistModel);
                }
                //}
                //catch(Exception ex)
                //{
                //    return View("ArtistSignup", artist);
                //}
            }
            else
            {
                return View("ArtistSignup", artistModel);
            }
        }

        public ActionResult ArtistProfile(int? id)
        {
            HttpCookie Cookie = Request.Cookies["users"];
            int kullaniciid = Convert.ToInt32(Cookie["UserId"]);
            if (id == null) id = kullaniciid;

            VMArtist model = new VMArtist();
            //profil bilgilerinin bind edilmesi
            var result = db.Artists.FirstOrDefault(p => p.ArtistID == id);
            model.KullaniciID = kullaniciid;
            model.Name = result.Name;
            model.Surname = result.Surname;
            model.Email = result.Email;
            model.Phone = result.Phone;
            model.RegistrationDate = result.RegistrationDate;
            model.Experience = result.Experience;
            model.Address = result.Address;
            model.Username = result.Username;
            model.Studios = result.Studios;
            model.Description = result.Description;
            model.Expertise_Area = result.Expertise_Area;
            model.ArtistID = (int)id;
            model.Point = result.TotalPoint / result.TotalPointCount;
            //çalışmaların bind edilmesi
            model.ArtistWorkList = db.ArtistWorks.Where(p => p.ArtistId == id).ToList();
            model.ArtistCommentList = db.ArtistComments.Where(p => p.ArtistId == id).ToList();
            var resultUserPoint = db.UserArtistPoints.FirstOrDefault(p => p.UserId == kullaniciid && p.ArtistId == id);
            if (resultUserPoint == null) model.PointGive = true;
            return View("ArtistProfile", model);
        }

        [HttpPost]
        public ActionResult ArtistProfile(VMArtist model)
        {
            HttpCookie Cookie = Request.Cookies["users"];
            int kullaniciid = Convert.ToInt32(Cookie["UserId"]);

            if (model.tabPage == 3)//Profil güncelleme yapıyor ise
            {
                var result = db.Artists.FirstOrDefault(p => p.ArtistID == model.ArtistID);
                result.Name = model.Name;
                result.Surname = model.Surname;
                result.Email = model.Email;
                result.Phone = model.Phone;
                result.RegistrationDate = model.RegistrationDate;
                result.Experience = model.Experience;
                result.Address = model.Address;
                result.Username = model.Username;
                result.Studios = model.Studios;
                result.Description = model.Description;
                result.Expertise_Area = model.Expertise_Area;

            }
            else if (model.tabPage == 4)//Çalışma yüklüyor ise
            {
                ArtistWork artistWork = new ArtistWork();
                string dosyaYolu = Path.GetFileName(model.File.FileName);
                var yuklemeYeri = Path.Combine(Server.MapPath("~/Work"), dosyaYolu);
                model.File.SaveAs(yuklemeYeri);

                artistWork.ArtistId = model.ArtistID;
                artistWork.Description = model.WorkDescription;
                artistWork.WorkImageUrl = yuklemeYeri;

                db.ArtistWorks.Add(artistWork);
            }
            else if (model.tabPage == 2)//Yorum yaptıysa
            {
                ArtistComment artistComment = new ArtistComment();
                artistComment.ArtistId = model.ArtistID;
                artistComment.Description = model.Comments;
                artistComment.UserId = kullaniciid;
                artistComment.CDate = DateTime.Now;
                db.ArtistComments.Add(artistComment);
            }
            else if (model.tabPage == 1)//Puanlama yaptıysa
            {
                UserArtistPoint userArtistPoint = new UserArtistPoint();
                userArtistPoint.ArtistId = model.ArtistID;
                userArtistPoint.UserId = kullaniciid;
                db.UserArtistPoints.Add(userArtistPoint);
                var result = db.Artists.FirstOrDefault(p => p.ArtistID == model.ArtistID);
                if (result.TotalPoint == null)//ilk defa puan atanırken null oldugu için toplama hatası olmasın diye
                {
                    result.TotalPoint = model.Point;
                    result.TotalPointCount = 1;
                }
                else
                {
                    result.TotalPoint = result.TotalPoint + model.Point;
                    result.TotalPointCount = result.TotalPointCount + 1;
                }
            }

            db.SaveChanges();

            return RedirectToAction("ArtistsList");
        }

        public ActionResult ArtistsList(string id)
        {
            using (TattooProjectEntities dbModel = new TattooProjectEntities())
            {
                List<Artist> model;
                if (String.IsNullOrEmpty(id))
                    model = dbModel.Artists.ToList();
                else
                    model = dbModel.Artists.Where(p => p.Name.Contains(id)).ToList();

                ModelState.Clear();
                return View("ArtistsList", model);
            }
        }

        public ActionResult ArtistBooking(int? id)
        {
            ViewBag.ArtistId = id;
            return View();
        }
        [HttpPost]
        public ActionResult ArtistBooking(Booking model)
        {
         
            HttpCookie Cookie = Request.Cookies["users"];
            int id = Convert.ToInt32(Cookie["UserId"]);
            model.cuserid = id;
            using (TattooProjectEntities dbModel = new TattooProjectEntities())
            {
                var result = dbModel.Bookings.FirstOrDefault(p => p.bookingdate == model.bookingdate && p.time == model.time);

                if (result == null)
                {
                    dbModel.Bookings.Add(model.MapTo<Booking>());
                    dbModel.SaveChanges();
                    ViewBag.Data = "Success";
                }
                else
                    ViewBag.Data = "Date and time busy";
            }

            return View();
        }


        public ActionResult StudioList(string id)
        {
            using (TattooProjectEntities dbModel = new TattooProjectEntities())
            {
                List<Artist> model;
                if (String.IsNullOrEmpty(id))
                    model = dbModel.Artists.ToList();
                else
                    model = dbModel.Artists.Where(p => p.Studios.Contains(id)).ToList();

                ModelState.Clear();
                return View("ArtistsList", model);
            }
        }
    }
}