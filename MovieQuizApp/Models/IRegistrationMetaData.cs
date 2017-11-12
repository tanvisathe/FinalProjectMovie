using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MovieQuizApp.Models
{
    public interface IRegistrationMetaData
    {
        [Key]
         int UserID { get; set; }

        [Display(Name = "First Name")]
        [Required]
         string FirstName { get; set; }

        [Display(Name = "Last Name")]
         string LastName { get; set; }

        [Display(Name = "User Name")]
        [Required]
         string Username { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required]
         string Password { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        [Required]
         string Email { get; set; }

        [Display(Name = "Mobile")]
        [Phone]
         string Mobile { get; set; }

    }

    [MetadataType(typeof(IRegistrationMetaData))]
    public partial class Registration : IRegistrationMetaData
    {

    }
}