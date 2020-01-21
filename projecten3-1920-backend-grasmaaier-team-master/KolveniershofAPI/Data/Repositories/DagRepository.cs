using KolveniershofAPI.Model;
using KolveniershofAPI.Model.DTO;
using KolveniershofAPI.Model.Interface;
using KolveniershofAPI.Model.Model_EF;
using Microsoft.EntityFrameworkCore;
using System;
using KolveniershofAPI.Model.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Data.Repositories {
    public class DagRepository : IDagRepository {

        private readonly DbSet<DagTemplate> _templates;
        private readonly DbSet<Dag> _dag;
        private readonly DbSet<Client> _clients;
        private readonly DbContext _context;

        public DagRepository(ApplicationDBContext context) {
            _context = context;
            _templates = context.Templates;
            _dag = context.Dagen;
            _clients = context.Clienten;
        }

        public IEnumerable<Atelier_Dag> GetDay(DateTime dag) {
            Dag d = _dag.FirstOrDefault(e => e.Date == dag);
            return d.Atelier_Dag;
        }


        public Dag GetDagByDay(DateTime dag) {
            Dag d = _dag.Include(p => p.Atelier_Dag).ThenInclude(x => x.Atelier)
                .Include(p => p.Atelier_Dag).ThenInclude(x => x.ADP).ThenInclude(q => q.Persoon)
                .Include(p => p.Notities).ThenInclude(n=>n.LijstNoties).ThenInclude(n => n.Persoon)
                .FirstOrDefault(e => e.Date.Date == dag.Date);
            return d;
        }

        public IEnumerable<Client> GetClientsInActivityByDay(DateTime dag, int activiteitID) {
            Dag ad = _dag.FirstOrDefault(e => e.Date.Date == dag.Date);
            Atelier a = ad.Ateliers.FirstOrDefault(e => e.ID == activiteitID);


            return ad.GeefClientenVoorAtelier(a); ;
        }

        public IEnumerable<Begeleider> GetBegeleidersInActivity(DateTime dag, int activiteitID) {
            Dag ad = _dag.FirstOrDefault(e => e.Date.Date == dag.Date);
            Atelier a = ad.Ateliers.FirstOrDefault(e => e.ID == activiteitID);

            return ad.GeefBegeleidersVoorAtelier(a);
        }

        public void SaveChanges() {
            _context.SaveChanges();
        }

        public void RemoveDag(Dag dag) {
            _dag.Remove(dag);
        }

        public Notitieblok getNotitiesByDay(DateTime dag) {
            Dag d = _dag.Include(da=>da.Notities)
                .ThenInclude(n=>n.LijstNoties)
                .ThenInclude(ln=>ln.Persoon)
                .FirstOrDefault(e => e.Date.Date == dag.Date);
            if (d == null) {
                return new Notitieblok();
            } else {
                return d.Notities;
            }
        }

        public bool HeeftTemplate(DateTime dt) {
            return _dag.Where(d => d.Date.Date == dt.Date)
                       .FirstOrDefault() != null;
        }

        public void AddDag(Dag dag) {
            _dag.Add(dag);
        }

        public IEnumerable<Dag> Getweek(DateTime start) {
            DateTime[] week = start.GeefWeek().Select(d => d.Date.Date).ToArray();
            return _dag.Include(p => p.Atelier_Dag).ThenInclude(d => d.Atelier)
                .Include(p => p.Atelier_Dag).ThenInclude(d => d.ADP).ThenInclude(q => q.Persoon)
                .Where(d => week.Contains(d.Date.Date)).ToList();
        }

        public IEnumerable<Dag> GetweekForClient(DateTime start, long id) {
            IEnumerable<Dag> week = Getweek(start.Date);
            Client cl = _clients.FirstOrDefault(c => c.ID == id);

            foreach (Dag d in week){
                List< Atelier_Dag > ad = new List<Atelier_Dag>();
                foreach (var ads in d.Atelier_Dag) {
                    if (ads.Clienten.Contains(cl))
                        ad.Add(ads);
                }
                d.Atelier_Dag = ad;
            }
            
            return week;
        }

        public IEnumerable<Dag> GetAll()
        {
           
            return _dag.ToList();
        }

      
    }
}
