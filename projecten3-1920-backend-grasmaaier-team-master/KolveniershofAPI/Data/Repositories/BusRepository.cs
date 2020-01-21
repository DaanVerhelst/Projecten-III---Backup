using KolveniershofAPI.Model;
using KolveniershofAPI.Model.Interface;
using KolveniershofAPI.Model.Model_EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Data.Repositories
{
    public class BusRepository : IBusRepository
    {
        private readonly DbSet<Bus> _bussen;
        private readonly ApplicationDBContext _context;

        public BusRepository(ApplicationDBContext context)
        {
            _bussen = context.Bussen;
            _context = context;
        }

        public void Add(Bus b)
        {
            _bussen.Add(b);

        }

        public IEnumerable<Bus> GetAll()
        {
            return _bussen.ToList();
        }

        public Bus GetById(long id)
        {
            return _bussen.Include(b => b.BusDagen).FirstOrDefault(b => b.ID == id);
        }

        public Foto GetBusPicto(int id)
        {
            Bus b = _bussen.Include(bs => bs.Pictogram).FirstOrDefault(bs => bs.ID == id);
            return b?.Pictogram;
        }

        public void Remove(Bus b)
        {
            _bussen.Remove(b);

        }

        public void Update(Bus b)
        {
            _context.Update(b);
        }

        public void saveChanges()
        {
            _context.SaveChanges();
        }
    }
}
