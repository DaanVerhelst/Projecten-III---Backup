using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.DTO
{
    public class PersoonDTO{
        public long ID { get; set; }
        public string Voornaam { get; set; }
        public string Familienaam { get; set; }

        public PersoonDTO()
        {

        }
        public PersoonDTO(Persoon pers){
            ID = pers.ID;
            Voornaam = pers.Voornaam;
            Familienaam = pers.Familienaam;
        }

    }
}
