using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model
{
    public class BijzonderUurRegistratie:NotiteLijsten{
        public DateTime UurToekomst { get; set; }
        public DateTime UurBuiten { get; set; }
        public string Reden { get; set; }
		public BijzonderUurRegistratie()
		{
				
		}
		public BijzonderUurRegistratie(Persoon p, DateTime buiten, DateTime toekomst, string reden)
		{
			Persoon = p;
			UurBuiten = buiten;
			UurToekomst = toekomst;
			Reden = reden;
		}
    }
}
