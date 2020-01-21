using System;
using System.Collections.Generic;
using KolveniershofAPI.Model.Model_EF;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace KolveniershofAPI.Model
{
    public class Dag : DagTemplate
    {
        public DateTime Date { get; set; }

        public ICollection<Dag_Persoon> Dag_Personen { get; set; }
        public Notitieblok Notities { get; set; }

        public Dag() : this(DateTime.Now) { }

        public Dag(DateTime datetime)
        {
            Date = datetime;
            Notities = new Notitieblok();
            Atelier_Dag = new List<Atelier_Dag>();
            Bus_Dag = new List<Bus_Dag>();
            Dag_Personen = new List<Dag_Persoon>();

        }
        public IEnumerable<Persoon> GeefAfwezigen()
        {
            return Dag_Personen.Where(d => d.IsZiek = true).Select(p => p.Persoon).ToList();
        }
        public IEnumerable<Persoon> GeefVrijwilligers()
        {
            return Dag_Personen.Where(d => d.rol == "Vrijwilliger").Select(p => p.Persoon).ToList();
        }
        public IEnumerable<Persoon> GeefStagair()
        {
            return Dag_Personen.Where(d => d.rol == "Stagaire").Select(p => p.Persoon).ToList();
        }
        public void VoegVrijwilligerToe(Persoon pers)
        {
            Dag_Personen.Add(new Dag_Persoon() { Persoon = pers, Dag = this, IsZiek = false, rol = "Vrijwilliger" });
        }
        public void VoegStagairToe(Persoon pers)
        {
            Dag_Personen.Add(new Dag_Persoon() { Persoon = pers, Dag = this, IsZiek = false, rol = "Stagaire" });
        }

        public void VoegIndividueleOndersteuningToe(DateTime tijd, Persoon pers, string wat, string wie)
        {
            Notities.VoegIndividueleOndersteuningNotitieToe(tijd, pers, wat, wie);
        }

        public void VoegBijzondereUurRegeling(DateTime binnen, DateTime toekomst, Persoon pers, string reden)
        {
            Notities.VoegBijzondereUurRegelingToe(binnen, toekomst, pers, reden);
        }

        public void ZetPersonenOpAfwezig(ICollection<Persoon> Personen)
        {
            foreach (Persoon p in Personen)
            {
                Dag_Personen.Add(new Dag_Persoon() { Persoon = p, Dag = this, IsZiek = true });
            }
        }

        public void zetPersoonOpAfwezig(Persoon persoon)
        {
            if (persoon == null) { throw new ArgumentException("Please enter a person"); }
            Dag_Personen.Add(new Dag_Persoon() { Persoon = persoon, Dag = this, IsZiek = true });
        }

        public void voegCommentaarToe(string comment, Persoon p)
        {
            if (p == null) { throw new ArgumentException("Please enter a person"); }
            if (comment == null || comment == "") { throw new ArgumentException("Please enter a valid comment"); }
            Dag_Personen.Add(new Dag_Persoon() { Persoon = p, Dag = this, commentaar = comment });
        }
        public void pasDuurAtelierDagAan(Atelier at, TimeSpan? eind)
        {
            Atelier_Dag ad = Atelier_Dag.FirstOrDefault(e => e.Atelier == at);
            if (ad == null)
            {
                throw new ArgumentException("This Atelier_Dag does not exist");
            }
            else
            {
                ad.End = eind;
            }

        }

		public void verwijderAtelierUitDag(Atelier at)
		{
			Atelier_Dag ad = Atelier_Dag.FirstOrDefault(e => e.Atelier == at);
			if(ad == null)
			{
				throw new ArgumentException("This atelier_dag does not exist");
			}else
			{
				Atelier_Dag.Remove(ad);
			}

		}

		public void verwijderBegeleiderUitActiviteit(Atelier at, Begeleider begeleider)
		{
			Atelier_Dag ad = Atelier_Dag.FirstOrDefault(e => e.Atelier == at);
			if(ad == null)
			{
				throw new ArgumentException("this atelier_dag does not exist");
			} else
			{
				ad.Begeleiders.Remove(begeleider);
			}
		}

		internal void verwijderClientUitActiviteit(Atelier at, Client e)
		{
			Atelier_Dag ad = Atelier_Dag.FirstOrDefault(atelier => atelier.Atelier == at);
			if (ad == null)
			{
				throw new ArgumentException("this atelier_dag does not exist");
			}
			else {
				ad.Clienten.Remove(e);
			}
		}

		public ICollection<String> GetNotitieCategoriën()
		{
			return Notities.GetCategoriën();
		}

		internal void voegNotitieToe(string commentaar, string catEnum)
		{
			bool parse = Enum.TryParse(catEnum, out NotieblokCategorie nc);
			if (parse)
			{
				Notities.VoegCommentToe(commentaar, nc);
			} else
			{
				throw new ArgumentException("this category does not exist");
			}
		}

		internal void removeCommentaar(string catEnum, int commentID)
		{
			Enum.TryParse(catEnum, out NotieblokCategorie nc);
			Notities.removeCommentaar(nc, commentID);
		}
      public Notitieblok getNotietieBlock()
        {
            return Notities;
        }
	}
}

