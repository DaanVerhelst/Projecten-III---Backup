using KolveniershofAPI.Model.Model_EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model
{
    public class DagTemplate
    {
        public ICollection<Atelier_Dag> Atelier_Dag { get; set; }
        public ICollection<Bus_Dag> Bus_Dag { get; set; }
        [NotMapped]
        public ICollection<Atelier> Ateliers { get => Atelier_Dag.Select(ad => ad.Atelier).ToList(); }
        [NotMapped]
        public ICollection<Bus> Bussen { get => Bus_Dag.Select(ad => ad.Bus).ToList(); }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public int WeekNR { get; set; }
        public int DagNR { get; set; }

        public DagMenu Menu { get; set; }
        public long? MenuID { get; set; }

        public DagTemplate()
        {
            Atelier_Dag = new List<Atelier_Dag>();
            Bus_Dag = new List<Bus_Dag>();
        }

        public void AtelierToevoegen(Atelier at)
        {
            AtelierToevoegenOpTijdstip(at, new TimeSpan(), new TimeSpan());
        }


        public void BusToevoegen(Bus at)
        {
            BusToevoegenOpTijdstip(at);
        }



        public void AtelierToevoegenOpTijdstip(Atelier at, TimeSpan? start, TimeSpan? end)
        {

            if (at == null)
            {
                throw new ArgumentException("Atelier is null");
            }
            if (start == null)
            {
                throw new ArgumentException("start is null");
            }
            if (end == null)
            {
                throw new ArgumentException("End is null");
            }

            Atelier_Dag atd = new Atelier_Dag(start) { Atelier = at, Template = this, End = end };
            Atelier_Dag.Add(atd);
        }


        public void BusToevoegenOpTijdstip(Bus b)
        {
            if (b == null)
            {
                throw new ArgumentException("Bus is null");
            }

            Bus_Dag bd = new Bus_Dag(b) { Template = this};
            Bus_Dag.Add(bd);
        }


        public IEnumerable<Client> GeefClientenVoorAtelier(Atelier at)
        {
            if (at == null)

            {
                throw new ArgumentException("Atelier is null");
            }

            Atelier_Dag ad = GetAtelierDagVanAtelier(at);
            return ad?.Clienten;
        }

        public IEnumerable<Begeleider> GeefBegeleidersVoorAtelier(Atelier at)
        {
            if (at == null)
            {
                throw new ArgumentException("Atelier is null");
            }

            Atelier_Dag ad = GetAtelierDagVanAtelier(at);
            return ad?.Begeleiders;
        }

        public void VoegClientenToeAanAtelier(Atelier at, ICollection<Client> clients)
        {
            Atelier_Dag ad = GetAtelierDagVanAtelier(at);

            if (ad == null)
                throw new ArgumentException("This atelier doesn't exist");

            ad.VoegPersonenToe(clients.Select(c => (Persoon)c).ToList());
        }

        public void VoegBegeleidersToeAanAtelier(Atelier at, ICollection<Begeleider> begeleiders)
        {
            Atelier_Dag ad = GetAtelierDagVanAtelier(at);

            if (ad == null)
                throw new ArgumentException("This atelier doesn't exist");

            ad.VoegPersonenToe(begeleiders.Select(c => (Persoon)c).ToList());
        }


        public Atelier_Dag GetAtelierDagVanAtelier(Atelier at) {
            return Atelier_Dag.Where(a => a.Atelier==at && a.Template == this).FirstOrDefault();
        return Atelier_Dag.Where(a => a.Atelier == at && a.Template == this).FirstOrDefault();
        }
        public List<Atelier_Dag> GeefAteliersVM()
        {
            return Atelier_Dag.Where(a => a.Start.Value.Hours < 12).ToList();
        }
        public List<Atelier_Dag> GeefAteliersNM()
        {
            return Atelier_Dag.Where(a => a.Start.Value.Hours >= 12).ToList();
        }

    }
}