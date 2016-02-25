using System;
using System.ComponentModel.DataAnnotations;

namespace Sales.Web.Models
{
    public class ManageUserViewModel
    {
        [Required]
        [Display(Name = "Your Login")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class AddAdminViewModel
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [Display(Name = "Admin name")]
        public string AdminName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class AddCompanyViewModel
    {
        [Required]
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [Display(Name = "Company Email")]
        public string Email { get; set; }

        [Display(Name = "Company Mobile Number")]
        public string Mobile { get; set; }
    }
    public class AddEmployeeModel
    {
        public Guid CompanyId { get; set; }
        [Required]
        [Display(Name = "Employee name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Employee last name")]
        public string LastName { get; set; }
        [Display(Name = "Employee Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class AddButtonModel
    {
        public Guid CompanyId { get; set; }
        [Required]
        [Display(Name = "Button text")]
        public string Text { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Font color")]
        public string FontColor { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Background color")]
        public string BgColor { get; set; }
    }
}
