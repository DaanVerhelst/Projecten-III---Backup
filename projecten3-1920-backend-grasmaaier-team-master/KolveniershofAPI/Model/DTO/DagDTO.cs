using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.DTO
{
    public class DagDTO{
        public DateTime Date { get; set; }
        public AteliersConcreteDagDTO[] Ateliers { get; set; }

        public DagDTO(Dag ad){
            Date = ad.Date;
            try
            {
                Ateliers = ad.Atelier_Dag.Select(p => new AteliersConcreteDagDTO(p)).ToArray();
            }
            catch {
                Ateliers = new List<AteliersConcreteDagDTO>().ToArray();
            }
        }
    }
}
      