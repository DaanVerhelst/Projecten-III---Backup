using System.ComponentModel.DataAnnotations;


namespace KolveniershofAPI.Model.DTO{
    public class LoginDTO{
        [Required(AllowEmptyStrings=false,ErrorMessage="Gelieve een email mee te geven")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings=false,ErrorMessage="Please provide a value for the password")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Password needs to be 6 in length")]
        public string Password { get; set; }
    }
}
