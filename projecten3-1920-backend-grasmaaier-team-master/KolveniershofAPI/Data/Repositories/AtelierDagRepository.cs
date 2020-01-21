using KolveniershofAPI.Model.Interface;
using KolveniershofAPI.Model.Model_EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Data.Repositories{
    public class AtelierDagRepository : IAtelierDagRepository{
        private readonly DbSet<Atelier_Dag> _at_dag;
        private readonly DbContext _context;

        public AtelierDagRepository(ApplicationDBContext context){
            _context = context;
            _at_dag = context.Atelier_Dagen;
        }

        public void Delete(Atelier_Dag ad){
            _at_dag.Remove(ad);
            _context.SaveChanges();
        }
    }
}
