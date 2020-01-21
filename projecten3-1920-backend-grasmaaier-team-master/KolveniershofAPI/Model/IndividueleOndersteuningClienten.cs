using System;

namespace KolveniershofAPI.Model
{
    public class IndividueleOndersteuningClienten:NotiteLijsten{
        public DateTime Tijd { get; set; }
        public string Wie { get; set; }
        public string Wat { get; set; }

		public IndividueleOndersteuningClienten()
		{

		}

		public IndividueleOndersteuningClienten(Persoon pers, DateTime tijd, string wie, string wat):base(){
			Persoon = pers;
			Tijd = tijd;
			Wie = wie;
			Wat = wat;
		}
    }
}
