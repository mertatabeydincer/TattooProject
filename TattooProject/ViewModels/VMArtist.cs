using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TattooProject.Models;

namespace TattooProject.ViewModels
{
    public class VMArtist
    {

        public VMArtist()
        {
            this.PuanList = new List<SelectListItem>();
            #region
            PuanList.Add(new SelectListItem { Text = "1", Value = "1" });
            PuanList.Add(new SelectListItem { Text = "2", Value = "2" });
            PuanList.Add(new SelectListItem { Text = "3", Value = "3" });
            PuanList.Add(new SelectListItem { Text = "4", Value = "4" });
            PuanList.Add(new SelectListItem { Text = "5", Value = "5" });
            PointGive = false;
            #endregion
        }


        public int tabPage { get; set; }
        //ZAMAN TÜNELİ
        public List<ArtistWork> ArtistWorkList { get; set; }
        public List<ArtistComment> ArtistCommentList { get; set; }


        //PROFİL
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string PasswordArtist { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Studios { get; set; }
        public string Comments { get; set; }
        public Nullable<float> Rates { get; set; }
        public string Gender { get; set; }
        public Nullable<int> Experience { get; set; }
        public string Description { get; set; }
        public string Expertise_Area { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public Nullable<int> AcceptedComment { get; set; }
        public Nullable<int> RejectedComment { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public Nullable<int> ImageCount { get; set; }
        public int ArtistID { get; set; }

        public int KullaniciID { get; set; }
        public Nullable<int> TotalPointCount { get; set; }
        public Nullable<decimal> TotalPoint { get; set; }
        public Nullable<decimal> Point { get; set; }
        public virtual List<SelectListItem> PuanList { get; set; }
        public bool PointGive { get; set; }

        //ÇALIŞMA EKLEME
        public HttpPostedFileBase File { get; set; }
        public string WorkImageUrl { get; set; }
        public string WorkDescription { get; set; }
    }
}