using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Interface{
    public interface IAtelierRepository{
        void Add(Atelier at);
        void Remove(Atelier at);
        IEnumerable<Atelier> GetAll();
        Atelier GetById(long id);
        Foto GetAtelierPicto(int id);
        void saveChanges();
        void Update(Atelier at);
    }
}
