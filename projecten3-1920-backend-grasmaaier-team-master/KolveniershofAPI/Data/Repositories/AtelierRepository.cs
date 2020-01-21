using System.Collections.Generic;
using System.Linq;
using KolveniershofAPI.Model;
using KolveniershofAPI.Model.DTO;
using KolveniershofAPI.Model.Interface;
using Microsoft.EntityFrameworkCore;

namespace KolveniershofAPI.Data.Repositories {
    public class AtelierRepository : IAtelierRepository{
        private readonly ApplicationDBContext _context;
        private readonly DbSet<Atelier> _ateliers;

        public AtelierRepository(ApplicationDBContext context){
            _context = context;
            _ateliers = context.Ateliers;
        }

        public void Add(Atelier at){
            _ateliers.Add(at);
       
        }

        public IEnumerable<Atelier> GetAll(){
            return _ateliers.ToList();
        }

        public Atelier GetById(long id){
            return _ateliers.Include(a=>a.AtelierDagen).FirstOrDefault(a => a.ID == id);    
        }

        public Foto GetAtelierPicto(int id) {
            Atelier at = _ateliers.Include(a => a.Pictogram).FirstOrDefault(a => a.ID == id);
            return at?.Pictogram;
        }

        public void Remove(Atelier at){
            _ateliers.Remove(at);
           
        }

		public void Update(Atelier at)
		{
			_context.Update(at);
		}

		public void saveChanges()
		{
			_context.SaveChanges();
		}
	}
}