using KolveniershofAPI.Model;
using KolveniershofAPI.Model.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Data.Repositories
{
    public class DagMenuRepository : IDagMenuRepository
    {

        private readonly ApplicationDBContext _context;
        private readonly DbSet<DagMenu> _menus;

        public DagMenuRepository(ApplicationDBContext context)
        {
            this._context = context;
            this._menus = _context.DagMenus;
        }

        public void Add(DagMenu menu)
        {
            _menus.Add(menu);
        }

        public IEnumerable<DagMenu> GetAll()
        {
            return _menus.ToList();
        }

        public DagMenu getBy(long id)
        {
            return _menus.FirstOrDefault(m => m.ID == id);
        }

        public void Remove(DagMenu menu)
        {
            _menus.Remove(menu);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Update(DagMenu menu)
        {
            _menus.Update(menu);
        }
    }
}
