using KolveniershofAPI.Model.Model_EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Interface
{
    public interface IBusRepository
    {
        void Add(Bus b);
        void Remove(Bus b);
        IEnumerable<Bus> GetAll();
        Bus GetById(long id);
        Foto GetBusPicto(int id);
        void saveChanges();
        void Update(Bus at);
    }
}
