using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KolveniershofAPI.Model.Model_EF;

namespace KolveniershofAPI.Model.DTO
{
    public class TemplateDTO {
        public int DagNR { get; set; }
        public int WeekNR { get; set; }
        public TemplateActiviteitDTO[]  TemplateActiviteiten { get; set; }

        public TemplateDTO(DagTemplate ad) {
            DagNR = ad.DagNR;
            WeekNR = ad.WeekNR;
            TemplateActiviteiten = ad.Atelier_Dag.Select(a => new TemplateActiviteitDTO(a)).ToArray();
        }
    }
}
