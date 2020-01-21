using KolveniershofAPI.Model;
using KolveniershofAPI.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KolveniershofAPI.Data.Repositories{
    public class TemplateRepository : ITemplateRepository{
        private readonly ApplicationDBContext _context;
        private readonly DbSet<DagTemplate> _template;

        public TemplateRepository(ApplicationDBContext context){
            _context = context;
            _template = context.Templates;
        }

        public void AddTemplate(DagTemplate temp){
            _template.Add(temp);
            _context.SaveChanges();
        }

        public DagTemplate GeefTemplate(int dag, int week) {
            return _template
                .Include(t=>t.Atelier_Dag).ThenInclude(ad=>ad.Atelier)
                .Include(t=>t.Atelier_Dag).ThenInclude(ad=>ad.ADP) .ThenInclude(adp=>adp.Persoon)
                .Include(t=>t.Menu)
                .FirstOrDefault(t => t.WeekNR == week && t.DagNR == dag);
        }

        public DagTemplate[] GeefWeekTemplate(int week) {
            return _template
                   .Include(t => t.Atelier_Dag).ThenInclude(ad => ad.Atelier)
                   .Include(t=> t.Atelier_Dag).ThenInclude(ad => ad.ADP).ThenInclude(adp => adp.Persoon)
                   .Where(dt => dt.WeekNR == week).ToArray(); 
        }

        public int[] GeefWeken(){
            return _template.Where(t=> !(t is Dag) && (((Dag)t).WeekNR!=0)).Select(t => t.WeekNR)
                .Distinct().ToArray();
        }

        public DagTemplate GetByMenu(int id)
        {
            return _template.FirstOrDefault(t => t.MenuID == id);
        }

        public void Verwijder(DagTemplate dt){
            _template.Remove(dt);
            _context.SaveChanges();
        }
    }
}
