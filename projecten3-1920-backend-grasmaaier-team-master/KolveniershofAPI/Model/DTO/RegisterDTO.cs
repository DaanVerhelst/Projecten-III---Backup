using System.ComponentModel.DataAnnotations;

namespace KolveniershofAPI.Model.DTO{
    public class RegisterDTO:LoginDTO{
        [Required(AllowEmptyStrings =false,ErrorMessage ="Gelieve een voornaam op te geven")]
        [DataType(DataType.Text)]
        public string Voornaam { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Gelieve een familienaam op te geven")]
        [DataType(DataType.Text)]
        public string Familienaam { get; set; }
    }
}
