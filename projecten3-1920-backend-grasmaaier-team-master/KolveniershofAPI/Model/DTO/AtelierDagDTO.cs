using System;
using KolveniershofAPI.Model.Model_EF;

namespace KolveniershofAPI.Model.DTO
{
    public class AtelierDagDTO
    {
        public long AtelierID { get; set; }

        public TimeSpan? Start { get; set; }
        public TimeSpan? Eind { get; set; }

        public AtelierDagDTO() { }

        public AtelierDagDTO(Atelier_Dag ad)
        {
            AtelierID = ad.Atelier.ID;
            Start = ad.Start;
            Eind = ad.End;
        }
    }
}