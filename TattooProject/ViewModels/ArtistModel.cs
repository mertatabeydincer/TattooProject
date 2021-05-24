using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TattooProject.ViewModels
{
    public class ArtistModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required.")]
        public string Surname { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^.*(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*\(\)_\-+=]).*$", ErrorMessage = "Password must contain minimum nine characters, at least one letter and one number:")]
        public string PasswordArtist { get; set; }

        [Required(ErrorMessage = "Password confirmation is required.")]
        [Compare(nameof(PasswordArtist), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPasswordArtist { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email address is invalid.")]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public Nullable<int> Experience { get; set; }
        public string Expertise_Area { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Today;
    }

    public partial class ArtistLogin
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}