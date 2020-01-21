using KolveniershofAPI.Model.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model
{
    public class DagMenu
    {
        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public int dagNr { get; set; }
        public string dagNaam { get; set; }
        public String Soep { get; set; }
        public String Groente { get; set; }
        public String Vlees { get; set; }
        public String Supplement { get; set; }
        #endregion

        #region Constructors
        public DagMenu(int dagNr, String dagNaam, String Soep, String Groente, String Vlees, String Supplement)
        {
            this.dagNr = dagNr;
            this.dagNaam = dagNaam;
            this.Soep = Soep;
            this.Groente = Groente;
            this.Vlees = Vlees;
            this.Supplement = Supplement;
        }

        public DagMenu(String Soep, String Groente, String Vlees, String Supplement)
        {
            this.Soep = Soep;
            this.Groente = Groente;
            this.Vlees = Vlees;
            this.Supplement = Supplement;
        }

        public DagMenu(MenuDTO menuDTO)
        {
            this.Soep = menuDTO.Soep;
            this.Groente = menuDTO.Groente;
            this.Vlees = menuDTO.Vlees;
            this.Supplement = menuDTO.Supplement;
        }
        public DagMenu() { }
        #endregion
    }
}
