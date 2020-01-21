using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Model_EF
{
    public class Bus_Dag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public Bus Bus { get; set; }
        public DagTemplate Template { get; set; }

        public ICollection<Bus_Dag_Persoon> BDP { get; set; }

        [NotMapped]
        public ICollection<Begeleider> Begeleiders
        {
            get
            {
                return BDP.Where(a => a.Persoon is Begeleider)
                          .Select(a => (Begeleider)a.Persoon).ToList();
            }
        }

        [NotMapped]
        public ICollection<Client> Clienten
        {
            get
            {
                return BDP.Where(a => a.Persoon is Client)
                          .Select(a => (Client)a.Persoon).ToList();
            }
        }

        protected Bus_Dag()
        {
            BDP = new List<Bus_Dag_Persoon>();
        }

        public Bus_Dag(Bus b)
        {
            this.Bus = b;
        }

        public void VerwijderPersonen(ICollection<Persoon> personen)
        {
            var bdpLijst = BDP.Where(bdp => personen.Contains(bdp.Persoon)).ToArray();
            Array.ForEach(bdpLijst, (bdp) => BDP.Remove(bdp));
        }

        public void VoegPersonenToe(ICollection<Persoon> personen)
        {
            var bdpLijst = personen.Select(p => new Bus_Dag_Persoon() { BD = this, Persoon = p }).ToList();
            ((List<Bus_Dag_Persoon>)BDP).AddRange(bdpLijst);
        }

        public void UpdatePersonen(ICollection<Persoon> personen)
        {
            ICollection<Bus_Dag_Persoon> ToDelete = new List<Bus_Dag_Persoon>();

            Persoon[] ToAdd = new Persoon[] { };

            if (personen.First() is Begeleider)
            {
                ToAdd = personen.Except(Begeleiders).ToArray();
                ToDelete = BDP.Where(bdp => !personen.Contains(bdp.Persoon) && bdp.Persoon is Begeleider).ToList();
            }

            if (personen.First() is Client)
            {
                ToAdd = personen.Except(Clienten).ToArray();
                ToDelete = BDP.Where(bdp => !personen.Contains(bdp.Persoon) && bdp.Persoon is Client).ToList();
            }

            BDP = BDP.Except(ToDelete).ToList();
            List<Bus_Dag_Persoon> lbdp = new List<Bus_Dag_Persoon>();
            Array.ForEach(ToAdd, add => lbdp.Add(new Bus_Dag_Persoon() { Persoon = add, BD = this }));
            ((List<Bus_Dag_Persoon>)BDP).AddRange(lbdp);
        }
    }
}
