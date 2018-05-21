using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace LSQEx.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username Required", AllowEmptyStrings = false)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberUser { get; set; }

    }

    public class UserModel
    {
        public int UserID { get; set; }
        [DisplayName("Username")]
        public string UserName { get; set; }
        //[DataType(DataType.Password)]
        //public string Password { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Email Address")]
        public string Email { get; set; }

        public string Role { get; set; }
        public bool CanAddNews { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }

    }

    public class EditModel
    {
        public int UserID { get; set; }

        public int UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get;  set;}

        public string Email { get; set; }
    }


    public class RegistrationModel
    {
        public int UserID { get; set; }
        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} characters long", MinimumLength = 6)]
        [RegularExpression(@"^[a-zA-Z]+[ 0-9a-zA-Z-_]*$", ErrorMessage = "Username must start with a character and can only include numbers and underscores.")]
        [Required(ErrorMessage = "Username Required", AllowEmptyStrings = false)]
        [DisplayName("Username")]
        public string UserName { get; set; }


        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} characters long", MinimumLength = 6)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$",ErrorMessage = "Password must contain at least one digit, uppercase and lowercase character.")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(.{8,15})$", ErrorMessage = "Password must contain at least one digit, uppercase and lowercase character.")]
        [Required(ErrorMessage = "Password Required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Required", AllowEmptyStrings = false)]
        [Compare("Password", ErrorMessage = "Password does not match with Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First Name Required", AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required", AllowEmptyStrings = false)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address Required", AllowEmptyStrings = false)]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Email Address must be valid")]
        //[RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z-.]+$", ErrorMessage = "Email Address must be valid")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Address must be valid")]
        public string Email { get; set; }
    }


    public class ChangePasswordModel
    {
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Display(Name ="Current Password")]
        public string OldPassword { get; set; }

        [Display(Name ="New Password")]
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} characters long", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Display(Name ="Confirm New Password")]
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="New Password and Confirmation Password do not match")]
        public string ConfirmNewPassword { get; set; }
    }

}