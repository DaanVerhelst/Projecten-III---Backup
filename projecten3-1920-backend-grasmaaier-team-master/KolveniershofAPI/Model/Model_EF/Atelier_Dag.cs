using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KolveniershofAPI.Model.Model_EF
{
    public class Atelier_Dag {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public Atelier Atelier { get; set; }
        public DagTemplate Template { get; set; }

        public ICollection<Atelier_Dag_Persoon> ADP { get; set; }

        [NotMapped]
        public ICollection<Begeleider> Begeleiders { get {
                return ADP.Where(a => a.Persoon is Begeleider)
                          .Select(a => (Begeleider)a.Persoon).ToList();
            }
        }

        [NotMapped]
        public ICollection<Client> Clienten { get {
                return ADP.Where(a => a.Persoon is Client)
                          .Select(a => (Client)a.Persoon).ToList();
            }
        }

        public TimeSpan? Start { get; set; }
        public TimeSpan? End { get; set; }

        public Atelier_Dag() {
            ADP = new List<Atelier_Dag_Persoon>();
        }

        public Atelier_Dag(TimeSpan? start) {
            Start = start;
            ADP = new List<Atelier_Dag_Persoon>();
        }

        public void VerwijderPersonen(ICollection<Persoon> personen) {
            var adpLijst = ADP.Where(adp => personen.Contains(adp.Persoon)).ToArray();
            Array.ForEach(adpLijst, (adp) => ADP.Remove(adp));
        }

        public void VoegPersonenToe(ICollection<Persoon> personen) {
            var adpLijst = personen.Select(p => new Atelier_Dag_Persoon(){AD = this,Persoon = p}).ToList();
            ((List<Atelier_Dag_Persoon>)ADP).AddRange(adpLijst);
        }

        public void UpdatePersonen(ICollection<Persoon> personen){
            ICollection<Atelier_Dag_Persoon> ToDelete = new List<Atelier_Dag_Persoon>();

            Persoon[] ToAdd = new Persoon[] { };

            if (personen.First() is Begeleider){
                ToAdd = personen.Except(Begeleiders).ToArray();
                ToDelete = ADP.Where(adp => !personen.Contains(adp.Persoon) && adp.Persoon is Begeleider).ToList();
            }

            if (personen.First() is Client){
                ToAdd = personen.Except(Clienten).ToArray();
                ToDelete = ADP.Where(adp => !personen.Contains(adp.Persoon) && adp.Persoon is Client).ToList();
            }

            ADP = ADP.Except(ToDelete).ToList();
            List<Atelier_Dag_Persoon> ladp = new List<Atelier_Dag_Persoon>();
            Array.ForEach(ToAdd, add => ladp.Add(new Atelier_Dag_Persoon() { Persoon = add, AD = this }));
            ((List<Atelier_Dag_Persoon>)ADP).AddRange(ladp);
        }
    }
}
