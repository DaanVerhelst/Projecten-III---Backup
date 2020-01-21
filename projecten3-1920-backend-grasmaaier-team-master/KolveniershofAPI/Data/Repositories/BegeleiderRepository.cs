using KolveniershofAPI.Model;
using Microsoft.EntityFrameworkCore;
using KolveniershofAPI.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Data.Repositories
{
    public class BegeleiderRepository:IBegeleiderRepository{
        private readonly ApplicationDBContext _context;
        private readonly DbSet<Begeleider> _begeleiders;

        public BegeleiderRepository(ApplicationDBContext context){
            _context = context;
            _begeleiders = context.Begeleiders;
        }

        public void AddBegeleider(Begeleider begeleider)
        {
            _begeleiders.Add(begeleider);
        }

        public ICollection<Begeleider> GetAllBegeleiders(){
            return _begeleiders.ToList();
        }

        public Begeleider GetBegeleiderByID(long id){
            return _begeleiders
                .Include(e => e.Dag_Personen)
                .Include(e => e.ProfielFoto)
                .FirstOrDefault(b => b.ID == id);
        }

        public void RemoveBegeleider(Begeleider begeleider)
        {
            _begeleiders.Remove(begeleider);
        }

        public void saveChanges()
        {
            _context.SaveChanges();
        }

        public void UpdateBegeleider(Begeleider begeleider)
        {
            _begeleiders.Update(begeleider);
        }
    }
}
