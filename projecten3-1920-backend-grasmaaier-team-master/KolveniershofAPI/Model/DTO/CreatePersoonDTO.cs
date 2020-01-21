using System;
namespace KolveniershofAPI.Model.DTO
{
    public class CreatePersoonDTO
    {
        public string Voornaam { get; set; }
        public string Familienaam { get; set; }

        public CreatePersoonDTO(Persoon pers)
        {
            Voornaam = pers.Voornaam;
            Familienaam = pers.Familienaam;
        }

        public CreatePersoonDTO()
        {
                
        }
    }
}
