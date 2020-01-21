using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.DTO { 
    public class ActiviteitPersonenDTO{
        public PersoonDTO[] Personen { get; set; }
        public string ActiviteitNaam  { get; set; }
        public long ActiviteitID { get; set; }

        public ActiviteitPersonenDTO(Atelier at,Persoon[] personen){
            Personen = personen.Select(p => new PersoonDTO(p)).ToArray();
            ActiviteitNaam = at.Naam;
            ActiviteitID = at.ID;
        }
    }
}
