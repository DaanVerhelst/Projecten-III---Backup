using KolveniershofAPI.Model;
using KolveniershofAPI.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Data.Repositories
{
    public class SfeergroepRepository : ISfeergroepRepository{
        private readonly DbSet<SfeerGroep> _sfeerGroepen;
        private readonly ApplicationDBContext _context;

        public SfeergroepRepository(ApplicationDBContext context){
            _sfeerGroepen = context.SfeerGroepen;
            _context = context;
        }

		public void Create(SfeerGroep sg)
		{
			_context.Add(sg);
		}

		public void Delete(SfeerGroep sg)
		{
			_context.Remove(sg);
		}

		public IEnumerable<SfeerGroep> GetAll(){
            return _sfeerGroepen.ToList();
        }

        public SfeerGroep GetById(int id){
            return _sfeerGroepen.FirstOrDefault(s => s.ID == id);
        }

		public void SaveChanges()
		{
			_context.SaveChanges();
		}

		public void Update(SfeerGroep sg)
		{
			_context.Update(sg);
		}
	}
}
