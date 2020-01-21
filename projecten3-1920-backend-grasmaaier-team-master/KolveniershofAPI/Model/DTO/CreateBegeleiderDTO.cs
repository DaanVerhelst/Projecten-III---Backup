using System;
namespace KolveniershofAPI.Model.DTO
{
    public class CreateBegeleiderDTO
    {
        public string Voornaam { get; set; }
        public string Familienaam { get; set; }
        public bool IsAdmin { get; set; }

        public CreateBegeleiderDTO()
        {
        }
    }
}
