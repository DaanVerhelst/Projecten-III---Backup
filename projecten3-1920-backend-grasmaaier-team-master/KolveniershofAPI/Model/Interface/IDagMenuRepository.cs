using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Interface
{
    public interface IDagMenuRepository
    {
        IEnumerable<DagMenu> GetAll();
        DagMenu getBy(long id);
        void Add(DagMenu menu);
        void Remove(DagMenu menu);
        void Update(DagMenu menu);
        void SaveChanges();
    }
}
