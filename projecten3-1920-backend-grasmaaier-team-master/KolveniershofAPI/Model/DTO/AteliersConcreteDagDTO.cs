using KolveniershofAPI.Model.Model_EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.DTO {
    public class AteliersConcreteDagDTO {
        public PersoonDTO[] Clienten { get; set; }
        public PersoonDTO[] Begeleiders { get; set; }
        public long AtelierID { get; set; }
        public string  Naam { get; set; }
        public TimeSpan? Start { get; set; }
        public TimeSpan? End { get; set; }

        public AteliersConcreteDagDTO(Atelier_Dag ad) {
            Begeleiders = ad.Begeleiders.Select(p=> new PersoonDTO(p)).ToArray();
            Clienten = ad.Clienten.Select(p => new PersoonDTO(p)).ToArray();
            AtelierID = ad.Atelier.ID;
            Naam = ad.Atelier.Naam;
            Start = ad.Start;
            End = ad.End;
        }
    }
}
