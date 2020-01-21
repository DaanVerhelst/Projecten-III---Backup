using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.DTO
{
    public class MenuDTO
    {
        public long ID { get; set; }
        public int dagNr { get; set; }
        public String dagNaam { get; set; }
        public String Soep { get; set; }
        public String Groente { get; set; }
        public String Vlees { get; set; }
        public String Supplement { get; set; }

        public MenuDTO(DagMenu dm)
        {
            this.ID = dm.ID;
            this.dagNr = dm.dagNr;
            this.dagNaam = dm.dagNaam;
            this.Soep = dm.Soep;
            this.Groente = dm.Groente;
            this.Vlees = dm.Vlees;
            this.Supplement = dm.Supplement;
        }

        public MenuDTO()
        {

        }
    }
}
